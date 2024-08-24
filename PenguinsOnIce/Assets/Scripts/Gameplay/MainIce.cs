using DG.Tweening;
using GDC.Managers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainIce : MonoBehaviour
{
    [Header("Graphic")]
    [SerializeField] private SpriteRenderer iceCenter;
    [SerializeField] private SpriteRenderer iceUpLeft, iceUpRight, iceDownLeft, iceDownRight;
    //[SerializeField] private LineRenderer iceUpLine, iceDownLine, iceLeftLine, iceRightLine;
    [SerializeField] private SpriteRenderer iceUpLine, iceDownLine, iceLeftLine, iceRightLine;
    [SerializeField] private Transform vfxBroken;
    [SerializeField] private Debris debrisPrefab;
    [SerializeField] private SpriteRenderer iceCrackRendered;
    [SerializeField] private List<Sprite> iceCrackSpriteList;
    [SerializeField] private Color hurtColor, healColor;

    [Header("Stat")]
    [SerializeField] private float scaleSpeed;
    [SerializeField] private Vector2 minScale, maxScale;
    private bool isCanControl;

    [Header("Collider")]
    [SerializeField] private BoxCollider2D coll;

    public void Setup()
    {
        isCanControl = true;
    }
    // Update is called once per frame
    private void Update()
    {
        InputScale();
        UpdateGraphic();
    }
    private void InputScale()
    {
        if (GameplayManager.Instance.state != GDC.Enums.GameplayState.PLAYING || isCanControl == false) return;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            iceUpLeft.transform.position += new Vector3(0, scaleSpeed * Time.deltaTime, 0);
            iceUpRight.transform.position += new Vector3(0, scaleSpeed * Time.deltaTime, 0);
        }
        else
        {
            iceUpLeft.transform.position -= new Vector3(0, scaleSpeed * 2 * Time.deltaTime, 0);
            iceUpRight.transform.position -= new Vector3(0, scaleSpeed * 2 * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            iceDownLeft.transform.position -= new Vector3(0, scaleSpeed * Time.deltaTime, 0);
            iceDownRight.transform.position -= new Vector3(0, scaleSpeed * Time.deltaTime, 0);
        }
        else
        {
            iceDownLeft.transform.position += new Vector3(0, scaleSpeed * 2 * Time.deltaTime, 0);
            iceDownRight.transform.position += new Vector3(0, scaleSpeed * 2 * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            iceDownLeft.transform.position -= new Vector3(scaleSpeed * Time.deltaTime, 0);
            iceUpLeft.transform.position -= new Vector3(scaleSpeed * Time.deltaTime, 0);
        }
        else
        {
            iceDownLeft.transform.position += new Vector3(scaleSpeed * 2 * Time.deltaTime, 0);
            iceUpLeft.transform.position += new Vector3(scaleSpeed * 2 * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            iceUpRight.transform.position += new Vector3(scaleSpeed * Time.deltaTime, 0);
            iceDownRight.transform.position += new Vector3(scaleSpeed * Time.deltaTime, 0);
        }
        else
        {
            iceUpRight.transform.position -= new Vector3(scaleSpeed * 2 * Time.deltaTime, 0);
            iceDownRight.transform.position -= new Vector3(scaleSpeed * 2 * Time.deltaTime, 0);
        }

        iceUpLeft.transform.position = new Vector2(Mathf.Clamp(iceUpLeft.transform.position.x, -maxScale.x, -minScale.x), Mathf.Clamp(iceUpLeft.transform.position.y, minScale.y, maxScale.y));
        iceDownLeft.transform.position = new Vector2(Mathf.Clamp(iceDownLeft.transform.position.x, -maxScale.x, -minScale.x), Mathf.Clamp(iceDownLeft.transform.position.y, -maxScale.y, -minScale.y));
        iceUpRight.transform.position = new Vector2(Mathf.Clamp(iceUpRight.transform.position.x, minScale.x, maxScale.x), Mathf.Clamp(iceUpRight.transform.position.y, minScale.y, maxScale.y));
        iceDownRight.transform.position = new Vector2(Mathf.Clamp(iceDownRight.transform.position.x, minScale.x, maxScale.x), Mathf.Clamp(iceDownRight.transform.position.y, -maxScale.y, -minScale.y));

        //iceCenter.localScale += new Vector3(xAxis, yAxis) * scaleSpeed * Time.deltaTime;
        //iceCenter.localScale = new Vector3(Mathf.Clamp(iceCenter.localScale.x, minScale, maxScale), Mathf.Clamp(iceCenter.localScale.y, minScale, maxScale));
    }
    public void ToDefaultSize(bool isFishAttack = false)
    {
        isCanControl = false;

        iceUpLeft.transform.DOLocalMove(new Vector3(-minScale.x, minScale.y),0.5f);
        iceDownLeft.transform.DOLocalMove(new Vector3(-minScale.x, -minScale.y),0.5f);
        iceUpRight.transform.DOLocalMove(new Vector3(minScale.x, minScale.y),0.5f);
        iceDownRight.transform.DOLocalMove(new Vector3(minScale.x, -minScale.y), 0.5f);
        if (isFishAttack == false) //this is lose
        {
            StartCoroutine(CorBroken());
        }
        else
        {
            StartCoroutine(CorCanControl());
        }
    }
    private IEnumerator CorCanControl()
    {
        yield return new WaitForSeconds(1.1f);
        if (GameplayManager.Instance.state == GDC.Enums.GameplayState.PLAYING)
            isCanControl = true;
    }
    private IEnumerator CorBroken()
    {
        yield return new WaitForSeconds(1.5f);

        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_HIT);

        iceUpLeft.gameObject.SetActive(false);
        iceUpRight.gameObject.SetActive(false);
        iceDownLeft.gameObject.SetActive(false);
        iceDownRight.gameObject.SetActive(false);
        iceCenter.gameObject.SetActive(false);
        iceLeftLine.gameObject.SetActive(false);
        iceRightLine.gameObject.SetActive(false);
        iceUpLine.gameObject.SetActive(false);
        iceDownLine.gameObject.SetActive(false);
        iceCrackRendered.gameObject.SetActive(false);

        vfxBroken.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnRange = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
            Debris debris = Instantiate(debrisPrefab, transform.position + spawnRange, Quaternion.identity);
            debris.Setup();
        }
    }
    public void IncreaseHp(int value, int hp)
    {
        iceCrackRendered.sprite = iceCrackSpriteList[hp];

        iceUpLeft.color = Color.white;
        iceUpRight.color = Color.white;
        iceDownLeft.color = Color.white;
        iceDownRight.color = Color.white;
        iceCenter.color = Color.white;
        iceLeftLine.color = Color.white;
        iceRightLine.color = Color.white;
        iceUpLine.color = Color.white;
        iceDownLine.color = Color.white;
 
        Color effectColor;
        if (value < 0)
        {
            effectColor = hurtColor;
        }
        else
        {
            effectColor = healColor;
        }

        DOTween.Kill(iceUpLeft);
        DOTween.Kill(iceUpRight);
        DOTween.Kill(iceDownLeft);
        DOTween.Kill(iceDownRight);
        DOTween.Kill(iceCenter);
        DOTween.Kill(iceLeftLine); 
        DOTween.Kill(iceRightLine);
        DOTween.Kill(iceUpLine);
        DOTween.Kill(iceDownLine);

        iceUpLeft.DOColor(effectColor, 0.15f).SetLoops(2, LoopType.Yoyo);
        iceUpRight.DOColor(effectColor, 0.15f).SetLoops(2, LoopType.Yoyo);
        iceDownLeft.DOColor(effectColor, 0.15f).SetLoops(2, LoopType.Yoyo);
        iceDownRight.DOColor(effectColor, 0.15f).SetLoops(2, LoopType.Yoyo);
        iceCenter.DOColor(effectColor, 0.15f).SetLoops(2, LoopType.Yoyo);
        iceLeftLine.DOColor(effectColor, 0.15f).SetLoops(2, LoopType.Yoyo);
        iceRightLine.DOColor(effectColor, 0.15f).SetLoops(2, LoopType.Yoyo);
        iceUpLine.DOColor(effectColor, 0.15f).SetLoops(2, LoopType.Yoyo);
        iceDownLine.DOColor(effectColor, 0.15f).SetLoops(2, LoopType.Yoyo);
    }
    private void UpdateGraphic()
    {
        //iceUpLeft.position = iceCenter.position + new Vector3(-iceCenter.localScale.x / 2, iceCenter.localScale.y / 2);
        //iceDownLeft.position = iceCenter.position + new Vector3(-iceCenter.localScale.x / 2, -iceCenter.localScale.y / 2);
        //iceUpRight.position = iceCenter.position + new Vector3(iceCenter.localScale.x / 2, iceCenter.localScale.y / 2);
        //iceDownRight.position = iceCenter.position + new Vector3(iceCenter.localScale.x / 2, -iceCenter.localScale.y / 2);
        iceCenter.transform.position = new Vector2((iceUpLeft.transform.position.x + iceUpRight.transform.position.x) / 2, (iceUpLeft.transform.position.y + iceDownLeft.transform.position.y) / 2);
        iceCenter.transform.localScale = new Vector2(iceUpRight.transform.position.x - iceUpLeft.transform.position.x, iceUpLeft.transform.position.y - iceDownLeft.transform.position.y);

        iceUpLine.transform.position = new Vector2(iceCenter.transform.position.x, iceUpLeft.transform.position.y);
        iceDownLine.transform.position = new Vector2(iceCenter.transform.position.x, iceDownLeft.transform.position.y);
        iceUpLine.transform.localScale = iceDownLine.transform.localScale = new Vector2(iceCenter.transform.localScale.x, 1);

        iceLeftLine.transform.position = new Vector2(iceUpLeft.transform.position.x ,iceCenter.transform.position.y);
        iceRightLine.transform.position = new Vector2(iceUpRight.transform.position.x, iceCenter.transform.position.y);
        iceLeftLine.transform.localScale = iceRightLine.transform.localScale = new Vector2(1, iceCenter.transform.localScale.y);

        //iceUpLine.SetPosition(1, iceUpLeft.position);
        //iceUpLine.SetPosition(0, iceUpRight.position);
        //iceDownLine.SetPosition(1, iceDownLeft.position);
        //iceDownLine.SetPosition(0, iceDownRight.position);
        //iceLeftLine.SetPosition(1, iceDownLeft.position);
        //iceLeftLine.SetPosition(0, iceUpLeft.position);
        //iceRightLine.SetPosition(1, iceUpRight.position);
        //iceRightLine.SetPosition(0, iceDownRight.position);

        coll.size = iceCenter.transform.localScale + Vector3.one;
        coll.offset = iceCenter.transform.position;
    }
    public bool CheckCanMove(Land land)
    {
        switch (land.landDirect)
        {
            case GDC.Enums.LandDirect.UP:
                return ((int)Mathf.Round(iceUpLeft.transform.position.y) == (int)Mathf.Round(maxScale.y));
            case GDC.Enums.LandDirect.DOWN:
                return ((int)Mathf.Round(iceDownLeft.transform.position.y) == (int)Mathf.Round(-maxScale.y));
            case GDC.Enums.LandDirect.LEFT:
                return ((int)Mathf.Round(iceDownLeft.transform.position.x) == (int)Mathf.Round(-maxScale.x));
            case GDC.Enums.LandDirect.RIGHT:
                return ((int)Mathf.Round(iceDownRight.transform.position.x) == (int)Mathf.Round(maxScale.x));
            case GDC.Enums.LandDirect.DOWN_LEFT:
                return ((int)Mathf.Round(iceDownLeft.transform.position.y) == (int)Mathf.Round(-maxScale.y))
                    && ((int)Mathf.Round(iceDownLeft.transform.position.x) == (int)Mathf.Round(-maxScale.x));
            case GDC.Enums.LandDirect.UP_LEFT:
                return ((int)Mathf.Round(iceUpLeft.transform.position.y) == (int)Mathf.Round(maxScale.y))
                    && ((int)Mathf.Round(iceUpLeft.transform.position.x) == (int)Mathf.Round(-maxScale.x));
            case GDC.Enums.LandDirect.DOWN_RIGHT:
                return ((int)Mathf.Round(iceDownRight.transform.position.y) == (int)Mathf.Round(-maxScale.y))
                    && ((int)Mathf.Round(iceDownRight.transform.position.x) == (int)Mathf.Round(maxScale.x));
            case GDC.Enums.LandDirect.UP_RIGHT:
                return ((int)Mathf.Round(iceUpRight.transform.position.y) == (int)Mathf.Round(maxScale.y))
                    && ((int)Mathf.Round(iceDownRight.transform.position.x) == (int)Mathf.Round(maxScale.x));
        }
        return false; //Never reach this line
    }    
}
