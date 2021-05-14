using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RacketLight : MonoBehaviour
{
    private Material lightMat;

    private void Awake()
    {
        lightMat = GetComponent<SpriteRenderer>().material;
    }

    IEnumerator SetLightIntensity(float targetIntensity, float duration)
    {
        float initialIntensity = lightMat.GetFloat("_Progress");
        float currentTime = 0;
        while (currentTime < duration)
        {
            float tempIntensity = Mathf.Lerp(initialIntensity, targetIntensity, currentTime / duration);
            lightMat.SetFloat("_Progress", tempIntensity);
            currentTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        lightMat.SetFloat("_Progress", targetIntensity);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
