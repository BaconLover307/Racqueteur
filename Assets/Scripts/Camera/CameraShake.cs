using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration;
    public float maxAmplitude;

    private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin noise;
    
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float amplitude, float frequency=1f)
    {
        Debug.Log("Shake");
        StopCoroutine(ProcessShake(amplitude, frequency, shakeDuration));
        StartCoroutine(ProcessShake(amplitude, frequency, shakeDuration));
    }

    IEnumerator ProcessShake(float amplitude, float frequency, float duration)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;
        yield return new WaitForSeconds(shakeDuration);
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
