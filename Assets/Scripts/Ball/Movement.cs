using UnityEngine;

namespace Ball
{
    public class Movement : MonoBehaviour
    {
        public float maxSpeed = 100f;

        public float initialForce = 100f;

        private Rigidbody2D _rb;

        #region unity callback

        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
            var direction = Random.insideUnitCircle.normalized;
            _rb.AddForce(direction * initialForce);
        }

        void Update()
        {
            var velocity = _rb.velocity;
            _rb.velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude, 0, maxSpeed);
        }

        #endregion
    }
}
