using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SimpleButton : MonoBehaviour
{
    public string clickFX = "ButtonClick";
    public string hoverFX = "ButtonHover";

    public void OnClick()
    {
        if (clickFX != null) AudioManager.instance.PlayOneShot(clickFX);
    }

    public void OnHover()
    {
        if (hoverFX != null) AudioManager.instance.PlayOneShot(hoverFX);
    }

}
