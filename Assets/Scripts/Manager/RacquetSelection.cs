using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacquetSelection : MonoBehaviour
{
    public GameObject[] racquets1;
    public GameObject[] racquets2;
    public int selectedRacquet1;
    public int selectedRacquet2;

    public void Start()
    {
        selectedRacquet1 = PlayerPrefs.GetInt("Racquet1", 0);
        racquets1[selectedRacquet1].SetActive(true);
        selectedRacquet2 = PlayerPrefs.GetInt("Racquet2", 0);
        racquets2[selectedRacquet2].SetActive(true);
    }

    public void nextRacquet1()
    {
        racquets1[selectedRacquet1].SetActive(false);
        selectedRacquet1 = (selectedRacquet1 + 1) % racquets1.Length;
        racquets1[selectedRacquet1].SetActive(true);
    }

    public void nextRacquet2()
    {
        racquets2[selectedRacquet2].SetActive(false);
        selectedRacquet2 = (selectedRacquet2 + 1) % racquets2.Length;
        racquets2[selectedRacquet2].SetActive(true);
    }

    public void prevRacquet1()
    {
        racquets1[selectedRacquet1].SetActive(false);
        selectedRacquet1--;
        if (selectedRacquet1 < 0)
        {
            selectedRacquet1 += racquets1.Length;
        }

        racquets1[selectedRacquet1].SetActive(true);
    }

    public void prevRacquet2()
    {
        racquets2[selectedRacquet2].SetActive(false);
        selectedRacquet2--;
        if(selectedRacquet2 < 0)
        {
            selectedRacquet2 += racquets2.Length;
        }

        racquets2[selectedRacquet2].SetActive(true);
    }
}
