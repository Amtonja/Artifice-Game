using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters
{
    public class tempKomodoAI : MonoBehaviour
    {

        //Temporary komodo script

        private Enemy _enemy; //ref to our Player script

        // Use this for initialization
        void Start()
        {
            _enemy = GetComponent<Enemy>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_enemy.IsMyTurn)
            {
                Debug.Log(gameObject.name.ToString() + "'s turn!");

                float randA = Random.value; //which target
                float randB = Random.value; //which attack

                GameObject person;
                if (randA < 0.5f)
                {
                    person = GameObject.Find("Evans");
                }
                else {
                    person = GameObject.Find("Hurley");
                }
                CombatEntity target = person.GetComponent<CombatEntity>();

//                if (randB < 0.5f)
//                {
//                    //_player.MeleeAttack(target);
//                    _player.MyCombatAction = _player.MeleeAttack;                    
//                }
//                else
//                {
                    //_player.FireBreath(target);
                    _enemy.MySpell = _enemy.FireBreath;
                    _enemy.MyCombatAction = _enemy.BeginSpellCast;
//                }

                _enemy.MyCombatAction(target, _enemy.ActiveWeapon);
            }
        }
    }
}
