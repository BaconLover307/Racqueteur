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
        public float maxAngularSpeed = 1000f;
        public Vector2 centerOfMass;

        private Rigidbody2D _rb;
        private Vector2 _movementInput = Vector2.zero;
        private float _rotationsInput;
    
        #region unity callback
        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
            _rb.centerOfMass = centerOfMass;
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Random.ColorHSV();
        }

        private void FixedUpdate()
        {
            _rb.AddForce(_movementInput * (linearForce * Time.fixedDeltaTime));
            var velocity = _rb.velocity;
            _rb.velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude, 0, maxLinearSpeed);
            _rb.AddTorque(_rotationsInput * angularForce * Time.fixedDeltaTime);
            _rb.angularVelocity = Mathf.Clamp(_rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);
        }

        #endregion

        #region public callback

        public void OnMove(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            _rotationsInput = context.ReadValue<float>();
        }

        #endregion
    }
}
