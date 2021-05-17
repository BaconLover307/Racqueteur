using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public Transform damageCanvas;
    public GameObject damageTextPrefab;
    public float forceMultiplier = 2f;

    public void Spawn(int damage, Vector3 position, Vector2 forceDirection, bool isWeakPoint)
    {
        GameObject damageTextObj = Instantiate(damageTextPrefab, position, Quaternion.identity, damageCanvas);
        TMP_Text damageText = damageTextObj.GetComponent<TMP_Text>();
        damageText.text = damage.ToString();
        if (isWeakPoint)
        {
            damageTextObj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            damageText.color = new Color(.9f, 0, 0, 1);
        }
        Rigidbody2D rb = damageTextObj.GetComponent<Rigidbody2D>();
        rb.AddForce(forceDirection * forceMultiplier, ForceMode2D.Impulse);
        Destroy(damageTextObj, 2f);
    }
}
