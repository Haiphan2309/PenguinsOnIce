using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SpriteRenderedDepth
{
    [HideInInspector] public int baseDepth;
    public SpriteRenderer sprRen;
    public SpriteRenderedDepth(SpriteRenderer sprRen)
    {
        this.baseDepth = sprRen.sortingOrder;
        this.sprRen = sprRen;
    }
}
public class Depth : MonoBehaviour
{
    public List<SpriteRenderedDepth> sprRenDepths;
    [SerializeField] private int baseDepth;
    private void Start()
    {
        foreach (var sprRenDepth in sprRenDepths)
        {
            sprRenDepth.baseDepth = sprRenDepth.sprRen.sortingOrder;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        foreach (var sprRenDepth in sprRenDepths)
        {
            sprRenDepth.sprRen.sortingOrder = baseDepth * 100 - (int)(transform.position.y * 10) + sprRenDepth.baseDepth;
        }
    }
    [Button]
    private void LoadSpriteRenderedList()
    {
        if (sprRenDepths == null)
        {
            sprRenDepths = new List<SpriteRenderedDepth>();
        }
        sprRenDepths.Clear();
        SpriteRenderer rendered = transform.GetComponent<SpriteRenderer>();
        if (rendered != null)
        {
            sprRenDepths.Add(new SpriteRenderedDepth(rendered));
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            SpriteRenderer childRendered = transform.GetChild(i).GetComponent<SpriteRenderer>();
            if (childRendered != null)
            {
                sprRenDepths.Add(new SpriteRenderedDepth(childRendered));
            }
        }
    }
}
