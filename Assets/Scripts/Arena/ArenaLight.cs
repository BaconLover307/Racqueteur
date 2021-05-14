using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ArenaLight : MonoBehaviour
{
    public float sweepDuration;
    public float lightsUpDuration;

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
        int multiplier = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        spriteRenderer.material.SetFloat("_LightSweep", multiplier);

        float sweepOnceDuration = sweepDuration / 3;
        int step = Mathf.FloorToInt(sweepOnceDuration / Time.fixedDeltaTime);
        float delta = 2 / (float) step;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < step; j++)
            {
                float currentProgress = spriteRenderer.material.GetFloat("_LightSweep");
                spriteRenderer.material.SetFloat("_LightSweep", currentProgress + delta * multiplier * -1);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            multiplier *= -1;
            spriteRenderer.material.SetFloat("_LightSweep", multiplier);
        }

        float currentTime = 0;
        while (currentTime < lightsUpDuration)
        {
            spriteRenderer.material.SetFloat("_MinAlpha", currentTime / lightsUpDuration * 0.4f);
            currentTime += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        spriteRenderer.material.SetFloat("_MinAlpha", 0.4f);
    }
}
