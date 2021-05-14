using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TowerLight : MonoBehaviour
{
    public Transform healthParent;
    public float lightsUpDuration;

    private GameObject[] healthDots;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        int i = 0;
        healthDots = new GameObject[healthParent.childCount];
        foreach (Transform child in healthParent)
        {
            healthDots[i] = child.gameObject;
            i += 1;
        }
    }

    public void Initialize()
    {
        StartCoroutine(TurnOnLight());
    }

    IEnumerator TurnOnLight()
    {
        float currentTime = 0;
        //while (currentTime < lightsUpDuration)
        //{
        //    spriteRenderer.material.SetFloat("_Progress", currentTime / lightsUpDuration * 0.3f);
        //    currentTime += Time.fixedDeltaTime;
        //    yield return new WaitForSeconds(Time.fixedDeltaTime);
        //}
        //spriteRenderer.material.SetFloat("_Progress", 1f);

        foreach (GameObject obj in healthDots)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(.05f);
        }
    }
}
