using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
    public Text fullName, attack, defense, magic, magicDefense, speed;

    void Init()
    {
        StatMenu menu = GetComponentInParent<StatMenu>();
        fullName.text = menu.SelectedCharacter.Stats.characterName;
        attack.text = menu.SelectedCharacter.Stats.attack.ToString();
        defense.text = menu.SelectedCharacter.Stats.defense.ToString();
        magic.text = menu.SelectedCharacter.Stats.magic.ToString();
        magicDefense.text = menu.SelectedCharacter.Stats.magicDefense.ToString();
        speed.text = menu.SelectedCharacter.Stats.speed.ToString();
    }

    void SelectedCharacterChange()
    {
        Init();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
