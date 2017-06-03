using UnityEngine;
using UnityEngine.Assertions;

// TODO consider re-wiring
using RPG.CameraUI; 
using RPG.Core;
namespace RPG.Characters
{

    [RequireComponent(typeof(CameraRaycaster))]
    public class Player : MonoBehaviour, IDamageable
    {
        private CameraRaycaster cameraRaycaster;
        [SerializeField] private float currentHealthPoints;
        [SerializeField] private float damagePerHit = 10;
        [SerializeField] private int enemyLayer = 9;
        private float lastHitTime = 0f;
        [SerializeField] private float maxAttackRange = 2f;
        [SerializeField] private float maxHealthPoints = 100f;
        [SerializeField] private float minTimeBetweenHits = .5f;
        [SerializeField] private Weapon weaponInUse;
        public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

        // Refactor to simplify, reduce number of lines
        private void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemyLayer)
            {
                var enemy = raycastHit.collider.gameObject;

                // Check distance from player to enemy
                // Could also use (enemy.transform.position - transform.position).magnitude
                float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
                if (distanceToEnemy > maxAttackRange) { return; }

                var enemyComponent = enemy.GetComponent<Enemy>();

                if (Time.time - lastHitTime > minTimeBetweenHits)
                {
                    enemyComponent.TakeDamage(damagePerHit);
                    lastHitTime = Time.time;
                }
            }
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
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }

        private void Start()
        {
            RegisterForMouseClick();
            currentHealthPoints = maxHealthPoints;
            PutWeaponInHand();
        }
    }
}