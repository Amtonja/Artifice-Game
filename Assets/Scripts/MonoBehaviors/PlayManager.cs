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

    void Start()
    {
        exploreMode = true;
        instance = this;
        combatantEnemies = new List<Player>();
        combatGrid = GameObject.FindGameObjectWithTag("CombatGrid").GetComponent<CombatGrid>();        
        combatGrid.gameObject.SetActive(false);
    }

    /// <summary>
    /// This is called when a player controlled character encounters an enemy.
    /// This is currently only being called by OnTriggerEnter through the Player
    /// script attached to a player controlled character.
    /// </summary>
    /// <param name="enemy">Enemy.</param>
    public void EnemyEncountered(Player enemy)
    {
        exploreMode = false;
        combatGrid.gameObject.SetActive(true);

        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemiesInScene)
        {
            combatantEnemies.Add(e.GetComponent<Player>());
        }
        SetupCombatPositions(combatantEnemies);

        for (int i = 0; i < combatantEnemies.Count; i++)
        {
            combatantEnemies[i].EnterCombat();
        }
        for (int i = 0; i < party.Length; i++)
        {
            party[i].EnterCombat();
            combatUI.SetupPlayerUI(party[i]);
        }        
    }

    /// <summary>
    /// Moves the players and combatants to their appropriate battle positions as
    /// set up in the CombatGrid script. 
    /// </summary>
    /// <param name="enemy">Enemy.</param>
    private void SetupCombatPositions(List<Player> enemies)
    {        
        combatGrid.GenerateGrid();
        combatGrid.gameObject.SetActive(true);

        // Set each player's position to the given cell of the grid, offset for their sprite's height        
        for (int i = 0; i < party.Length; i++)
        {
            Vector3 startPosition =
                new Vector3(combatGrid.playerStartPositions[i].x * combatGrid.CellSizeX, 
                combatGrid.playerStartPositions[i].y * combatGrid.CellSizeY - party[i].FootPos.y);
            party[i].transform.position = combatGrid.transform.position + startPosition;
        }        

        // Do the same as above for the enemies
        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 startPosition =
                new Vector3(combatGrid.enemyStartPositions[i].x * combatGrid.CellSizeX, 
                combatGrid.enemyStartPositions[i].y * combatGrid.CellSizeY - enemies[i].FootPos.y);
            enemies[i].transform.position = combatGrid.transform.position + startPosition; 
        }
    }

    public void CreatePopupText(string text, Transform location)
    {
        PopupText popup = Instantiate(popupTextPrefab, combatUI.transform.Find("Canvas"), false);
        popup.GetComponentInChildren<UnityEngine.UI.Text>().text = text;        
        popup.transform.position = location.position;
    }

    /// <summary>
    /// Call this when the encounter is complete.
    /// </summary>
    public void EncounterComplete()
    {
        exploreMode = true;

        for (int i = 0; i < party.Length; i++)
        {
            party[i].ExitCombat();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }

    #region C# Properties
    public List<Player> EnemyCombatants
    {
        get
        {
            return combatantEnemies;
        }
    }

    public bool ExploreMode
    {
        get
        {
            return exploreMode;
        }
    }    
    #endregion
}
