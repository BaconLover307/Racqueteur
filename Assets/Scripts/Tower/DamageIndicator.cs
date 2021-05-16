using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public Transform damageCanvas;
    public GameObject damageTextObj;
    public float forceMultiplier = 2f;

    public void Spawn(int damage, Vector3 position, Vector2 forceDirection)
    {
        GameObject damageText = Instantiate(damageTextObj, position, Quaternion.identity, damageCanvas);
        damageText.GetComponent<TMP_Text>().text = damage.ToString();
        Rigidbody2D rb = damageText.GetComponent<Rigidbody2D>();
        rb.AddForce(forceDirection * forceMultiplier, ForceMode2D.Impulse);
        Destroy(damageText, 2f);
    }
}
