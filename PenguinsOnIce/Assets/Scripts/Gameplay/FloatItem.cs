using GDC.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class FloatItem : MonoBehaviour
{
    [SerializeField] private Transform graphic;
    [SerializeField] private Transform waterTrail;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject vfxLoot;
    [SerializeField] private bool isHeart; //If not heart, it's time
    [SerializeField] private int increaseValue;
    [SerializeField] private GameObject pointObj;
    private GDC.Enums.Direct direct;
    private Vector2 dir;
    bool isAlreadyBroken;

    public void Setup(IceData iceData)
    {
        transform.position = iceData.spawnPos;
        int scaleFlip = Random.Range(0, 100) % 2;
        if (scaleFlip == 0) scaleFlip = -1;
        graphic.localScale = new Vector2(scaleFlip, 1);
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
    private void Loot()
    {
        Debug.Log(gameObject.name + " LOOT");
        GameObject pointObject = Instantiate(pointObj, transform.position, Quaternion.identity);
        Destroy(pointObject, 1.5f);
        if (isHeart)
        {
            GameplayManager.Instance.IncreaseHp(increaseValue);
        }
        else
        {
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_STAR);
            GameplayManager.Instance.IncreaseTime(increaseValue);
        }
    }
    private void Broken()
    {
        if (isAlreadyBroken) return;
        isAlreadyBroken = true;
        waterTrail.SetParent(null);
        vfxLoot.SetActive(true);
        vfxLoot.transform.SetParent(null);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MainIce"))
        {
            Loot();
        }
        Broken();
    }
}
