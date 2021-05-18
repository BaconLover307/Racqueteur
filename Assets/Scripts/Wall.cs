using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball")) {
            AudioManager.instance.Play("BallHitWall");
        } else if (other.gameObject.CompareTag("Player"))
        {
            //AudioManager.instance.Play("RacquetHitWall");
        }
    }
    

}
