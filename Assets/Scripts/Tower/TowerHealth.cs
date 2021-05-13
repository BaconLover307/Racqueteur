using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Tower
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TowerHealth : MonoBehaviour
    {
        public List<GameObject> Children;
        
        public static float startingHealth = 1000f;
        public float health;
        public float damageMultiplier = 10f;
        public GameObject rubleParticlePrefab;

        private float maxSliceHealth;
        private SpriteRenderer spriteRenderer;
        #region unity callback

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            foreach (Transform child in transform.GetChild(transform.childCount - 1))
            {
                if (child.tag == "Dot")
                {
                    Children.Add(child.gameObject);
                }
            }
            health = startingHealth;
        }

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

            Debug.Log(contact.normalImpulse);
            if (contact.normalImpulse > 2000f)
            {
                Quaternion rotation = Quaternion.LookRotation(contact.normal);
                var ruble = Instantiate(rubleParticlePrefab, contact.point, rotation);
                ParticleSystem rubleParticle = ruble.GetComponent<ParticleSystem>();
                var mainModule = rubleParticle.main;
                mainModule.startColor = spriteRenderer.color;
                var emissionModule = rubleParticle.emission;
                emissionModule.rateOverTime = contact.normalImpulse / 8000 * mainModule.maxParticles;
                Destroy(ruble, 1f);
            }

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
            SetHealthUI();
        }

        private void SetHealthUI()
        {
            // Get the number of active dot
            int numOfDot = (int) Math.Ceiling(health / (startingHealth / 36));
            if (numOfDot < -1)
            {
                numOfDot = -1;
            }
            for (int i = 35; i > numOfDot; i--)
            {
                Children[i].SetActive(false);
            }
        }

        #endregion
    }
}
