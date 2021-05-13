using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SparkSpawner))]
    public class PlayerController : MonoBehaviour
    {
        public float linearForce = 500f;
        public float maxLinearSpeed = 100f;
        public float angularForce = 500f;
        public float burstSpeed = 2500f;
        public float skillDelay = 1f;
        public float maxAngularSpeed = 500f;
        public float blockDuration = 1f;
        public Vector2 centerOfMass;

        private Rigidbody2D _rb;
        private Vector2 _movementInput = Vector2.zero;
        private float _rotationsInput;
        private float doubleTapDelay = 0.3f;
        private float lastTimestamp;
        private float lastSkillTimeStamp = 0f;
        private float lastTap;
        private bool isDoubleTap;
        private SparkSpawner sparkSpawner;
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
            _rb.AddForce(_movementInput * (linearForce * Time.fixedDeltaTime * 2), ForceMode2D.Impulse);
            var velocity = _rb.velocity;
            _rb.velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude, 0, maxLinearSpeed);

            if (isBlock)
            {
                var blockTime = Time.fixedTime - lastSkillTimeStamp;
                if (blockTime <= blockDuration)
                {
                    _rb.angularVelocity = 0;
                } else
                {
                    isBlock = false;
                }
            }
            else if (isDoubleTap)
            {
                _rb.angularVelocity = Mathf.Clamp(_rotationsInput * burstSpeed, -burstSpeed, burstSpeed);
                StartCoroutine(sparkSpawner.ShowSparks());
                isDoubleTap = false;

            } else
            {
                if (-maxAngularSpeed <= _rb.angularVelocity && _rb.angularVelocity <= maxAngularSpeed)
                {
                    _rb.AddTorque(_rotationsInput * angularForce * Time.fixedDeltaTime * 2, ForceMode2D.Impulse);
                    _rb.angularVelocity = Mathf.Clamp(_rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);
                }
            }
        }

        #endregion

        #region public callback

        public void OnMove(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            if (!gameObject.scene.IsValid())
            {
                return;
            }

            _rotationsInput = context.ReadValue<float>();

            var tapDelayTime = Time.fixedTime - lastTimestamp;
            var burstDelayTime = Time.fixedTime - lastSkillTimeStamp;
            if (context.started)
            {
                if (tapDelayTime < doubleTapDelay && burstDelayTime > skillDelay && lastTap == _rotationsInput)
                {
                    isDoubleTap = true;
                    lastSkillTimeStamp = Time.fixedTime;
                }

                lastTap = _rotationsInput;
                lastTimestamp = Time.fixedTime;
            }

            
        }

        public void OnFlip(InputAction.CallbackContext context)
        {
            Vector3 newScale = transform.localScale;
            if (newScale == null || !gameObject.scene.IsValid()) return;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        public void OnCover(InputAction.CallbackContext context)
        {
            if (!gameObject.scene.IsValid())
            {
                return;
            }

            if (context.started)
            {
                var burstDelayTime = Time.fixedTime - lastSkillTimeStamp;
                if (burstDelayTime > skillDelay)
                {
                    isBlock = true;
                    lastSkillTimeStamp = Time.fixedTime;
                }
            }

            if (context.canceled)
            {
                isBlock = false;
            }



        }
        #endregion
    }
}
