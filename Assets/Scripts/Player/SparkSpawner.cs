using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkSpawner : MonoBehaviour
{
    public GameObject sparkPrefab;
    public Transform[] spawnLocations;

    private GameObject[] sparkObjects;

    private void Awake()
    {
        sparkObjects = new GameObject[spawnLocations.Length];
        for(int i = 0; i < sparkObjects.Length; i++)
        {
            sparkObjects[i] = Instantiate(sparkPrefab, transform);
            sparkObjects[i].SetActive(false);
        }
    }

    private void SetSparksActive(bool enable)
    {
        for (int i = 0; i < sparkObjects.Length; i++)
        {
            sparkObjects[i].SetActive(enable);
            if (enable)
            {
                sparkObjects[i].transform.position = spawnLocations[i].position;
                sparkObjects[i].transform.rotation = spawnLocations[i].rotation;
            }
        }
    }

    public IEnumerator ShowSparks()
    {
        SetSparksActive(true);
        yield return new WaitForSeconds(1);
        SetSparksActive(false);
    }
}
