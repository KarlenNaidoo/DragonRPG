using UnityEngine;

namespace RPG.Weapons
{

    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private AnimationClip attackAnimation;
        public Transform gripTransform;

        [SerializeField] private float maxAttackRange = 2f;
        [SerializeField] private float minTimeBetweenHits = .5f;


        public float GetMinTimeBetweenHits()
        {
            // TODO consider whether we take animation time into account
            return minTimeBetweenHits;
        }

        public float GetMaxAttackRange()
        {
            // TODO consider whether we take animation time into account
            return maxAttackRange;
        }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        // So that asset packs cannot cause crashes
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }

        public GameObject GetWeaponPrefab() { return weaponPrefab; }
    }
}