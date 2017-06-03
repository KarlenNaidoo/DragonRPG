using UnityEngine;

[CreateAssetMenu(menuName = ("RPG/Weapon"))]
public class Weapon : ScriptableObject
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private AnimationClip attackAnimation;
    [SerializeField] Transform weaponGrip;


    public GameObject GetWeaponPrefab() { return weaponPrefab; }
}