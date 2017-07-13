using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ActionIcon : MonoBehaviour
{
    private List<GameObject> subActions = new List<GameObject>();
    private GameObject subActionPanel;
    private GameObject subActionPrefab;
    private CombatPlayerUI parentUI;

    // Use this for initialization
    void Start()
    {
        subActionPanel = transform.Find("OptionPanel").gameObject;
        if (subActionPanel == null)
        {
            Debug.LogError("Action icon " + name + " lacks a child panel for sub-actions");
        }
        else
        {
            CloseSubmenu();
        }

        subActionPrefab = Resources.Load("Prefabs/SubActionPrefab") as GameObject;

        parentUI = GetComponentInParent<CombatPlayerUI>();

        //subActions = new List<Button>();

        // TESTING
        AddSubAction("Cure", "OnCure");
        AddSubAction("Gust", "OnGust");
        AddSubAction("Bolt", "OnBolt");
        AddSubAction("Scorch", "OnScorch");
        //ArrangeSubActions();
        // TESTING
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   

    public void OpenSubmenu()
    {
        ArrangeSubActions();
        if (subActionPanel != null) subActionPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(subActions[0].gameObject);
    }

    public void CloseSubmenu()
    {
        if (subActionPanel != null) subActionPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void AddSubAction(string label, string methodName)
    {
        GameObject newAction = Instantiate(subActionPrefab, subActionPanel.transform, false);
        newAction.name = label;
        newAction.GetComponent<TextMeshProUGUI>().text = label;
        newAction.GetComponent<SubAction>().methodName = methodName;
        subActions.Add(newAction); 
    }

    public void ArrangeSubActions()
    {
        float angle = 2f * Mathf.PI / subActions.Count;
        float radius = 100f;        

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
}
