using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] private Sprite heartSprite, grayHeartSprite;
    [SerializeField] private List<Image> heartImageList = new List<Image>();
    [SerializeField] private Image timeImage;
    [SerializeField] private TMP_Text timeText, pointText;

    public void Setup()
    {
        SetHp(GameplayManager.Instance.maxHp);
        SetPoint(0);
        SetTime(GameplayManager.Instance.maxTime);
    }
    public void SetHp(int value)
    {
        for (int i = 0; i<GameplayManager.Instance.maxHp; i++)
        {
            //Debug.Log(i + " " + value);
            if (i < value)
            {
                if (heartImageList[i].sprite == grayHeartSprite) //Increase Hp
                {
                    heartImageList[i].sprite = heartSprite;
                    DOTween.Kill(heartImageList[i].rectTransform);
                    heartImageList[i].rectTransform.localScale = Vector3.one;
                    heartImageList[i].rectTransform.DOScale(1, 0.3f).SetEase(Ease.OutBounce);
                }
            }
            else
            {
                //Debug.Log("In SetgrayHeart");
                if (heartImageList[i].sprite == heartSprite) //Decrease Hp
                {
                    //Debug.Log("SetgrayHeart");
                    DOTween.Kill(heartImageList[i].rectTransform);
                    heartImageList[i].sprite = grayHeartSprite;
                    heartImageList[i].rectTransform.DOScale(0.7f, 0.15f).SetLoops(2, LoopType.Yoyo);
                }
            }
        }
    }
    public void SetPoint(int value)
    {
        pointText.text = "POINT: " + value.ToString();
    }
    public void SetTime(int value)
    {
        if (value <= 30)
        {
            timeText.color = Color.yellow;
        }
        else
        {
            timeText.color = Color.white;
        }
        timeText.text = value.ToString() + "s"; 
    }
}