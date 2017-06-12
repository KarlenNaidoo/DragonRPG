using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abilities/Power Attack"))]
    public class PowerAttackConfig : SpecialAbilityConfig
    {
        [Header("Special Ability Specific")]
        [SerializeField] float extraDamage = 10f;

        public float GetExtraDamage()
        {
            return this.extraDamage;
        }

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }
    }
}