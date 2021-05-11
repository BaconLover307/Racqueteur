using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public float linearForce = 500f;
        public float maxLinearSpeed = 100f;
        public float angularForce = 1000f;
        public float angularSpeedMultiplier = 1000f;
        public Vector2 centerOfMass;

        private Rigidbody2D _rb;
        private Vector2 _movementInput = Vector2.zero;
        private float _rotationsInput;
        private float spamRate = 0f;
        private float lastTimestamp;

        #region unity callback
        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
            _rb.centerOfMass = centerOfMass;
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            //spriteRenderer.color = Random.ColorHSV();
        }

        private void FixedUpdate()
        {
            _rb.AddForce(_movementInput * (linearForce * Time.fixedDeltaTime), ForceMode2D.Impulse);
            var velocity = _rb.velocity;
            _rb.velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude, 0, maxLinearSpeed);
            _rb.AddTorque(_rotationsInput * angularForce * spamRate * Time.fixedDeltaTime);

            if (_rotationsInput == 0)
            {
                spamRate -= 0.1f;
            }
            else
            {
                _rb.angularVelocity = Mathf.Clamp(_rb.angularVelocity, -angularSpeedMultiplier * spamRate, angularSpeedMultiplier * spamRate);
            }

            spamRate = Mathf.Max(0, spamRate);
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
            if (context.started)
            {
                spamRate = 1 / (Time.fixedTime - lastTimestamp);
                lastTimestamp = Time.fixedTime;
            }
        }

        #endregion
    }
}
