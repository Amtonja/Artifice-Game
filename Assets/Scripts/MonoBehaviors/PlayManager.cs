﻿using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The singleton manager for the current play session
/// This is more specific than the GameManager and non-persistent
/// </summary>
public class PlayManager : MonoBehaviour
{

    /// <summary>
    /// Currently only 2 states the game can be in, explore or combat
    /// If exploreMode is true, we are exploring
    /// If false, we are in combat
    /// </summary>
    private bool exploreMode;

    /// <summary>
    /// Singleton instance
    /// </summary>
    public static PlayManager instance;

    /// <summary>
    /// Reference to the combat UI manager, so that when combat
    /// begins, the UI will automatically show all the characters info.
    /// </summary>
    public CombatUIManager combatUI;

    /// <summary>
    /// Reference to the CombatGrid object so that it can be
    /// inactive initially and activated when combat begins.
    /// </summary>
    private CombatGrid combatGrid;

    /// <summary>
    /// Prefab used to display damage/healing in combat.
    /// </summary>
    public PopupText popupTextPrefab;

    /// <summary>
    /// The current players in the party
    /// </summary>
    public Player[] party;

    /// <summary>
    /// The enemies the player is currently engaged with.
    /// Usually this list is empty unless in combat.
    /// </summary>
    private List<Player> combatantEnemies;    

	/// <summary>
	/// Game's timescale for combat. Used instead of modifying Time.timescale so we can pause things during combat
	/// but still have timers and such moving.
	/// </summary>
	private bool pauseCombat = true;

	/// <summary>
	/// Combat states for... well, combat.
	/// </summary>
	private enum combatState {Init, CombatLoop, WaitForAttackConfirmation, Formation, EndCombat};
	private combatState state = combatState.Init;

	//When characters attack, they send this script the number of characters being attacked, so we can wait until they're all damaged
	//Not exactly needed yet, but this is where we'd hook in having AoE attacks later.
	private int attackedCountMax = 99999;
	private int attackedCountCurrent = 0;

	/// <summary>
	/// Small buffer after something gets hit to slow things down a bit.
	/// </summary>
	private float hitWaitTimer = 1f;
	private float hitWaitCurrent = 0f;

    /// <summary>
    /// The current scene's background image.
    /// </summary>
    public SpriteRenderer background;

    public int experiencePool;

    void Start()
    {
        exploreMode = true;
        instance = this;
        combatantEnemies = new List<Player>();
//        combatGrid = GameObject.FindGameObjectWithTag("CombatGrid").GetComponent<CombatGrid>();        
//        combatGrid.gameObject.SetActive(false);
    }



	void Update(){
		//This update loop contributes exclusively towards combat and combat-related processes.
		if (exploreMode) {
			if(Input.GetKeyDown(KeyCode.Return)){
				InteractCheck ();
			}

//			Player target = party [0];
//			Transform basePoint = target.transform.FindChild ("Base");
//			var ray = new Vector2 (0, 0);
//			ray = new Vector2 (basePoint.transform.position.x, basePoint.transform.position.y + 0.1f); //the base transform is the bottom of feet, need offset
//			//Get delta
//			Vector2 delta = target.GetComponent<Movement>().DirVector;
//			//get position. This should be pulled out of this for optimisation later
//
//			Debug.DrawRay( ray, delta * 0.2f, Color.red );

			return;
		}

		if (state == combatState.Init) {
			//Init phase. Anything pre-combat goes here. Mostly a stub, as things are currently set up by Enemy Encountered when 
			//a Battleground is ready.


			//turn on combat
			pauseCombat = false;
			state = combatState.CombatLoop;
		}

		//standard combat loop. Each character is slowly building up their ATB.
		//At present, it kind of just waits here until outside forces (the interface and Player.cs commands)
		//tell it to do otherwise.
		else if (state == combatState.CombatLoop) {



		}

		//Waiting for the attack to resolve - animations, hit frame, and damage. Checks for downed monsters, removes them from attackable list.
		//If attackabl list is now zero, moves ro end combat. Otherwise goes to combat loop.
		 else if (state == combatState.WaitForAttackConfirmation) {
			if (attackedCountCurrent >= attackedCountMax) {
				hitWaitCurrent += Time.deltaTime;
				if (hitWaitCurrent >= hitWaitTimer) {
					hitWaitCurrent = 0f;
					pauseCombat = false;                   
					state = combatState.CombatLoop;
				}
			}


		}

		//Waiting for confirmation from Battleground instance that a new formation position has been set. Returns to CombatLoop.
		else if (state == combatState.Formation) {



		}

		else if (state == combatState.EndCombat) {
			//ends combat. Gives players EXP, items, whatever, and returns to explore mode.
			exploreMode = true;
//			foreach (Player dudebro in Party) {
//				dudebro.ExitCombat ();
//			}
			EncounterComplete();
		}

	}

