using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BorderLight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize()
    {
        StartCoroutine(TurnOnLight());
    }

    IEnumerator TurnOnLight()
    {
        Color baseColor = spriteRenderer.material.GetColor("_MainColor");
        
        if (Random.Range(0f, 1f) > .3f)
        {
            spriteRenderer.material.SetColor("_MainColor", baseColor * 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
            spriteRenderer.material.SetColor("_MainColor", baseColor / 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
        }

        if (Random.Range(0f, 1f) > .4f)
        {
            spriteRenderer.material.SetColor("_MainColor", baseColor * 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
            spriteRenderer.material.SetColor("_MainColor", baseColor / 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
        }

        if (Random.Range(0f, 1f) > .5f)
        {
            spriteRenderer.material.SetColor("_MainColor", baseColor * 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
            spriteRenderer.material.SetColor("_MainColor", baseColor / 2);
            yield return new WaitForSeconds(Random.Range(.1f, .5f));
        }
        
        spriteRenderer.material.SetColor("_MainColor", baseColor * 2);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
