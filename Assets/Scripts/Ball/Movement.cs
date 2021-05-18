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

        #region public callback

        public void FreezeBall()
        {
            _rb.velocity = new Vector2(0,0);
            _rb.rotation = 0;
        }

        #endregion

    }
}
