using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SparkSpawner))]
    [RequireComponent(typeof(RacketLight))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movements Settings")]
        public float linearForce = 500f;
        public float maxLinearSpeed = 100f;
        public float angularForce = 500f;
        public float maxAngularSpeed = 500f;
        public Vector2 centerOfMass;

        [Header("Skill Settings")]
        public float flickSpeed = 2500f;
        public float flickCooldown = 1f;
        public float blockDuration = 1f;
        public float blockCooldown = 1f;

        [Header("Input Settings")]
        public float doubleTapDelayThreshold = 0.3f;

        private SparkSpawner sparkSpawner;
        private Rigidbody2D _rb;
        private Vector2 _movementInput = Vector2.zero;
        private float _rotationInput;
        private float blockTimestamp = 0f;
        private float firstTapTimestamp = 0f;
        private float lastFlickTimestamp = 0f;
        private float lastRotationInput = 0f;
        private bool isDoubleTap;
        private bool isBlock = false;

        #region unity callback

        private void Awake()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
            _rb.centerOfMass = centerOfMass;
            sparkSpawner = GetComponent<SparkSpawner>();
        }

        void Start()
        {
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            //spriteRenderer.color = Random.ColorHSV();
        }

        private void FixedUpdate()
        {
            Move();

            if (isBlock)
            {
                Block();
            }
            else if (isDoubleTap)
            {
                Flick();
            } else
            {
                Rotate();
            }
        }

        private void Move()
        {
            _rb.AddForce(_movementInput * (linearForce * Time.fixedDeltaTime * 2), ForceMode2D.Impulse);
            var velocity = _rb.velocity;
            _rb.velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude, 0, maxLinearSpeed);
        }

        private void Rotate()
        {
            if (-maxAngularSpeed <= _rb.angularVelocity && _rb.angularVelocity <= maxAngularSpeed)
            {
                _rb.AddTorque(_rotationInput * angularForce * Time.fixedDeltaTime * 2, ForceMode2D.Impulse);
                _rb.angularVelocity = Mathf.Clamp(_rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);
            }
        }

        private void Block()
        {
            float elapsedBlockTime = Time.fixedTime - blockTimestamp;
            if (elapsedBlockTime <= blockDuration)
            {
                _rb.angularVelocity = 0;
            }
            else
            {
                isBlock = false;
            }
        }

        private void Flick()
        {
            _rb.angularVelocity = Mathf.Clamp(_rotationInput * flickSpeed, -flickSpeed, flickSpeed);
            StartCoroutine(sparkSpawner.ShowSparks());
            isDoubleTap = false;
        }

        #endregion

        #region public callback

        public void OnMove(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            if (!gameObject.scene.IsValid()) return;

            _rotationInput = context.ReadValue<float>();

            if (context.started)
            {
                if (Time.fixedTime - firstTapTimestamp < doubleTapDelayThreshold 
                    && lastRotationInput == _rotationInput
                    && Time.fixedTime - lastFlickTimestamp > flickCooldown)
                {
                    isDoubleTap = true;
                    lastFlickTimestamp = Time.fixedTime;
                }
                else
                {
                    firstTapTimestamp = Time.fixedTime;
                    lastRotationInput = _rotationInput;
                }
            }
        }

        public void OnFlip(InputAction.CallbackContext context)
        {
            if (!gameObject.scene.IsValid()) return;

            Vector3 newScale = transform.localScale;
            if (newScale == null || !gameObject.scene.IsValid()) return;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        public void OnCover(InputAction.CallbackContext context)
        {
            if (!gameObject.scene.IsValid()) return;

            if (context.started)
            {
                if (Time.fixedTime - blockTimestamp > blockCooldown)
                {
                    isBlock = true;
                    blockTimestamp = Time.fixedTime;
                }
            }
            else if (context.canceled)
            {
                if (isBlock)
                {
                    isBlock = false;
                    blockTimestamp = Time.fixedTime;
                }
            }
        }

        #endregion
    }
}
