using GDC.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class FloatIce : MonoBehaviour
{
    [SerializeField] private Transform graphic;
    [SerializeField] private Transform waterTrail;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int debrisNum;
    [SerializeField] private Debris debrisPrefab;
    [SerializeField] private Vector2 spawnDebrishRange;
    [SerializeField] private GameObject vfxIceBroken;
    private GDC.Enums.Direct direct;
    private Vector2 dir;
    bool isAlreadyBroken;

    public void Setup(IceData iceData)
    {
        transform.position = iceData.spawnPos;
        int scaleFlip = Random.Range(0, 100)%2;
        if (scaleFlip == 0) scaleFlip = -1;
        graphic.localScale = new Vector2(scaleFlip,1);
        this.direct = iceData.direct;
        switch (direct)
        {
            case GDC.Enums.Direct.UP:
                dir = Vector2.up;
                break;
            case GDC.Enums.Direct.DOWN:
                dir = Vector2.down;
                break;
            case GDC.Enums.Direct.LEFT:
                dir = Vector2.left; 
                break;
            case GDC.Enums.Direct.RIGHT:
                dir = Vector2.right; 
                break;
        }
        isAlreadyBroken = false;
    }
    private void Update()
    {
        Move();
        if (transform.position.x > 20 || transform.position.y > 15 || transform.position.x < -20 || transform.position.y < -15)
        {
            waterTrail.SetParent(null);
            Destroy(gameObject);
        }
    }
    private void Move()
    {
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
    private void BreakIce()
    {
        if (isAlreadyBroken) return;
        isAlreadyBroken = true;
        waterTrail.SetParent(null);
        vfxIceBroken.SetActive(true);
        vfxIceBroken.transform.SetParent(null);
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_HIT);
        //Debug.Log(debrisNum);
        for (int i = 0; i < debrisNum; i++)
        {
            Vector3 spawnRange = new Vector3(Random.Range(-spawnDebrishRange.x, spawnDebrishRange.x), Random.Range(-spawnDebrishRange.y, spawnDebrishRange.y));
            Debris debris = Instantiate(debrisPrefab, transform.position + spawnRange, Quaternion.identity);
            debris.Setup();
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MainIce"))
        {
            GameplayManager.Instance.IncreaseHp(-1);
        }
        BreakIce();
    }
}
