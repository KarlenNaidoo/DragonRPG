// TODO consider re-wiring
using System;
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    [RequireComponent(typeof(CameraRaycaster))]
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private AnimatorOverrideController animatorOverrideController;
        private CameraRaycaster cameraRaycaster;
        [SerializeField] private float currentHealthPoints;
        [SerializeField] private float baseDamage = 10;
        private Animator animator;
        private float lastHitTime = 0f;

        // Temporarily serializing for debugging
        [SerializeField] SpecialAbility[] abilities;

        [SerializeField] private float maxHealthPoints = 100f;
        [SerializeField] private Weapon weaponInUse;
        public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

        private void AttackTarget(Enemy enemy)
        {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack"); // TODO make const
                enemy.TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            // Check distance from player to target
            // Could also use (enemy.transform.position - transform.position).magnitude
            float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }


        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // TODO remove constant
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemey;
        }

        private void OnMouseOverEnemey(Enemy enemy)
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                AttackTarget(enemy);
            }else if (Input.GetMouseButtonDown(1))
            {
                AttemptSpecialAbility(0, enemy);
            }
        }

        private void AttemptSpecialAbility(int abilityIndex, Enemy enemy)
        {
            var energyComponent = GetComponent<Energy>();
            float energyCost = abilities[abilityIndex].GetEnergyCost();
            print("Energy Cost: " + energyCost);
            if (energyComponent.IsEnergyAvailable(energyCost)) // TODO read from script obj
            {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage, this.transform);
                abilities[abilityIndex].Use(abilityParams);
                // Use the ability
            }
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            abilities[0].AttachComponentTo(gameObject);
        }

    }
}