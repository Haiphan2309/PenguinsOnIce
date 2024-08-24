using DG.Tweening;
using GDC.Enums;
using GDC.Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D coll;
    [SerializeField] private List<SpriteRenderer> scarfRenderedList;
    [SerializeField] private Transform waterSplashPrefab, heartTrans, timeTrans;

    private Vector3 targetPos/*, endLandPos*/;
    [SerializeField] private float moveSpeed;
    [SerializeField, ReadOnly] private int spawnLandIndex, targetLandIndex;
    [SerializeField] private int point;
    [SerializeField] private GameObject pointObj;
    [SerializeField] private bool isHoldHeart, isHoldTime;

    private Coroutine moveCor, landCor;
    [SerializeField, ReadOnly] private PenguinState state;
    [SerializeField,ReadOnly] private bool isMoveToEndLand;
    [SerializeField,ReadOnly] private bool /*isOnMainIce, */isJumping, isLanding; //isLanding dung de xac dinh no co dang thuc hien anim land tren mat dat ko
    [ReadOnly] public bool isOnMainIce;
    public void Setup(int spawnLandIndex, int targetLandIndex)
    {
        this.targetLandIndex = targetLandIndex;
        this.spawnLandIndex = spawnLandIndex;
        transform.position = LandManager.Instance.landList[spawnLandIndex].spawnPos;
        //endLandPos = LandManager.Instance.landList[targetLandIndex].pos;
        foreach (var renderer in scarfRenderedList)
        {
            renderer.color = LandManager.Instance.landList[targetLandIndex].color;
        }
        state = PenguinState.TO_START;

        MoveToPos(LandManager.Instance.landList[spawnLandIndex].pos);
        isMoveToEndLand = true;
        isOnMainIce = false;
        isJumping = false;
        isLanding = true;
    }
    private void MoveToPos(Vector2 pos)
    {
        targetPos = pos;
        if (moveCor != null)
        {
            StopCoroutine(moveCor);
        }
        moveCor = StartCoroutine(CorMove());
    }
    private void JumpToPos(Vector3 pos, float jumpPower = 1f, float jumpTime = 1f, bool isLie = false, bool isPlaySound = true)
    {
        isJumping = true;
        coll.enabled = false;

        if (isPlaySound)
        {
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_JUMP);
        }

        CalAnimParam(pos);
        anim.Play("FlyBlendTree");

        Vector3 dir = (pos-transform.position).normalized;
        transform.DOMove(pos, jumpTime).SetEase(Ease.InOutSine);
        anim.transform.DOLocalJump(Vector2.zero,jumpPower,1,jumpTime).SetEase(Ease.InOutSine).OnComplete(() =>
        {            
            anim.transform.rotation = Quaternion.identity;
            isJumping = false;
            isLanding = true;
            coll.enabled = true;
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_LAND);
            if (isLie)
            {
                transform.DOMove(transform.position + dir * 0.5f, 0.5f);
                anim.Play("LieBlendTree");
            }
            else
            {
                anim.Play("LandBlendTree");
            }
            landCor = StartCoroutine(CorLanding(isLie));
            //isOnMainIce = true; //Gia dinh la vay, neu ko phai thi ontriggerstay se xu li
        });
    }
    private void JumpToIce()
    {
        JumpToPos(LandManager.Instance.landList[spawnLandIndex].towardPos);
        state = PenguinState.TO_LAND;
        LandManager.Instance.landList[spawnLandIndex].RemovePenguin();
        isMoveToEndLand = false;
        MoveToPos(Vector2.zero); //to center
    }
    public void JumpRandom()
    {
        if (isJumping) return;
        if (moveCor != null)
        {
            StopCoroutine(moveCor);
        }

        float xDistance = Random.Range(-3, 3);
        float yDistance = Random.Range(-3, 3);
        float jumpPower = Random.Range(1, 3);
        float jumpTime = Mathf.Clamp(jumpPower / 1.25f, 0, 1f);
        DOTween.Kill(anim.transform);
        anim.transform.DORotate(new Vector3(0,0,-Mathf.Clamp(xDistance*jumpPower*10,-60,60)),jumpTime).SetEase(Ease.Linear);
        Vector2 randomPos = transform.position + new Vector3(xDistance, yDistance);
        
        JumpToPos(randomPos, jumpPower, jumpTime, true, false);
    }
    private IEnumerator CorLanding(bool isLie = false)
    {
        if (isLie == false)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            if (anim.GetFloat("xDir") > 0)
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            anim.transform.DORotate(Vector3.zero, 0.5f);
        }
        isLanding = false;

        if (state != PenguinState.FINISH)
        {
            if (LandManager.Instance.CheckCanMove(targetLandIndex))
            {
                isMoveToEndLand = true;
                MoveToPos(LandManager.Instance.landList[targetLandIndex].towardPos);
            }
            else
            {
                isMoveToEndLand = false;
                MoveToPos(Vector2.zero); //to center
            }
        }
    }
    private void Update()
    {
        if (state != PenguinState.TO_LAND || isJumping || isLanding) return;

        if (isMoveToEndLand == false && LandManager.Instance.CheckCanMove(targetLandIndex))
        {
            isMoveToEndLand = true;
            MoveToPos(LandManager.Instance.landList[targetLandIndex].towardPos);
        }
        else if (isMoveToEndLand && LandManager.Instance.CheckCanMove(targetLandIndex) == false)
        {
            isMoveToEndLand = false;
            MoveToPos(Vector2.zero); //to center
        }   
    }
    private void LateUpdate()
    {
        CheckCollider();
    }
    private void CalAnimParam(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        anim.SetFloat("xDir", dir.x);
        anim.SetFloat("yDir", dir.y);
    }
    private IEnumerator CorMove()
    {
        yield return new WaitUntil(() => isJumping == false && isLanding == false);

        CalAnimParam(targetPos);
        anim.Play("MoveBlendTree");

        while (Vector2.SqrMagnitude(transform.position - targetPos) > 0.2f)
        {
            Vector3 dir = (targetPos - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
            yield return null;
        }

        if (state == PenguinState.TO_START)
        {
            state = PenguinState.WAITING;
            yield return new WaitUntil(()=>LandManager.Instance.CheckCanMove(spawnLandIndex));
            JumpToIce();
        }
        else if (state == PenguinState.FINISH)
        {
            Destroy(gameObject);
        }

        if ((int)Mathf.Round(targetPos.x) == (int)Mathf.Round(LandManager.Instance.landList[targetLandIndex].towardPos.x)
            && (int)Mathf.Round(targetPos.y) == (int)Mathf.Round(LandManager.Instance.landList[targetLandIndex].towardPos.y)) //Da toi duoc towardPos cua end land
        {
            JumpToPos(LandManager.Instance.landList[targetLandIndex].pos);
        }
    }
    private void Finish()
    {
        GameObject pointObject = Instantiate(pointObj, transform.position + new Vector3(1.65f,0.5f,0), Quaternion.identity);
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_POINT);
        Destroy(pointObject, 1.5f);
        GameplayManager.Instance.IncreasePoint(point);
        state = PenguinState.FINISH;
        LandManager.Instance.RemovePenguin(this);
        MoveToPos(LandManager.Instance.landList[targetLandIndex].spawnPos);
    }
    private void Die()
    {
        Debug.Log("Die");
        GameObject waterSplash = Instantiate(waterSplashPrefab, transform.position, Quaternion.identity).gameObject;
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_WATER_SPLASH);
        Destroy(waterSplash, 3);
        state = PenguinState.DEAD;
        StopCoroutine(moveCor);
        if (landCor != null)
        {
            StopCoroutine(landCor);
        }
        LandManager.Instance.RemovePenguin(this);
        Destroy(gameObject);
    }
    private void CheckCollider()
    {
        if (isJumping) return;

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        if (colls != null)
        {
            bool isOnMainIce = false;
            foreach (var coll in colls)
            {
                if (state == PenguinState.TO_LAND)
                {
                    if (coll.CompareTag("MainIce"))
                    {
                        isOnMainIce = true;
                    }
                    else if (coll.CompareTag("Land"))
                    {
                        Land land = coll.GetComponent<Land>();
                        if (land.index == targetLandIndex)
                        {
                            Finish();
                            return;
                        }
                    }
                }
                
                if (coll.CompareTag("Penguin") && coll.GetInstanceID() != this.coll.GetInstanceID()) //Cham vao penguin khac, di xa ra
                {
                    if (state == PenguinState.WAITING)
                    {
                        if (moveCor != null)
                        {
                            StopCoroutine(moveCor);
                        }
                        JumpToIce();
                        return;
                    }
                    else if (state == PenguinState.TO_LAND)
                    {
                        Vector3 avoidDir = (transform.position - coll.transform.position).normalized;
                        transform.position += avoidDir * moveSpeed * Time.deltaTime;
                    }
                }
            }

            this.isOnMainIce = isOnMainIce;
            if (isOnMainIce == false && state == PenguinState.TO_LAND) Die();
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (isJumping || state == PenguinState.FINISH || state == PenguinState.TO_START) return;

    //    if (collision.CompareTag("Land"))
    //    {
    //        Land land = collision.GetComponent<Land>();
    //        if (land.index == targetLandIndex)
    //        {
    //            Finish();
    //        }
    //    }
    //    //else if (collision.CompareTag("MainIce"))
    //    //{
    //    //    isOnMainIce = true;
    //    //}
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (isJumping || state == PenguinState.FINISH || state == PenguinState.TO_START) return;

    //    if (collision.CompareTag("Land"))
    //    {
    //        if (isOnMainIce == false && state == PenguinState.TO_LAND && state != PenguinState.DEAD)
    //        {
    //            Die();
    //        }
    //    }
    //    else if (collision.CompareTag("MainIce"))
    //    {
    //        if (state == PenguinState.TO_LAND && state != PenguinState.DEAD)
    //        {
    //            Die();
    //        }
    //    }
    //}
}
