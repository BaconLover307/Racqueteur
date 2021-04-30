using System;
using UnityEngine;

namespace Tower
{
    public class Health : MonoBehaviour
    {
        public float damageMultiplier = 10f;
        #region unity callback

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Rigidbody2D>() == null) return;

            if (!other.gameObject.CompareTag("Ball")) return;
            
            //Calculate the damage based on magnitude
            var damage = other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * damageMultiplier;
            Hit(damage);
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
