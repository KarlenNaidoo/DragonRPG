using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility  {

        PowerAttackConfig config; // This component will be added to the gameobject
   

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }
        public void Use()
        {
            print("Power attack used");
        }

        // Use this for initialization
        void Start() {
            print("Power attack behaviour attached to " + gameObject.name);

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
