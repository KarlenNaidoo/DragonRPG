using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(CameraRaycaster))]
public class Player : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float currentHealthPoints;
    [SerializeField] int enemyLayer = 9;
    [SerializeField] float damagePerHit = 10;
    [SerializeField] float minTimeBetweenHits = .5f;
    [SerializeField] float maxAttackRange = 2f;
    [SerializeField] Weapon weaponInUse;

    
    GameObject currentTarget;
    CameraRaycaster cameraRaycaster;
    float lastHitTime = 0f;

    void Start()
    {
        RegisterForMouseClick();
        currentHealthPoints = maxHealthPoints;
        PutWeaponInHand();

    }

    private void PutWeaponInHand()
    {
        var weaponPrefab = weaponInUse.GetWeaponPrefab();
        GameObject dominantHand = RequestDominantHand();
        var weapon = Instantiate(weaponPrefab, dominantHand.transform);
        weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
        weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
    }

    private GameObject RequestDominantHand()
    {
        var dominantHands = GetComponentsInChildren<DominantHand>();
        int numberOfDominantHands = dominantHands.Length;
        Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on player, please add one");
        Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
        return dominantHands[0].gameObject;

    }

    private void RegisterForMouseClick()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
    }

    // Refactor to simplify, reduce number of lines
    void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;

            // Check distance from player to enemy
            // Could also use (enemy.transform.position - transform.position).magnitude
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
            if (distanceToEnemy > maxAttackRange)
            {
                return;
            }

            currentTarget = enemy;
            var enemyComponent = enemy.GetComponent<Enemy>();

            if (Time.time - lastHitTime > minTimeBetweenHits)
            {
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }


        }

    }
    public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints;} }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        
    }

}
