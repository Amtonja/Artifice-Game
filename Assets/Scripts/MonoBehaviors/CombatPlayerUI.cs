using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatPlayerUI : MonoBehaviour {

    [SerializeField]
	private Player activePlayer;

	//All the various ui elements that need to be updated during battle
	private Image actionBar, specialBar, rageBar, magicBar, agilityBar, healthBar, healthBarDelta;
	private Text playerName, playerLevel;    
        
    public float healthBarDeltaTime;
    
	// Use this for initialization
	void OnEnable () {
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
    }
	
	// Update is called once per frame
	void Update () {
        if (ActivePlayer.HealthChanged)
        {
            UpdateHealthBar();
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
        //Debug.Log("Started coroutine AnimateHealthBarDelta");

        float elapsedTime = 0f;
        healthBarDelta.fillAmount = startValue;

        while (elapsedTime < time)
        {
            healthBarDelta.fillAmount = Mathf.Lerp(healthBarDelta.fillAmount, endValue, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
}

	/// <summary>
	/// Sets the active player, and also updates the
	/// UI to initially display info about the character.
	/// </summary>
	/// <value>The active player.</value>
	public Player ActivePlayer {
		set {
			activePlayer = value;
			playerName.text = activePlayer.Name;
			playerLevel.text = activePlayer.Stats.Level.ToString("00");
			transform.position = activePlayer.transform.position + Vector3.up*2f + Vector3.right/2f;
		}
		get {
			return activePlayer;
		}
	}
}
