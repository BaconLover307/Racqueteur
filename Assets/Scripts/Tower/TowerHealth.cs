using System;
using UnityEngine;

namespace Tower
{
    public class TowerHealth : MonoBehaviour
    {
        public float health = 1000f;
        private float maxSliceHealth;

        public float damageMultiplier = 10f;
        #region unity callback

        private void Start()
        {
            // Define how max of slice health point of tower
            maxSliceHealth = health / 5;            
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Rigidbody2D>() == null) return;

            if (!other.gameObject.CompareTag("Ball")) return;

            // Know which body of tower got hit
            ContactPoint2D contact = other.contacts[0];
            Debug.Log(contact.collider.name + " hit " + contact.otherCollider.name);

            //Calculate the damage based on magnitude
            float damage;

            if (contact.otherCollider.name == "WeakpointTop" ||
                contact.otherCollider.name == "WeakpointBottom" ||
                contact.otherCollider.name == "WeakpointLeft" ||
                contact.otherCollider.name == "WeakpointRight")
            {
                damage = other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude *
                    damageMultiplier;
            }

            else
            {
                damage = other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
            }

            // Make damage to be 1/5 of full health if larger than > 1/5 full health
            // and round it
            damage = Mathf.Round(Mathf.Min(damage, maxSliceHealth));

            Hit(damage);
        }

        #endregion

        #region private function

        private void Hit(float damage)
        {
            health -= damage;
            Debug.Log(health);
        }

        #endregion
    }
}
