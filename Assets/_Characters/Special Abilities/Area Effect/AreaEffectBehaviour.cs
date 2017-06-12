using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{

    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility
    {
        AreaEffectConfig config;

        public void SetConfig(AreaEffectConfig config)
        {
            this.config = config;
        }

        public void Use(AbilityUseParams useParams)
        {
            //print("Player position: " + transform.position);
            Vector3 origin = transform.position;
            float radius = config.GetRadius();
            // Static sphere cast for target
            RaycastHit[] hits = Physics.SphereCastAll(origin, radius, Vector3.up, radius);
            foreach (RaycastHit hit in hits)
            {
                var damagable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damagable != null)
                {
                    float damageToDeal = useParams.baseDamage + config.GetDamageToEachtarget();
                    damagable.TakeDamage(damageToDeal);
                }
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}