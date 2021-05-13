using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BorderLight : MonoBehaviour
{
    public float flashDuration = .3f;
    private SpriteRenderer spriteRenderer;

    private Color offColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offColor = spriteRenderer.material.GetColor("_MainColor");
    }

    public void Initialize()
    {
        StartCoroutine(TurnOnLight());
    }

    IEnumerator TurnOnLight()
    {
        if (Random.Range(0f, 1f) > .3f)
        {
            spriteRenderer.material.SetColor("_MainColor", offColor * 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
            spriteRenderer.material.SetColor("_MainColor", offColor / 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
        }

        if (Random.Range(0f, 1f) > .4f)
        {
            spriteRenderer.material.SetColor("_MainColor", offColor * 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
            spriteRenderer.material.SetColor("_MainColor", offColor / 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
        }

        if (Random.Range(0f, 1f) > .5f)
        {
            spriteRenderer.material.SetColor("_MainColor", offColor * 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
            spriteRenderer.material.SetColor("_MainColor", offColor / 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
        }
        
        spriteRenderer.material.SetColor("_MainColor", offColor * 2);
    }

    IEnumerator Flash(float duration)
    {
        float current = 0f;
        Color deltaColor = offColor * -2;
        Color flashColor = offColor * 4;

        spriteRenderer.material.SetColor("_MainColor", flashColor);
        while (current < duration)
        {
            spriteRenderer.material.SetColor("_MainColor", flashColor + current / duration * deltaColor);
            current += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        spriteRenderer.material.SetColor("_MainColor", offColor * 2);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            StopCoroutine(Flash(flashDuration));
            StartCoroutine(Flash(flashDuration));
        }
    }
}
