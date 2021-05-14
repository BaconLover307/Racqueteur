using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration;
    public float maxAmplitude;
    public VolumeProfile globalVolume;

    private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin noise;
    
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float amplitude, float frequency=1f)
    {
        StopCoroutine(ProcessShake(amplitude, frequency, shakeDuration));
        StartCoroutine(ProcessShake(amplitude, frequency, shakeDuration));
    }

    IEnumerator ProcessShake(float amplitude, float frequency, float duration)
    {
        ChromaticAberration caSetting;
        globalVolume.TryGet(out caSetting);

        caSetting.intensity.Override(1);
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        float currentTime = 0f;
        while (currentTime < shakeDuration)
        {
            caSetting.intensity.Override(1 - currentTime / shakeDuration);
            currentTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        caSetting.intensity.Override(0);
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
