using UnityEngine;

using RPG.Core;

namespace RPG.Weapons
{

    public class Projectile : MonoBehaviour
    {
        private const float DESTROY_DELAY = 0.01f;
        private float damageCaused;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private GameObject shooter; // So can inspect when paused

        public float GetDefaultLaunchSpeed { get { return projectileSpeed; } }

        // TODO investigate OOP with using getters and setters

        // Sets the damage caused using a public get method
        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        private void DealDamageOnlyIfDamagable(Collision collision)
        {
            var damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            if (damageableComponent)
            {
                (damageableComponent as IDamageable).TakeDamage(damageCaused);
            }

            Destroy(gameObject, DESTROY_DELAY);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var layerCollidedWith = collision.gameObject.layer;
            if (shooter && (shooter.layer != layerCollidedWith))
            {
                // damageableComponent is nullable. Finding a gameobject with a script that implements the Idamageable interface
                DealDamageOnlyIfDamagable(collision);
            }
        }
    }
}