	/// <summary>
	/// Checks to see if there's an interactive CM object in front of us. If so, send it a pulse.
	/// Holy crap this needs to be optimised later but time is an issue right now.
	/// </summary>
	private void InteractCheck(){
		Player target = party [0];
		Transform basePoint = target.transform.FindChild ("Base");
		var ray = new Vector2 (0, 0);
		ray = new Vector2 (basePoint.transform.position.x, basePoint.transform.position.y + 0.1f); //the base transform is the bottom of feet, need offset
		//Get delta
		Vector2 delta = target.GetComponent<Movement>().DirVector;
		//get position. This should be pulled out of this for optimisation later

		Debug.DrawRay( ray, delta * 0.2f, Color.red );

		LayerMask mask = (1 << 10);
		RaycastHit2D _raycastHit;
		_raycastHit = Physics2D.Raycast( ray, delta, 0.2f, mask); //9 is interact layer, ~ means only focus on that
		if (_raycastHit) {
			Debug.Log ("We hit " + _raycastHit.collider.name.ToString () + " and its layer is " + _raycastHit.collider.gameObject.layer.ToString());
			_raycastHit.collider.gameObject.SendMessage ("Activate");
		} 
	}


    /// <summary>
    /// This is called when a player controlled character encounters an enemy.
    /// This is currently only being called when a player wanders into a trigger used by a Battleground script,
    /// and is called after every charcter is in position for the fight.
    /// </summary>
    /// <param name="enemy">Enemy.</param>
	public void EnemyEncountered()//Player enemy)
    {
        exploreMode = false;
//        combatGrid.gameObject.SetActive(true);

//        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
//        foreach (GameObject e in enemiesInScene)
//        {
//            combatantEnemies.Add(e.GetComponent<Player>());
//        }
//        SetupCombatPositions(combatantEnemies);

        for (int i = 0; i < combatantEnemies.Count; i++)
        {
            combatantEnemies[i].EnterCombat();
        }
        for (int i = 0; i < party.Length; i++)
        {
            party[i].EnterCombat();
            combatUI.SetupPlayerUI(party[i]);
        }        

		//move to correct combat state
		state = combatState.Init;

        MusicManager.instance.StartCoroutine("PlayCombatMusic");

        experiencePool = 0;
    }

    /// <summary>
    /// Moves the players and combatants to their appropriate battle positions as
    /// set up in the CombatGrid script. 
	/// 
	/// Now completely uneeded, as this is handled via each Battleground script, since positions are local 
	/// to that script's instance.
    /// </summary>
    /// <param name="enemy">Enemy.</param>
    private void SetupCombatPositions(List<Player> enemies)
    {        
//        combatGrid.GenerateGrid();
//        combatGrid.gameObject.SetActive(true);

        // Set each player's position to the given cell of the grid, offset for their sprite's height        
//        for (int i = 0; i < party.Length; i++)
//        {
//            Vector3 startPosition =
//                new Vector3(combatGrid.playerStartPositions[i].x * combatGrid.CellSizeX, 
//                combatGrid.playerStartPositions[i].y * combatGrid.CellSizeY - party[i].FootPos.y);
//            party[i].transform.position = combatGrid.transform.position + startPosition;
//        }        
//		party[1].

        // Do the same as above for the enemies
//        for (int i = 0; i < enemies.Count; i++)
//        {
//            Vector3 startPosition =
//                new Vector3(combatGrid.enemyStartPositions[i].x * combatGrid.CellSizeX, 
//                combatGrid.enemyStartPositions[i].y * combatGrid.CellSizeY - enemies[i].FootPos.y);
//            enemies[i].transform.position = combatGrid.transform.position + startPosition; 
//        }
    }

