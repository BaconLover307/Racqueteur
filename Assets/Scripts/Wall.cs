using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball")) {
            ProcessAudio(other.GetContact(0));
        } else if (other.gameObject.CompareTag("Player"))
        {
            //AudioManager.instance.Play("RacquetHitWall");
        }
    }

    private void ProcessAudio(ContactPoint2D contact)
    {
        var maxImpulse = 100f;
        AudioManager.instance.PlayWithVolume("BallHitWall", Mathf.Clamp(1.5f*contact.normalImpulse / maxImpulse, 0.2f, 8f));
    }

}
