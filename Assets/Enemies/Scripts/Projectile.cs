using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float projectileSpeed;
    float damageCaused;

    // Sets the damage caused using a public get method
    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }

    void OnTriggerEnter(Collider collider)
    {
       // damageableComponent is nullable. Finding a gameobject with a script that implements the Idamageable interface
        var damageableComponent = collider.gameObject.GetComponent(typeof(IDamageable));
        print("Hit object " + collider.gameObject);
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
    }

   public float getProjectileSpeed { get { return projectileSpeed; } }
}
