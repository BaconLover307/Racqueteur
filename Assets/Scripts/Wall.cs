using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public AudioClip ballHitSFX;
    public AudioClip racquetHitSFX;

    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        _audioManager = AudioManager.instance;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Mashok");
        if (other.gameObject.CompareTag("Ball")) {
            _audioManager.PlaySFX(ballHitSFX);
        } else if (other.gameObject.CompareTag("Player"))
        {
            _audioManager.StopSFX();
            _audioManager.PlaySFX(racquetHitSFX);
        }
    }
    

}
