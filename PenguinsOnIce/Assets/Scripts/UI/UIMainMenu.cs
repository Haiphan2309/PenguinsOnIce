using DG.Tweening;
using GDC.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    private bool isAlreadyToGameplay = false;
    [SerializeField] private RectTransform titleRect, buttonRect;
    private void Start()
    {
        SoundManager.Instance.PlayMusic(AudioPlayer.SoundID.MUSIC_MAIN_MENU);

        titleRect.anchoredPosition = new Vector2(0, 500);
        buttonRect.anchoredPosition = new Vector2(0, -500);

        titleRect.DOAnchorPosY(-252, 1.5f).SetEase(Ease.OutBack);
        buttonRect.DOAnchorPosY(308, 1.5f).SetEase(Ease.OutBack);
    }
    public void ToGameplay()
    {
        if (isAlreadyToGameplay) return;
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_BUTTON_CLICK);
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_WIN);
        isAlreadyToGameplay = true;
        GameManager.Instance.LoadSceneManually(GDC.Enums.SceneType.GAMEPLAY, GDC.Enums.TransitionType.IN);
    }
}
