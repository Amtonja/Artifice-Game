using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Artifice.Characters;

public class CombatPlayerUI : MonoBehaviour
{

    [SerializeField]
    private Player activePlayer;

    //All the various ui elements that need to be updated during battle
    private Image actionBar, specialBar, rageBar, magicBar, agilityBar, healthBar, healthBarDelta;
    private Text playerName, playerLevel;

    //private Canvas iconCanvas;
    private GameObject iconPanel;
    private GameObject cursor;
    private List<Player> enemiesList;
    private Player[] partyList;
    private Player selectedEnemy, selectedPlayer;

    private float targetSelectionTimer;
    public float targetSelectionDelay;
    
    public enum PlayerUIState
    {
        WAITING_FOR_TURN,
        ACTION_SELECT,
        SUB_ACTION_SELECT,
        ENEMY_SELECT,
        PLAYER_SELECT
    }
    private PlayerUIState state;

    public float verticalOffset;

    // Use this for initialization
    void OnEnable()
    {
        iconPanel = transform.Find("IconPanel").gameObject;
        iconPanel.SetActive(false);

        cursor = transform.Find("Cursor").gameObject;

        enemiesList = PlayManager.instance.EnemyCombatants;
        partyList = PlayManager.instance.party;
        selectedEnemy = enemiesList[0];
        selectedPlayer = partyList[0];        

        State = PlayerUIState.WAITING_FOR_TURN;
    }

    // Update is called once per frame
    void Update()
    {
        //reset enemy cursor position if that enemy is dead
        if (enemiesList.Count != 0 && !enemiesList.Contains(selectedEnemy))
        {
            selectedEnemy = enemiesList[0];
            //MoveCursor(selectedEnemy);
        }


        if (State == PlayerUIState.WAITING_FOR_TURN)
        {
            if (ActivePlayer.IsMyTurn)
            {
                State = PlayerUIState.ACTION_SELECT;
                return;
            }            
        }        

        if (State == PlayerUIState.ENEMY_SELECT)
        {
            targetSelectionTimer += Time.deltaTime;

            if (Input.GetAxisRaw("Vertical") < 0f && targetSelectionTimer >= targetSelectionDelay)
            {
                selectedEnemy = enemiesList[(enemiesList.IndexOf(selectedEnemy) + 1) % enemiesList.Count];
                MoveCursor(selectedEnemy);
                targetSelectionTimer = 0;
            }

            if (Input.GetAxisRaw("Vertical") > 0f && targetSelectionTimer >= targetSelectionDelay)
            {
                selectedEnemy = enemiesList.IndexOf(selectedEnemy) == 0 ? 
                    enemiesList[enemiesList.Count - 1] : enemiesList[enemiesList.IndexOf(selectedEnemy) - 1];
                MoveCursor(selectedEnemy);
                targetSelectionTimer = 0;
            }

            if (Input.GetButtonDown("Submit"))
            {
                ActivePlayer.MyCombatAction(selectedEnemy);
                State = PlayerUIState.WAITING_FOR_TURN;
            }

            if (Input.GetButtonDown("Cancel"))
            {
                State = PlayerUIState.SUB_ACTION_SELECT;
            }
        }

        if (State == PlayerUIState.PLAYER_SELECT)
        {
            targetSelectionTimer += Time.deltaTime;

            if (Input.GetAxisRaw("Vertical") < 0f && targetSelectionTimer >= targetSelectionDelay)
            {
                selectedPlayer = partyList[(System.Array.IndexOf(partyList, selectedPlayer) + 1) % partyList.Length];
                MoveCursor(selectedPlayer);
                targetSelectionTimer = 0;
            }

            if (Input.GetAxisRaw("Vertical") > 0f && targetSelectionTimer >= targetSelectionDelay)
            {
                selectedPlayer = System.Array.IndexOf(partyList, selectedPlayer) == 0 ?
                    partyList[partyList.Length - 1] : partyList[System.Array.IndexOf(partyList, selectedPlayer) - 1];
                MoveCursor(selectedPlayer);
                targetSelectionTimer = 0;
            }

            if (Input.GetButtonDown("Submit"))
            {
                ActivePlayer.MyCombatAction(selectedPlayer);
                State = PlayerUIState.WAITING_FOR_TURN;
            }

            if (Input.GetButtonDown("Cancel"))
            {
                State = PlayerUIState.SUB_ACTION_SELECT;
            }
        }
    }

