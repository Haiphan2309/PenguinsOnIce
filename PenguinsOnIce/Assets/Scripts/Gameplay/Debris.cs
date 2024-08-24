using DG.Tweening;
using GDC.Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField] private Transform graphic;
    [SerializeField] private Transform disappearPrefab, waterSplashPrefab;
    [SerializeField] private DebrisTrail debrisTrailPrefab;
    [SerializeField] private List<Sprite> debrisSpriteList;
    [SerializeField] private SpriteRenderer rendered;
    [SerializeField] private float minRotateSpeed = -10, maxRotateSpeed = 10, spawnTrailDeltaTime;
    private float rotateSpeed;
    private Coroutine spawnTrailCor;

    [Button]
    public void Setup()
    {
        rendered.sprite = debrisSpriteList[Random.Range(0, debrisSpriteList.Count)];
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
        JumpRandom();
        spawnTrailCor = StartCoroutine(CorSpawnTrail());
    }
    private void Update()
    {
        rendered.transform.Rotate(new Vector3(0, 0, rotateSpeed));
    }
    private void JumpToPos(Vector2 pos, float jumpPower = 1f, float jumpTime = 1f, bool isLie = false)
    {

        transform.DOMove(pos, jumpTime).SetEase(Ease.InOutSine);
        graphic.transform.DOLocalJump(Vector2.zero, jumpPower, 1, jumpTime).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            CheckCollider();
        });
    }
    public void JumpRandom()
    {
        float xDistance = Random.Range(-5, 5);
        float yDistance = Random.Range(-5, 5);
        float jumpPower = Random.Range(1, 5);
        float jumpTime = Mathf.Clamp(jumpPower / 1.25f, 0, 1.5f);
        Vector2 randomPos = transform.position + new Vector3(xDistance, yDistance);

        JumpToPos(randomPos, jumpPower, jumpTime, true);
    }
    private void CheckCollider()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        if (colls != null)
        {
            foreach (var coll in colls)
            {
                if (coll.CompareTag("MainIce") || coll.CompareTag("Land"))
                {
                    GameObject disappear = Instantiate(disappearPrefab, transform.position, Quaternion.identity).gameObject;
                    SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_DISAPPEAR);
                    Destroy(disappear, 1);
                    StopCoroutine(spawnTrailCor);
                    Destroy(gameObject);
                    return;
                }
            }
        }

        GameObject waterSplash = Instantiate(waterSplashPrefab, transform.position, Quaternion.identity).gameObject;
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_WATER_SPLASH);
        Destroy(waterSplash, 3);
        StopCoroutine(spawnTrailCor);
        Destroy(gameObject);
    }
    IEnumerator CorSpawnTrail()
    {
        yield return new WaitForSeconds(spawnTrailDeltaTime);
        DebrisTrail debrisTrail = Instantiate(debrisTrailPrefab, rendered.transform.position, Quaternion.identity);
        debrisTrail.Setup(rendered);
        spawnTrailCor = StartCoroutine(CorSpawnTrail());
    }
}
