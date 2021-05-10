using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLine : MonoBehaviour
{
    // Filter all collider which collider will be abandoned and considered
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else if ((collision.gameObject.tag == "Player") && (collision.gameObject.GetComponent<Collider2D>().GetType() == typeof(BoxCollider2D))) 
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
