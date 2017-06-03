using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RPG.Core;

namespace RPG.Characters
{

    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] float currentHealthPoints;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float chaseRadius = 5f;

        [SerializeField] float attackRadius = 4f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float secondsBetweenShots = 0.5f;

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
        AICharacterControl aiCharacterControl = null;
        GameObject player = null;

        bool isAttacking = false;


        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                InvokeRepeating("FireProjectile", 0f, secondsBetweenShots); // TODO Switch to coroutines
                FireProjectile();
            }

            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }

            if (distanceToPlayer <= chaseRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }

        }

        // TODO Separate out character firing logic
        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(this.gameObject);

            //Calculate direction between player root and projectile spawn point
            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * newProjectile.GetComponent<Projectile>().GetDefaultLaunchSpeed;

        }
        public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0)
            {
                Destroy(gameObject);
            }
        }

        void OnDrawGizmos()
        {
            // Draw chase sphere
            Gizmos.color = new Color(0f, 0f, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}