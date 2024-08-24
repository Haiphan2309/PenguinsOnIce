using GDC.Enums;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public Vector2 spawnPos; //vi tri spawn
    public Vector2 pos; //vi tri chuan bi nhay xuong
    public Vector2 towardPos; //vi tri nhay xuong
    public int index;
    public LandDirect landDirect;
    [ReadOnly] public Color color;
    [SerializeField, ReadOnly] private int penguinNum = 0;

    public void AddPenguin()
    {
        penguinNum++;
    }
    public void RemovePenguin()
    {
        penguinNum--;
    }

    //Editor only
    [Button]
    private void SetFlagColor()
    {
        color = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
    }
}
