using GDC.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFish : MonoBehaviour
{
    public void Setup(FishData fishData)
    {
        transform.position = fishData.spawnPos;
        Destroy(gameObject, 5);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MainIce"))
        {
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_HIT);
            GameplayManager.Instance.IncreaseHp(-1);
        }
        GameplayManager.Instance.mainIce.ToDefaultSize(true);
    }
}
