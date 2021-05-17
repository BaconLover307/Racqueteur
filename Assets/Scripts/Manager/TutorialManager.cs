using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] pages;
    public TextMeshProUGUI numberPages;
    public int selectedPages = 0;

    // Start is called before the first frame update
    void Start()
    {
        DisablePages();
        pages[selectedPages].SetActive(true);

        numberPages.text = (selectedPages + 1).ToString() + " / " + (pages.Length).ToString();
    }

    public void DisablePages()
    {
        foreach (var page in pages)
        {
            page.SetActive(false);
        }
    }

    public void NextPage()
    {
        pages[selectedPages].SetActive(false);
        selectedPages = (selectedPages + 1) % pages.Length;
        pages[selectedPages].SetActive(true);

        numberPages.text = (selectedPages + 1).ToString() + " / " + (pages.Length).ToString();
    }

    public void PrevPage()
    {
        pages[selectedPages].SetActive(false);
        selectedPages = selectedPages - 1 < 0 ? pages.Length - 1 : selectedPages - 1;
        pages[selectedPages].SetActive(true);

        numberPages.text = (selectedPages + 1).ToString() + " / " + (pages.Length).ToString();
    }
}
