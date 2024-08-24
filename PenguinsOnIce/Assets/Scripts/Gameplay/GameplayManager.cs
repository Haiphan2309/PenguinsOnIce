using GDC.Enums;
using GDC.Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public LandManager landManager;
    [SerializeField] private IceManager iceManager;
    [SerializeField] private UIGameplay uiGameplay;
    public MainIce mainIce;

    public int maxHp, maxTime;
    public GameplayState state;
    [SerializeField,ReadOnly] private int hp, time, totalTime, point;

    private Coroutine timeCor;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    private void Start()
    {
        SoundManager.Instance.PlayMusic(AudioPlayer.SoundID.GAMEPLAY_1);
        state = GameplayState.START_GAME;
        StartCoroutine(CorShowTutorial());
    }
    private IEnumerator CorShowTutorial()
    {
        yield return new WaitForSeconds(1.5f);
        PopupManager.Instance.ShowTutorial();
    }

    public void Setup()
    {
        hp = maxHp;
        time = maxTime;
        totalTime = 0;
        point = 0;
        state = GameplayState.PLAYING;
        landManager.Setup();
        iceManager.Setup();
        uiGameplay.Setup();
        mainIce.Setup();
        timeCor = StartCoroutine(CorTime());
    }
    public void IncreaseHp(int value)
    {
        hp += value;
        if (value < 0)
        {
            Debug.Log("Decrease HP");
            
            landManager.AllPenguinOnIceJump();
            Camera.main.GetComponent<Animator>().Play("CameraShake");
            if (hp <= 0)
            {
                hp = 0;
                Lose();
            }
        }
        else
        {
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_HEAL);
            if (hp > maxHp)
            {
                hp = maxHp;
            }
            Debug.Log("Increase HP");
        }
        mainIce.IncreaseHp(value, hp);
        uiGameplay.SetHp(hp);
    }
    public void IncreasePoint(int value)
    {
        point += value;
        uiGameplay.SetPoint(point);
    }
    public void IncreaseTime(int value)
    {
        time += value;
        uiGameplay.SetTime(time);
    }
    [Button]
    private void Lose()
    {
        if (state == GameplayState.END_GAME) return;

        Debug.Log("Lose");
        state = GameplayState.END_GAME;
        mainIce.ToDefaultSize();
        StopCoroutine(timeCor);

        StartCoroutine(CorLose());
    }
    private IEnumerator CorLose()
    {
        if (time <= 0)
        {
            yield return new WaitForSeconds(2);
            PopupManager.Instance.ShowLosePanel(true);
        }
        else
        {
            yield return new WaitForSeconds(3.5f);
            PopupManager.Instance.ShowLosePanel(false);
        }
    }
    public int GetPoint()
    {
        return point;
    }
    private IEnumerator CorTime()
    {
        yield return new WaitForSeconds(1);
        time--;
        totalTime++;
        iceManager.SetStateStat(totalTime);
        if (time<=0)
        {
            Lose();
        }
        else
        {
            timeCor = StartCoroutine(CorTime());
        }
        uiGameplay.SetTime(time);
    }
}
