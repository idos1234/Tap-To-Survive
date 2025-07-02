using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundScript : MonoBehaviour
{
    public Image SoundImage;
    public Sprite SoundOnSprite;
    public Sprite SoundOffSprite;
    public AudioSource GameSound;
    public bool isSoundOn = true;

    private void SoundOff()
    {
        SoundImage.sprite = SoundOffSprite;
        GameSound.Pause();
    }

    private void SoundOn()
    {
        SoundImage.sprite = SoundOnSprite;
        GameSound.UnPause();
    }

    public void ChangeSoundStatus()
    {
        if (isSoundOn)
        {
            SoundOff();
        } else
        {
            SoundOn();
        }
        isSoundOn = !isSoundOn;
    }
}