    public void CreatePopupText(string text, Transform location, Color textColor)
    {
        PopupText popup = Instantiate(popupTextPrefab, combatUI.transform.Find("Canvas"), false);   
        popup.GetComponentInChildren<UnityEngine.UI.Text>().text = text;
        popup.GetComponentInChildren<UnityEngine.UI.Text>().color = textColor;
        popup.transform.position = location.position;
    }

    /// <summary>
    /// Call this when the encounter is complete.
    /// </summary>
    public void EncounterComplete()
    {
        exploreMode = true;

        MusicManager.instance.PlayCombatEnding();

        DisplayCombatRewards(2f);

        for (int i = 0; i < party.Length; i++)
        {
            party[i].ExitCombat();
            party[i].AddExperience(experiencePool);   
			//Reset characters following lead character
//			if (i != 0) {
//				GameObject dude = party [i].gameObject;
//				dude.gameObject.GetComponent<Movement> ().FollowTarget = party [0];
//			}
        }

        //turn off combat UI
        combatUI.DeactivatePlayerUI();
        experiencePool = 0;
    }

    public void DisplayCombatRewards(float messageDuration)
    {
        //float messageDuration = 2f;
        for (int i = 0; i < party.Length; i++)
        {
            DialogueManager.ShowAlert(party[i].name + " gained " + experiencePool + " XP!", messageDuration);
        }        
    }

    public void PauseGame()
    {
//        Time.timeScale = 0f;
//		combatTimeScale = 0.00000001f; //you probably don't want this to be zero
		//combat
		pauseCombat = true;

    }

    public void UnpauseGame()
    {
//        Time.timeScale = 1f;
//		combatTimeScale = 1f;
		pauseCombat = false;
    }

	//when a character attacks, it sends how many characters it's attacking. This is mostly a stub for fure AoE attacks
	public void SendAttackCount(int count){
		attackedCountMax = count;
		attackedCountCurrent = 0;

	}
	//called by a character when it is attacked
	public void UpdateAttacked()
    {
		attackedCountCurrent++;
	}

	//called when a character's ATB is full to change states into WaitingForAttackConfirmation
	public void StartingAttack(){
		state = combatState.WaitForAttackConfirmation;

	}

    /// <summary>
    /// Called to darken or un-darken the background image of this scene to increase/restore contrast with foreground
    /// elements.
    /// </summary>
    /// <param name="darken">Darken if true, re-lighten otherwise</param>
    public void DarkenBG(bool darken)
    {
        if (darken)
        {
            background.color = Color.gray;
        }
        else
        {
            background.color = Color.white;
        }
    }
	//Called when an enemy's HP is at zero
	public void RemoveEnemy(Player enemy){
		EnemyCombatants.Remove (enemy);
		if (EnemyCombatants.Count == 0) {

			Debug.Log ("Enemies killed!");
			state = combatState.EndCombat;
		}

	}
	//Called when a player's HP is at zero
	public void HeroDown(Player hero){

	}

    #region C# Properties
    public List<Player> EnemyCombatants
    {
        get
        {
            return combatantEnemies;
        }
		set{
			combatantEnemies = value;
		}
    }

    public bool ExploreMode
    {
        get
        {
            return exploreMode;
        }
		set {
			exploreMode = value;
		}
    }    

	public Player[] Party {
		get {
			return party;
		}
		set {
			party = value;
		}
	}

	public bool PauseCombat {
		get {
			return pauseCombat;
		}
		set {
			pauseCombat = value;
		}
	}
    #endregion
}
