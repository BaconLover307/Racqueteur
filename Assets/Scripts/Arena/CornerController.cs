using System;
using UnityEngine;

namespace Arena
{
    public class CornerController : MonoBehaviour
    {
        public float pushForce = 10f;
        public Transform pushDirection;
        
        #region unity callback

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Rigidbody2D>() == null) return;

            if (!other.gameObject.CompareTag("Ball")) return;


            PushAway(other);
        }

        #endregion

        #region private function

        private void PushAway(Component ball)
        {
            Vector2 direction = pushDirection.position - ball.transform.position;

            ball.gameObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * pushForce);
        }

        #endregion
    }
}
