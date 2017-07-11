using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{

    /// <summary>
    /// This is a reference to the gameobject that should be spawned above
    /// each combatant when we enter combat. This is set up in Start()
    /// </summary>
    private GameObject combatPlayerUI;

    /// <summary>
    /// This list is populated when the PlayManager indicates we have entered combat
    /// </summary>
    private List<CombatPlayerUI> combatants;


    void Start()
    {
        combatPlayerUI = transform.FindChild("Canvas/PlayerUI").gameObject;
        combatants = new List<CombatPlayerUI>();
    }

    public void SetupPlayerUI(Player p)
    {
        //First check if we can reuse an existing piece of UI
        for (int i = 0; i < combatants.Count; i++)
        {
            if (combatants[i].ActivePlayer == null)
            {
                combatants[i].gameObject.SetActive(true);
                combatants[i].ActivePlayer = p;
                return;
            }
        }

        //If not, we make a new one
        GameObject combatUI = GameObject.Instantiate(combatPlayerUI, transform.GetChild(0));        
        //combatUI.transform.localScale = Vector3.one;
        combatants.Add(combatUI.GetComponent<CombatPlayerUI>());
        combatUI.SetActive(true);
        combatUI.GetComponent<CombatPlayerUI>().ActivePlayer = p;        
    }

    public void DeactivatePlayerUI()
    {
        for (int i = 0; i < combatants.Count; i++)
        {
            combatants[i].gameObject.SetActive(false);
        }
    }
}