    void MoveCursor(Player target)
    {
        cursor.GetComponent<CombatCursor>().SelectedCharacter = target;
    }

    public void CloseAllSubmenus()
    {
        Transform icons = iconPanel.transform;
        for (int i = 0; i < icons.childCount; i++)
        {
            Transform currentIcon = icons.GetChild(i);
            if (currentIcon.Find("OptionPanel") != null)
            {
                currentIcon.GetComponent<ActionIcon>().CloseSubmenu();
            }
        }
    }

    public void OnPiercing()
    {        
        ActivePlayer.MyCombatAction = ActivePlayer.PiercingAttack;
        Input.ResetInputAxes();
        State = PlayerUIState.ENEMY_SELECT;
    }

    public void OnProjectile()
    {
        ActivePlayer.MyCombatAction = ActivePlayer.ProjectileAttack;
        Input.ResetInputAxes();
        State = PlayerUIState.ENEMY_SELECT;
    }

    public void OnBlunt()
    {
        ActivePlayer.MyCombatAction = ActivePlayer.BluntAttack;
        Input.ResetInputAxes();
        State = PlayerUIState.ENEMY_SELECT;
    }

    public void OnBolt()
    {
        ActivePlayer.MySpell = ActivePlayer.BoltSpell;
        ActivePlayer.MyCombatAction = ActivePlayer.BeginSpellCast;
        Input.ResetInputAxes();
        State = PlayerUIState.ENEMY_SELECT;
    }

    public void OnGust()
    {
        ActivePlayer.MySpell = ActivePlayer.GustSpell;
        ActivePlayer.MyCombatAction = ActivePlayer.BeginSpellCast;
        Input.ResetInputAxes();
        State = PlayerUIState.ENEMY_SELECT;
    }

    public void OnCure()
    {
        ActivePlayer.MySpell = ActivePlayer.CureSpell;
        ActivePlayer.MyCombatAction = ActivePlayer.BeginSpellCast;
        Input.ResetInputAxes();
        State = PlayerUIState.PLAYER_SELECT;
    }

    public void OnAim()
    {
        ActivePlayer.MySpell = ActivePlayer.AimSpell;
        ActivePlayer.MyCombatAction = ActivePlayer.BeginSpellCast;
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
            transform.position = Camera.main.WorldToScreenPoint(ActivePlayer.transform.position + Vector3.down * verticalOffset) ;
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
                        Debug.Log(ActivePlayer.Stats.characterName + " taking a turn");
                        cursor.SetActive(false);
                        iconPanel.SetActive(true);
                        EventSystem.current.sendNavigationEvents = true;
                        EventSystem.current.SetSelectedGameObject(iconPanel.transform.Find("AttackIcon").gameObject);
                        break;
                    case PlayerUIState.SUB_ACTION_SELECT:
                        cursor.SetActive(false);
                        iconPanel.SetActive(true);
                        EventSystem.current.sendNavigationEvents = true;                        
                        break;
                    case PlayerUIState.ENEMY_SELECT:
                        EventSystem.current.sendNavigationEvents = false;                        
                        cursor.SetActive(true);
                        MoveCursor(selectedEnemy);                       
                        break;
                    case PlayerUIState.PLAYER_SELECT:
                        EventSystem.current.sendNavigationEvents = false;
                        cursor.SetActive(true);
                        MoveCursor(selectedPlayer);                  
                        break;
                    case PlayerUIState.WAITING_FOR_TURN:
                        CloseAllSubmenus();
                        iconPanel.SetActive(false);
                        EventSystem.current.sendNavigationEvents = true;
                        cursor.SetActive(false);
                        break;
                } // end switch
            } // end if
        } // end set
    } // end property State
    #endregion
}
