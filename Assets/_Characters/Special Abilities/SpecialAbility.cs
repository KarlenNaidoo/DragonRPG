using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;

        // Set up constructor
        public AbilityUseParams(IDamageable target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }
    public abstract class SpecialAbility : ScriptableObject
    {
        [Header("Special Ability General")]

        [SerializeField] float energyCost = 10f;

        public float GetEnergyCost()
        {
            return this.energyCost;
        }

        protected ISpecialAbility behaviour;

        public void Use(AbilityUseParams useParams)
        {
            behaviour.Use(useParams);
        }

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);
    }

    public interface ISpecialAbility
    {
        void Use(AbilityUseParams useParams);
    }
}
