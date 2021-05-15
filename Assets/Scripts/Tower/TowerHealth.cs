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
        public static float startingHealth = 1000f;
        public float health;
        public float damageMultiplier = 10f;
        public TowerLight towerLight;
        public GameObject rubleParticlePrefab;
        public CameraShake cameraShake;
        public Action OnTowerDestroy;

        private float maxDamagePerHit;
        private GameObject[] healthDots;
        private SpriteRenderer spriteRenderer;
        private int healthDotsIterator;

        #region unity callback

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            health = startingHealth;
            maxDamagePerHit = health / 5;
            healthDotsIterator = 0;
        }

        private void Start()
        {
            healthDots = towerLight.healthDots;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Ball")) return;

            // Know which body of tower got hit
            ContactPoint2D contact = other.contacts[0];
            //Debug.Log(contact.collider.name + " hit " + contact.otherCollider.name);

            ProcessEffect(contact);

            // Calculate the damage based on magnitude
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

            damage = Mathf.Min(damage, maxDamagePerHit);
            Hit(damage);
        }

        #endregion

        #region private function

        private void ProcessEffect(ContactPoint2D contact)
        {
            float minThresholdImpulse = 20f;
            float maxImpulse = 100f;

            if (contact.normalImpulse > minThresholdImpulse)
            {
                var ruble = Instantiate(rubleParticlePrefab, contact.point, Quaternion.identity);
                Quaternion rotation = Quaternion.LookRotation(ruble.transform.forward, contact.normal * -1);
                ruble.transform.rotation = rotation;
                ParticleSystem rubleParticle = ruble.GetComponent<ParticleSystem>();
                var mainModule = rubleParticle.main;
                var emissionModule = rubleParticle.emission;
                ParticleSystem.Burst burst = new ParticleSystem.Burst(0, contact.normalImpulse / maxImpulse * mainModule.maxParticles);
                emissionModule.SetBurst(0, burst);
                Destroy(ruble, 1f);

                cameraShake.Shake(contact.normalImpulse / maxImpulse * cameraShake.maxAmplitude);
            }
        }

        private void Hit(float damage)
        {
            health -= damage;
            health = Mathf.Max(health, 0);
            if (health == 0)
            {
                OnTowerDestroy?.Invoke();
            }
            SetHealthUI();
        }

        private void SetHealthUI()
        {
            int lastDotActiveIndex = healthDots.Length - Mathf.CeilToInt((health / startingHealth) * healthDots.Length);
            while (healthDotsIterator < lastDotActiveIndex)
            {
                healthDots[healthDotsIterator].SetActive(false);
                healthDotsIterator++;
            }
        }

        #endregion
    }
}
