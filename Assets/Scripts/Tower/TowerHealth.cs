using System;
using UnityEngine;

namespace Tower
{
    [Serializable]
    public struct DamageMultiplier
    {
        public float topMultiplier;
        public float leftMultiplier;
        public float rightMultiplier;
        public float bottomMultiplier;
    }

    [RequireComponent(typeof(TowerDestroy))]
    [RequireComponent(typeof(DamageIndicator))]
    [RequireComponent(typeof(Animator))]
    public class TowerHealth : MonoBehaviour
    {
        public float maxHealth = 1000f;
        public float maxDamagePercentage = 20;
        public float blinkThreshold = 0.2f;
        public TowerLight towerLight;
        public GameObject rubleParticlePrefab;
        public CameraShake cameraShake;
        public DamageMultiplier damageMultipliers;
        public Action OnTowerDestroy;

        [Header("SFX Clips")]
        public AudioClip towerHitSFX;
        public AudioClip towerHitWeakpointSFX;
        public AudioClip towerDestroySFX;

        [HideInInspector] public float health;

        private Animator _animator;
        private DamageIndicator _damageIndicator;
        private GameObject[] _healthDots;
        private int _healthDotsIterator;

        private float _maxDamagePerHit;
        private TowerDestroy _towerDestroy;
        private string collisionName = "";

        private AudioManager _audioManager;

        #region unity callback

        private void Awake()
        {
            _towerDestroy = GetComponent<TowerDestroy>();
            _damageIndicator = GetComponent<DamageIndicator>();
            _animator = GetComponent<Animator>();
            health = maxHealth;
            _maxDamagePerHit = maxHealth * maxDamagePercentage / 100;
            _audioManager = AudioManager.instance;
        }

        private void Start()
        {
            _healthDots = towerLight.healthDots;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Ball")) return;
            if (collisionName != "") return;
            collisionName = other.otherCollider.name;
            
            // Know which body of tower got hit
            var contact = other.GetContact(0);

            var damage = CalculateDamage(contact, other);
            Hit(damage);
            _damageIndicator.Spawn(Mathf.RoundToInt(damage), contact.point, contact.normal.normalized * -1, IsWeakPoint(contact));
            ProcessEffect(contact);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Ball")) return;
            if (collisionName == other.otherCollider.name) collisionName = "";
        }

        #endregion

        #region private function

        private bool IsWeakPoint(ContactPoint2D contact)
        {
            return (contact.otherCollider.name == "WeakpointTop") ||
                (contact.otherCollider.name == "WeakpointBottom") ||
                (contact.otherCollider.name == "WeakpointLeft") ||
                (contact.otherCollider.name == "WeakpointRight");
        }

        private void ProcessEffect(ContactPoint2D contact)
        {
            var minThresholdImpulse = 20f;
            var maxImpulse = 100f;

            if (contact.normalImpulse > minThresholdImpulse || health <= 0f)
            {
                var ruble = Instantiate(rubleParticlePrefab, contact.point, Quaternion.identity);
                var rotation = Quaternion.LookRotation(ruble.transform.forward, contact.normal * -1);
                ruble.transform.rotation = rotation;
                var rubleParticle = ruble.GetComponent<ParticleSystem>();
                var mainModule = rubleParticle.main;
                var emissionModule = rubleParticle.emission;
                var burst = new ParticleSystem.Burst(0, contact.normalImpulse / maxImpulse * mainModule.maxParticles);
                emissionModule.SetBurst(0, burst);
                Destroy(ruble, 1f);

                string hitSFX = IsWeakPoint(contact) ? "TowerHitWeakpoint" : "TowerHit";
                AudioManager.instance.Play(hitSFX);

                cameraShake.Shake(contact.normalImpulse / maxImpulse * cameraShake.maxAmplitude);
            }
        }

        private float CalculateDamage(ContactPoint2D weakPoint, Collision2D ball)
        {
            // Calculate the damage based on magnitude
            Debug.Log(weakPoint.normalImpulse);
            var damage = weakPoint.normalImpulse;

            switch (weakPoint.otherCollider.name)
            {
                case "WeakpointTop":
                    damage *= damageMultipliers.topMultiplier;
                    break;
                case "WeakpointBottom":
                    damage *= damageMultipliers.bottomMultiplier;
                    break;
                case "WeakpointLeft":
                    damage *= damageMultipliers.leftMultiplier;
                    break;
                case "WeakpointRight":
                    damage *= damageMultipliers.rightMultiplier;
                    break;
            }

            return Mathf.Min(damage, _maxDamagePerHit);
        }

        private void Hit(float damage)
        {
            health -= damage;
            health = Mathf.Max(health, 0);

            if (health <= blinkThreshold * maxHealth && _animator.enabled == false) _animator.enabled = true;

            if (health <= 0)
            {
                OnTowerDestroy?.Invoke();
                _towerDestroy.Shatter();

                AudioManager.instance.Play("TowerDestroy");
            }

            SetHealthUI();
        }

        private void SetHealthUI()
        {
            var lastDotActiveIndex = _healthDots.Length - Mathf.CeilToInt(health / maxHealth * _healthDots.Length);
            while (_healthDotsIterator < lastDotActiveIndex)
            {
                _healthDots[_healthDotsIterator].SetActive(false);
                _healthDotsIterator++;
            }
        }

        #endregion
    }
}