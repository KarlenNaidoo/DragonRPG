using System;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abilities/Area Effect"))]
    public class AreaEffectConfig : SpecialAbility
    {
        [Header("Area Effect Specific")]
        [SerializeField] float radius = 5f;
        [SerializeField] float damageToEachTarget = 15f;

        public float GetRadius()
        {
            return this.radius;
        }

        public float GetDamageToEachtarget()
        {
            return damageToEachTarget;
        }

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<AreaEffectBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent; // Behaviour is inherited from Special Ability local variable
        }
    }
}