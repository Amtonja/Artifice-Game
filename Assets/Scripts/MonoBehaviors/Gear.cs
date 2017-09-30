using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Characters;

public class Gear : MonoBehaviour
{
    public Weapon primaryWeapon, secondaryWeapon;
    public Armor armor;

    // Use this for initialization
    void Awake()
    {
        // Create an instance of each weapon
        // This should only be done on starting a new game.
        Weapon primaryInstance = Object.Instantiate<Weapon>(primaryWeapon);
        Weapon secondaryInstance = Object.Instantiate<Weapon>(secondaryWeapon);

        primaryInstance.guid = System.Guid.NewGuid();
        Debug.Log(name + "'s primary weapon assigned guid " + primaryInstance.guid.ToString());


        secondaryInstance.guid = System.Guid.NewGuid();

        primaryWeapon = primaryInstance;
        secondaryWeapon = secondaryInstance;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
