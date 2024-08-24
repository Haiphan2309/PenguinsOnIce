using GDC.Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIPopup
{
    [SerializeField] private Button menuBtn, replayBtn, hideButton;
    [SerializeField] private Slider musicSlider, soundSlider;
    private Coroutine hideCor;
    [SerializeField] private int maxVolume = 10;
    private bool isAreadySetup;

    [Button]
    public override void Show()
    {
        base.Show();

        if (isAreadySetup == false)
        {
            isAreadySetup = true;
            Setup();
        }

        if (hideCor != null)
        {
            StopCoroutine(hideCor);
        }
    }
    private void Setup()
    {
        menuBtn.onClick.AddListener(OnMenu);
        replayBtn.onClick.AddListener(OnReplay);
        hideButton.onClick.AddListener(Hide);

        musicSlider.onValueChanged.AddListener(delegate { OnChangeMusicVolume(); });
        soundSlider.onValueChanged.AddListener(delegate { OnChangeSoundVolume(); });
        musicSlider.maxValue = maxVolume;
        soundSlider.maxValue = maxVolume;
        musicSlider.value = SoundManager.Instance.GetMusicVolume() * maxVolume;
        soundSlider.value = SoundManager.Instance.GetSFXVolume() * maxVolume;
    }
    public override void Hide()
    {
        base.Hide();
    }
    public void OnMenu()
    {
        //int curChapterIndex = GameplayManager.Instance.chapterData.id;
        //GameManager.Instance.LoadSceneManually(
        //    GDC.Enums.SceneType.MAINMENU,
        //    GDC.Enums.TransitionType.IN,
        //    SoundType.NONE,
        //    cb: () =>
        //    {
        //        //    //GDC.Managers.GameManager.Instance.SetInitData(levelIndex);
        //        GameManager.Instance.LoadMenuLevel(curChapterIndex);
        //    },
        //    true);
    }

    public void OnReplay()
    {
        //int currentChapterIndex = GameplayManager.Instance.chapterData.id;
        //int currentLevelIndex = GameplayManager.Instance.levelData.id;
        //GameManager.Instance.LoadSceneManually(
        //    GDC.Enums.SceneType.GAMEPLAY,
        //    GDC.Enums.TransitionType.IN,
        //    SoundType.NONE,
        //    cb: () =>
        //    {
        //        GDC.Managers.GameManager.Instance.SetInitData(currentChapterIndex, currentLevelIndex);
        //    },
        //    true);
    }

    void OnChangeMusicVolume()
    {
        SoundManager.Instance.SetMusicVolume((float)musicSlider.value/maxVolume);
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_TOUCH);
    }
    void OnChangeSoundVolume()
    {
        SoundManager.Instance.SetSFXVolume((float)soundSlider.value / maxVolume);
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_TOUCH);
    }
}
