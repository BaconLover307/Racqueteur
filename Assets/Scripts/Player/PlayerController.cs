using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public float linearForce = 500f;
        public float maxLinearSpeed = 100f;
        public float angularForce = 500f;
        public float burstSpeed = 2500f;
        public float burstDelay = 1f;
        public float maxAngularSpeed = 500f;
        public Vector2 centerOfMass;

        private Rigidbody2D _rb;
        private Vector2 _movementInput = Vector2.zero;
        private float _rotationsInput;
        private float doubleTapDelay = 0.3f;
        private float lastTimestamp;
        private float lastBurstTimeStamp;
        private float lastTap;
        private bool isDoubleTap;

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
            _rb.AddForce(_movementInput * (linearForce * Time.fixedDeltaTime * 2), ForceMode2D.Impulse);
            var velocity = _rb.velocity;
            _rb.velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude, 0, maxLinearSpeed);

            if (isDoubleTap)
            {
                _rb.angularVelocity = Mathf.Clamp(_rotationsInput * burstSpeed, -burstSpeed, burstSpeed);
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
            if (context.started)
            {
                var tapDelayTime = Time.fixedTime - lastTimestamp;
                var burstDelayTime = Time.fixedTime - lastBurstTimeStamp;
                if (tapDelayTime < doubleTapDelay && burstDelayTime > burstDelay && lastTap == _rotationsInput)
                {
                    isDoubleTap = true;
                    lastBurstTimeStamp = Time.fixedTime;
                }

                lastTap = _rotationsInput;
                lastTimestamp = Time.fixedTime;
            }
        }

        public void OnFlip(InputAction.CallbackContext context)
        {
            Vector3 newScale = this.transform.localScale;
            if (newScale == null) return;
            newScale.x *= -1;
            this.transform.localScale = newScale;
        }

        public void OnCover(InputAction.CallbackContext context)
        {
            if (!gameObject.scene.IsValid())
            {
                return;
            }
            Debug.Log("Cover Pressed");
        }

        #endregion
    }
}
