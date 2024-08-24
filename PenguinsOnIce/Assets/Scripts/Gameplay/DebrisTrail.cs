using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisTrail : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ren;
    public void Setup(SpriteRenderer ren)
    {
        this.ren.sprite = ren.sprite;
        this.ren.transform.rotation = ren.transform.rotation;
        this.ren.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() => Destroy(gameObject));
    }
}
