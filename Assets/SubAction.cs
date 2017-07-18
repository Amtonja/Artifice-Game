using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Text;
using System.Xml;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SubAction : MonoBehaviour, ICancelHandler, ISubmitHandler, ISelectHandler
{
    private ActionIcon _actionIcon;
    private CombatPlayerUI parentUI;
    private TextMeshProUGUI textmesh;
    private string methodName;
    private string description;
    private GameObject descriptionDisplay;

    public string actionName;

    // Use this for initialization
    void Start()
    {
        _actionIcon = GetComponentInParent<ActionIcon>();
        parentUI = GetComponentInParent<CombatPlayerUI>();
        TextAsset actionsDB = Resources.Load("Files/ArtificeCombatActions") as TextAsset;
        LoadActionInfo(actionsDB, actionName);

        descriptionDisplay = PlayManager.instance.groupCombatUI.transform.Find("Canvas/Panel/ActionInfo/ActionDescription").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadActionInfo(TextAsset file, string name)
    {
        if (file == null)
        {
            Debug.LogError("Combat actions XML file not found");
            return;
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(file.text);
        
        XmlNode root = xmlDoc.DocumentElement;        
        XmlNode action = root.SelectSingleNode(string.Format("descendant::action[name = '{0}']", name));

        if (action == null)
        {
            Debug.LogError("Couldn't find any information on file for the action " + name);
            return;
        }

        methodName = action["methodname"].InnerText;
        description = action["description"].InnerText;
    }

    public void OnCancel(BaseEventData eventData)
    {
        _actionIcon.CloseSubmenu();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (methodName != null)
        {
            parentUI.Invoke(methodName, 0);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (description != null)
        {
            descriptionDisplay.GetComponent<Text>().text = description;
        }
    }
}
