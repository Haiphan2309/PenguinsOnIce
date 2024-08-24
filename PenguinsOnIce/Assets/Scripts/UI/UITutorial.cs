using GDC.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorial : UIPopup
{
    public void Show(bool isTimeUp)
    {
        base.Show();
    }
    public void Play()
    {
        base.Hide();
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_BUTTON_CLICK);
        GameplayManager.Instance.Setup();
    }
}
