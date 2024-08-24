using GDC.Enums;
using GDC.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILosePanel : UIPopup
{
    [SerializeField] TMP_Text pointText, titleText;
    public void Show(bool isTimeUp)
    {
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_LOSE);
        base.Show();
        if (isTimeUp) 
        {
            titleText.text = "TIME UP!";
        }
        else
        {
            titleText.text = "ICE BROKEN";
        }
        pointText.text = "POINT: " + GameplayManager.Instance.GetPoint().ToString();
    }
    public void Restart()
    {
        base.Hide();
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_BUTTON_CLICK);
        GameManager.Instance.LoadSceneManually(SceneType.GAMEPLAY, TransitionType.IN);
    }
    public void Back()
    {
        base.Hide();
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_BUTTON_CLICK);
        GameManager.Instance.LoadSceneManually(SceneType.MAIN_MENU, TransitionType.IN);
    }
}
