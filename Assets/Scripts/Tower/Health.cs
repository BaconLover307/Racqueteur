using System;
using UnityEngine;

namespace Tower
{
    public class Health : MonoBehaviour
    {
        public float damageMultiplier = 10f;
        #region unity callback

        // Start is called before the first frame update

        void Start()
        {
        
        }

        // Update is called once per frame

        void Update()
        {
        
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Rigidbody2D>() == null) return;
            
            if (other.gameObject.CompareTag("Ball"))
            {
                //Calculate the damage based on magnitude
                var damage = other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * damageMultiplier;
                Hit(damage);
            }
        }

        #endregion

        #region private function

        private void Hit(float damage)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
