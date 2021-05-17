using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDestroy : MonoBehaviour
{
    public GameObject fullModel;
    public GameObject shatteredModel;
    public float explosionForce = 100f;
    public float explosionForceDeviation = 10f;
    public float explosionRadius = 6f;
    public LayerMask mask;
    public Collider2D[] towerColliders;

    private Vector2 explosionOrigin;

    public void Shatter()
    {
        foreach (Collider2D collider in towerColliders)
        {
            collider.enabled = false;
        }

        fullModel.SetActive(false);
        shatteredModel.SetActive(true);

        explosionOrigin = new Vector2(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f));
        Collider2D[] pieces = Physics2D.OverlapCircleAll(explosionOrigin, explosionRadius, mask);
        foreach (Collider2D piece in pieces)
        {
            Rigidbody2D rb = piece.attachedRigidbody;
            Vector2 piecePosition = piece.bounds.center;
            rb.AddForce((piecePosition - explosionOrigin) * (explosionForce + Random.Range(-explosionForceDeviation, explosionForceDeviation)), ForceMode2D.Force);
        }
    }
}
