﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Artifice.Characters;

public class ActionIcon : MonoBehaviour
{ 
    private List<GameObject> subActions = new List<GameObject>();
    private GameObject subActionPanel;
    private GameObject subActionPrefab;
    private CombatPlayerUI parentUI;
    private Player player;    
    
    // Use this for initialization
    void Start()
    {
        subActionPanel = transform.Find("OptionPanel").gameObject;        

        subActionPrefab = Resources.Load("Prefabs/SubActionPrefab") as GameObject;
        parentUI = GetComponentInParent<CombatPlayerUI>();
        player = parentUI.ActivePlayer;        

        GetPlayerActions();
    }

    public void OpenSubmenu()
    {
        ArrangeSubActions();
        if (subActionPanel != null) subActionPanel.SetActive(true);
        if (subActions.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(subActions[0].gameObject);
        }
    }

    public void CloseSubmenu()
    {
        if (subActionPanel != null) subActionPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void AddSubAction(CombatAction action)
    {
        GameObject newAction = Instantiate(subActionPrefab, subActionPanel.transform, false);
        newAction.GetComponent<SubActionButton>().action = action;
        newAction.GetComponent<TextMeshProUGUI>().text = action.name;
        subActions.Add(newAction);
    }

    public void ArrangeSubActions()
    {
        float angle = 2f * Mathf.PI / subActions.Count;
        float radius = 120f;

        for (int i = 0; i < subActions.Count; i++)
        {
            subActions[i].transform.localPosition = new Vector2(radius * Mathf.Cos(i * angle), radius * Mathf.Sin(i * angle));
            // Set up circular navigation among the options
            Navigation navigation = subActions[i].GetComponent<Button>().navigation;
            navigation.mode = Navigation.Mode.Explicit;
            // The modulo operator doesn't work for this in one of the directions
            navigation.selectOnRight = i == 0 ? subActions[subActions.Count - 1].GetComponent<Button>() : subActions[i - 1].GetComponent<Button>();
            navigation.selectOnLeft = subActions[(i + 1) % subActions.Count].GetComponent<Button>();
            subActions[i].GetComponent<Button>().navigation = navigation;
        }
    }

    public void GetPlayerActions()
    {
        if (name == "AttackIcon")
        {
            foreach (WeaponAttack attack in player.Stats.weaponAttacks)
            {
                AddSubAction(attack);
            }
        }

        if (name == "MagicIcon")
        {
            foreach (Spell spell in player.Stats.spells)
            {
                AddSubAction(spell);
            }
        }

        if (name == "UtilityIcon")
        {
            CombatAction flee = Resources.Load("ScriptableObjects/Flee") as CombatAction;
            AddSubAction(flee);
        }
    }
}
