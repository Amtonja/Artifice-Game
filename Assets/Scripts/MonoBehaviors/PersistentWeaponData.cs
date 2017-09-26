using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class PersistentWeaponData : MonoBehaviour //<--Copy this file. Rename the file and class name.
{ 

    public void OnRecordPersistentData()
    {
        // Add your code here to record data into the Lua environment.
        // Typically, you'll record the data using a line similar to:
        //    DialogueLua.SetVariable("someVarName", someData);
        // or:
        //    DialogueLua.SetActorField("myName", "myFieldName", myData);
        //
        // Note that you can use this static method to get the actor 
        // name associated with this GameObject:
        //    var actorName = OverrideActorName.GetActorName(transform);
        Gear equipment = GetComponent<Gear>();
        var actorName = OverrideActorName.GetActorName(transform);
        string primaryGUID = equipment.primaryWeapon.guid.ToString();
        Debug.Log("Saving weapon guid " + equipment.primaryWeapon.guid.ToString());

        DialogueLua.SetActorField(actorName, "PrimaryWeaponID", primaryGUID);
        DialogueLua.SetItemField(primaryGUID, "name", equipment.primaryWeapon.itemName);
        DialogueLua.SetItemField(primaryGUID, "XP", equipment.primaryWeapon.XP);
        DialogueLua.SetItemField(primaryGUID, "attackValue", equipment.primaryWeapon.attackValue);
        DialogueLua.SetItemField(primaryGUID, "magicValue", equipment.primaryWeapon.magicValue);
        DialogueLua.SetItemField(primaryGUID, "unlockedAbilites", equipment.primaryWeapon.unlockedAbilities);

        string secondaryGUID = equipment.secondaryWeapon.guid.ToString();
        DialogueLua.SetActorField(actorName, "SecondaryWeaponID", secondaryGUID);
        DialogueLua.SetItemField(secondaryGUID, "name", equipment.secondaryWeapon.itemName);
        DialogueLua.SetItemField(secondaryGUID, "XP", equipment.secondaryWeapon.XP);
        DialogueLua.SetItemField(secondaryGUID, "attackValue", equipment.secondaryWeapon.attackValue);
        DialogueLua.SetItemField(secondaryGUID, "magicValue", equipment.secondaryWeapon.magicValue);
        DialogueLua.SetItemField(secondaryGUID, "unlockedAbilites", equipment.secondaryWeapon.unlockedAbilities);
    }

    public void OnApplyPersistentData()
    {
        // Add your code here to get data from Lua and apply it (usually to the game object).
        // Typically, you'll use a line similar to:
        // myData = DialogueLua.GetActorField(name, "myFieldName").AsSomeType;
        //
        // When changing scenes, OnApplyPersistentData() is typically called at the same 
        // time as Start() methods. If your code depends on another script having finished 
        // its Start() method, use a coroutine to wait one frame. For example, in 
        // OnApplyPersistentData() call StartCoroutine(DelayedApply());
        // Then define DelayedApply() as:
        // IEnumerator DelayedApply() {
        //     yield return null; // Wait 1 frame for other scripts to initialize.
        //     <your code here>
        // }

        Gear equipment = GetComponent<Gear>();
        var actorName = OverrideActorName.GetActorName(transform);

        string weaponID = DialogueLua.GetActorField(actorName, "PrimaryWeaponID").AsString;
        string weaponName = DialogueLua.GetItemField(weaponID, "name").AsString;
        string path = "ScriptableObjects/Weapons/" + weaponName + ".asset";
        equipment.primaryWeapon = Object.Instantiate(Resources.Load(path)) as Artifice.Characters.Weapon;
        equipment.primaryWeapon.guid = new System.Guid(weaponID);
        equipment.primaryWeapon.XP = DialogueLua.GetItemField(weaponID, "XP").AsInt;
        equipment.primaryWeapon.attackValue = DialogueLua.GetItemField(weaponID, "attackValue").AsInt;
        equipment.primaryWeapon.magicValue = DialogueLua.GetItemField(weaponID, "magicValue").AsInt;
        //equipment.primaryWeapon.unlockedAbilities = DialogueLua.GetItemField(weaponID, "unlockedAbilites").AsTable;
    }

    public void OnEnable()
    {
        // This optional code registers this GameObject with the PersistentDataManager.
        // One of the options on the PersistentDataManager is to only send notifications
        // to registered GameObjects. The default, however, is to send to all GameObjects.
        // If you set PersistentDataManager to only send notifications to registered
        // GameObjects, you need to register this component using the line below or it
        // won't receive notifications to save and load.
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public void OnDisable()
    {
        // Unsubscribe the GameObject from PersistentDataManager notifications:
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    //--- Uncomment this method if you want to implement it:
    //public void OnLevelWillBeUnloaded() 
    //{
    // This will be called before loading a new level. You may want to add code here
    // to change the behavior of your persistent data script. For example, the
    // IncrementOnDestroy script disables itself because it should only increment
    // the variable when it's destroyed during play, not because it's being
    // destroyed while unloading the old level.
    //}

}

