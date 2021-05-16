using System;
using UnityEngine;

namespace Tower
{
    [RequireComponent(typeof(TowerDestroy))]
    [RequireComponent(typeof(DamageIndicator))]
    [RequireComponent(typeof(Animator))]
    public class TowerHealth : MonoBehaviour
    {
        public float startingHealth;
        public float damageMultiplier = 10f;
        public float blinkThreshold = 0.2f;
        public TowerLight towerLight;
        public GameObject rubleParticlePrefab;
        public CameraShake cameraShake;
        public Action OnTowerDestroy;

        [HideInInspector]
        public float health;

        private float maxDamagePerHit;
        private GameObject[] healthDots;
        private int healthDotsIterator;
        private TowerDestroy towerDestroy;
        private DamageIndicator damageIndicator;
        private Animator animator;

        #region unity callback

        private void Awake()
        {
            towerDestroy = GetComponent<TowerDestroy>();
            damageIndicator = GetComponent<DamageIndicator>();
            animator = GetComponent<Animator>();
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
            damageIndicator.Spawn(Mathf.RoundToInt(damage), contact.point, contact.normal.normalized * -1);
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

            if (health <= blinkThreshold * startingHealth && animator.enabled == false)
            {
                animator.enabled = true;
            }

            if (health <= 0)
            {
                OnTowerDestroy?.Invoke();
                towerDestroy.Shatter();
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