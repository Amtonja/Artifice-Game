using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatPlayerUI : MonoBehaviour
{

    [SerializeField]
    private Player activePlayer;

    //All the various ui elements that need to be updated during battle
    private Image actionBar, specialBar, rageBar, magicBar, agilityBar, healthBar, healthBarDelta;
    private Text playerName, playerLevel;

    private Canvas iconCanvas;
    private GameObject cursor;
    private List<Player> enemiesList;
    private Player[] partyList;
    private Player selectedEnemy, selectedPlayer;

    /// <summary>
    /// Time (in seconds) it takes for the health bar "delta" to animate
    /// when health changes.
    /// </summary>
    public float healthBarDeltaTime;

    public enum PlayerUIState
    {
        WAITING_FOR_ACTION,
        ACTION_SELECT,
        ENEMY_SELECT,
        PLAYER_SELECT
    }
    private PlayerUIState state;

    // Use this for initialization
    void OnEnable()
    {
        playerName = transform.FindChild("PlayerName").GetComponent<Text>();
        playerLevel = transform.FindChild("PlayerLevel").GetComponent<Text>();

        actionBar = transform.FindChild("ActionBar/ActionBarOverlay").GetComponent<Image>();
        specialBar = transform.FindChild("SpecialBar/SpecialBarOverlay").GetComponent<Image>();
        rageBar = transform.FindChild("RageBar/SpecialBarOverlay").GetComponent<Image>();
        magicBar = transform.FindChild("MagicBar/SpecialBarOverlay").GetComponent<Image>();
        agilityBar = transform.FindChild("AgilityBar/SpecialBarOverlay").GetComponent<Image>();
        healthBar = transform.FindChild("HealthBar/GreenOverlay").GetComponent<Image>();
        healthBarDelta = transform.FindChild("HealthBar/DeltaOverlay").GetComponent<Image>();

        healthBarDelta.fillAmount = 1.0f;

        iconCanvas = GetComponentInChildren<Canvas>();
        iconCanvas.enabled = false;

        cursor = transform.Find("Cursor").gameObject;

        enemiesList = PlayManager.instance.EnemyCombatants;
        partyList = PlayManager.instance.party;
        selectedEnemy = enemiesList[0];
        selectedPlayer = partyList[0];

        State = PlayerUIState.WAITING_FOR_ACTION;
    }

    // Update is called once per frame
    void Update()
    {
        if (State == PlayerUIState.WAITING_FOR_ACTION)
        {
            if (ActivePlayer.IsMyTurn)
            {
                State = PlayerUIState.ACTION_SELECT;
                return;
            }
            // Update the five bars
            actionBar.fillAmount = ActivePlayer.ActionBarValue / ActivePlayer.ActionBarTarget;
            agilityBar.fillAmount = ActivePlayer.AgilityBarValue / ActivePlayer.AgilityBarTarget;
            magicBar.fillAmount = ActivePlayer.MagicBarValue / ActivePlayer.MagicBarTarget;
            rageBar.fillAmount = ActivePlayer.RageBarValue / ActivePlayer.RageBarTarget;
            specialBar.fillAmount = ActivePlayer.SpecialBarValue / ActivePlayer.SpecialBarTarget;
        }        

        // Happening in any state
        if (ActivePlayer.HealthChanged)
        {
            UpdateHealthBar();
        }

        if (State == PlayerUIState.ENEMY_SELECT)
        {
            //if (Input.GetAxisRaw("Vertical") < 0f)
            if (Input.GetKeyDown(KeyCode.DownArrow)) // only for video recording
            {
                selectedEnemy = enemiesList[(enemiesList.IndexOf(selectedEnemy) + 1) % enemiesList.Count];
                cursor.transform.position = selectedEnemy.transform.position;
            }

            //if (Input.GetAxisRaw("Vertical") > 0f)
            if (Input.GetKeyDown(KeyCode.UpArrow)) // Same
            {
                selectedEnemy = enemiesList[Mathf.Abs((enemiesList.IndexOf(selectedEnemy) - 1) % enemiesList.Count)];
                cursor.transform.position = selectedEnemy.transform.position;
            }

            if (Input.GetButtonDown("Submit"))
            {
                ActivePlayer.MyCombatAction(selectedEnemy);
                State = PlayerUIState.WAITING_FOR_ACTION;
            }

            if (Input.GetButtonDown("Cancel"))
            {
                State = PlayerUIState.ACTION_SELECT;
            }
        }

        if (State == PlayerUIState.PLAYER_SELECT)
        {
            if (Input.GetAxisRaw("Vertical") < 0f)
            {
                selectedPlayer = partyList[(System.Array.IndexOf(partyList, selectedPlayer) + 1) % partyList.Length];
                cursor.transform.position = selectedPlayer.transform.position;
            }

            if (Input.GetAxisRaw("Vertical") > 0f)
            {
                selectedPlayer = partyList[Mathf.Abs((System.Array.IndexOf(partyList, selectedPlayer) - 1) % partyList.Length)];
                cursor.transform.position = selectedPlayer.transform.position;
            }

            if (Input.GetButtonDown("Submit"))
            {
                ActivePlayer.MyCombatAction(selectedPlayer);
                State = PlayerUIState.WAITING_FOR_ACTION;
            }

            if (Input.GetButtonDown("Cancel"))
            {
                State = PlayerUIState.ACTION_SELECT;
            }
        }
    }

    /// <summary>
    /// Update the ActivePlayer's health bar, changing the green bar
    /// immediately and the red delta bar gradually
    /// </summary>
    private void UpdateHealthBar()
    {
        float oldFillAmount = healthBar.fillAmount;
        healthBar.fillAmount = (float)ActivePlayer.Health / (float)ActivePlayer.Stats.MaxHealth;
        StartCoroutine(AnimateHealthBarDelta(oldFillAmount, healthBar.fillAmount, healthBarDeltaTime));
        ActivePlayer.HealthChanged = false;
    }

    /// <summary>
    /// Coroutine to change the value of the health delta bar over (time) seconds
    /// </summary>
    /// <param name="startValue">Initial fill amount of the bar</param>
    /// <param name="endValue">Target fill amount of the bar</param>
    /// <param name="time">Total time in seconds the change will take</param>
    /// <returns></returns>
    private IEnumerator AnimateHealthBarDelta(float startValue, float endValue, float time)
    {
        float elapsedTime = 0f;
        healthBarDelta.fillAmount = startValue;

        while (elapsedTime < time)
        {
            healthBarDelta.fillAmount = Mathf.Lerp(healthBarDelta.fillAmount, endValue, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void OpenSubmenu(GameObject icon)
    {
        GameObject submenu = icon.transform.Find("OptionPanel").gameObject;
        if (submenu != null) submenu.SetActive(true);
    }

    public void CloseSubmenu(GameObject icon)
    {
        GameObject submenu = icon.transform.Find("OptionPanel").gameObject;
        if (submenu != null) submenu.SetActive(false);
    }

    /// <summary>
    /// Handles the UI event of focus moving FROM one of the 
    /// 5 top-level icons horizontally.
    /// </summary>
    /// <param name="eventData">Data of the Move event sent by the event system.</param>
    public void OnMoveIcon(BaseEventData eventData)
    {
        AxisEventData axisData = eventData as AxisEventData;
        MoveDirection moveDir = axisData.moveDir;
        if (moveDir == MoveDirection.Left)
        {
            // Here's hoping this nightmare is a workaround for EventSystem.current.lastSelectedGameObject
            // Point is, close the submenu of whichever icon we are moving AWAY FROM
            CloseSubmenu(axisData.selectedObject.GetComponent<Button>().FindSelectableOnRight().gameObject);
        }
        else if (moveDir == MoveDirection.Right)
        {
            // Same, but opposite direction
            CloseSubmenu(axisData.selectedObject.GetComponent<Button>().FindSelectableOnLeft().gameObject);
        }
    }

    public void OnMelee()
    {
        Debug.Log("You have selected: Melee Attack");
        ActivePlayer.MyCombatAction = ActivePlayer.MeleeAttack;
        Input.ResetInputAxes();
        State = PlayerUIState.ENEMY_SELECT;
    }

    public void OnRanged()
    {
        ActivePlayer.MyCombatAction = ActivePlayer.RangedAttack;
        Input.ResetInputAxes();
        State = PlayerUIState.ENEMY_SELECT;
    }

    public void OnBolt()
    {
        ActivePlayer.MyCombatAction = ActivePlayer.BoltSpell;
        Input.ResetInputAxes();
        State = PlayerUIState.ENEMY_SELECT;
    }

    public void OnGust()
    {
        ActivePlayer.MyCombatAction = ActivePlayer.GustSpell;
        Input.ResetInputAxes();
        State = PlayerUIState.ENEMY_SELECT;
    }

    public void OnCure()
    {
        ActivePlayer.MyCombatAction = ActivePlayer.CureSpell;
        Input.ResetInputAxes();
        State = PlayerUIState.PLAYER_SELECT;
    }

    public void OnAim()
    {
        ActivePlayer.MyCombatAction = ActivePlayer.AimSpell;
        Input.ResetInputAxes();
        State = PlayerUIState.PLAYER_SELECT;
    }

    #region C# Properties
    /// <summary>
    /// Sets the active player, and also updates the
    /// UI to initially display info about the character.
    /// </summary>
    /// <value>The active player.</value>
    public Player ActivePlayer
    {
        set
        {
            activePlayer = value;
            playerName.text = activePlayer.Name;
            playerLevel.text = activePlayer.Stats.Level.ToString("00");
            transform.position = activePlayer.transform.position + Vector3.up / 2f; //+ Vector3.up * 2f + Vector3.right / 2f;
        }
        get
        {
            return activePlayer;
        }
    }

    public PlayerUIState State
    {
        get
        {
            return state;
        }

        set
        {
            PlayerUIState oldState = state;
            state = value;
            if (state != oldState)
            {
                switch (value)
                {
                    case PlayerUIState.ACTION_SELECT:
                        cursor.SetActive(false);
                        iconCanvas.enabled = true;
                        EventSystem.current.sendNavigationEvents = true;
                        EventSystem.current.SetSelectedGameObject(iconCanvas.transform.Find("ActionIcon").gameObject);
                        break;
                    case PlayerUIState.ENEMY_SELECT:
                        EventSystem.current.sendNavigationEvents = false;
                        cursor.transform.position = selectedEnemy.transform.position;
                        cursor.SetActive(true);
                        break;
                    case PlayerUIState.PLAYER_SELECT:
                        EventSystem.current.sendNavigationEvents = false;
                        cursor.transform.position = selectedPlayer.transform.position;
                        cursor.SetActive(true);
                        break;
                    case PlayerUIState.WAITING_FOR_ACTION:
                        iconCanvas.enabled = false;
                        EventSystem.current.sendNavigationEvents = true;
                        cursor.SetActive(false);
                        break;
                } // end switch
            } // end if
        } // end set
    } // end property State
    #endregion
}
