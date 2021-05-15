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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        } else if (collision.gameObject.CompareTag("Player"))
        {
                Collider2D collider = collision.gameObject.GetComponent<Collider2D>();
                if (collider.GetType() == typeof(BoxCollider2D)
                || collider.GetType() == typeof(PolygonCollider2D)) 
                {
                    Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>());
                }

        }
    }
}
