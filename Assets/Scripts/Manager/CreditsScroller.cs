using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    public GameObject mainMenuManager;

    public void EndAnimation()
    {
        mainMenuManager.GetComponent<MainMenu>().BackToMainMenu();
    }
}
