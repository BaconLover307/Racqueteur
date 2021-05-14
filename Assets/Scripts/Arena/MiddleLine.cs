using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLine : MonoBehaviour
{
    public void Start()
    {
        GameObject ballObject = GameObject.FindGameObjectWithTag("Ball");
        Physics2D.IgnoreCollision(ballObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObject in playerObjects)
        {
            Collider2D[] colliders = playerObject.GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                if (collider.GetType() == typeof(BoxCollider2D)
                || collider.GetType() == typeof(PolygonCollider2D)) 
                {
                    Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>());
                }
            }
        }
        
    }
}
