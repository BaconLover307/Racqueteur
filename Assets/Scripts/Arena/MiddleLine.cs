using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLine : MonoBehaviour
{
    private string TagToIgnore = "Ball";

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TagToIgnore)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else if (collision.gameObject.tag == "Racquet")
        {
            if(GetComponent<Collider2D>().GetType() == typeof(BoxCollider2D)) 
            {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }
    }

}
