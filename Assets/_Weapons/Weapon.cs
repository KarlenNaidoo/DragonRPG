using UnityEngine;

namespace RPG.Weapons
{

    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private AnimationClip attackAnimation;
        public Transform gripTransform;


        public GameObject GetWeaponPrefab() { return weaponPrefab; }
    }
}