using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public Transform damageCanvas;
    public GameObject damageTextObj;
    public float forceMultiplier = 2f;

    public void Spawn(int damage, Vector3 position, Vector2 forceDirection, bool isWeakPoint)
    {
        GameObject damageText = Instantiate(damageTextObj, position, Quaternion.identity, damageCanvas);
        damageText.GetComponent<TMP_Text>().text = damage.ToString();
        if (isWeakPoint)
        {
            damageText.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
        Rigidbody2D rb = damageText.GetComponent<Rigidbody2D>();
        rb.AddForce(forceDirection * forceMultiplier, ForceMode2D.Impulse);
        Destroy(damageText, 2f);
    }
}
