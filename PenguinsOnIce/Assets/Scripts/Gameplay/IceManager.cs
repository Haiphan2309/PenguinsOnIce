using GDC.Enums;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class IceData
{
    public Vector2 spawnPos;
    public Direct direct;
    public Vector2 warningPos;
}
[Serializable]
public class FishData
{
    public Vector2 spawnPos;
}
[Serializable]
public class StateStat
{
    public int totalTime;
    public Vector2 spawnIceDeltaTime, spawnFishDeltaTime, spawnHeartDeltaTime, spawnClockDeltaTime, spawnPenguinDeltaTime; //x: min, y: max
}
public class IceManager : MonoBehaviour
{
    [Header("Float Ice")]
    [SerializeField] private List<FloatIce> floatIcePrefabList;
    [SerializeField] private List<IceData> iceDataList;
    [SerializeField, ReadOnly] private float minSpawnIceDeltaTime, maxSpawnIceDeltaTime;

    [Header("Big Fish")]
    [SerializeField] private BigFish bigFishPrefab;
    [SerializeField] private List<FishData> fishDataList;
    [SerializeField, ReadOnly] private float minSpawnFishDeltaTime, maxSpawnFishDeltaTime;

    [Header("Heart Item")]
    [SerializeField] private FloatItem heartItemPrefab;
    [SerializeField, ReadOnly] private float minSpawnHeartDeltaTime, maxSpawnHeartDeltaTime;

    [Header("Clock Item")]
    [SerializeField] private FloatItem clockItemPrefab;
    [SerializeField, ReadOnly] private float minSpawnClockDeltaTime, maxSpawnClockDeltaTime;
    
    [SerializeField] private Transform warningIceSign, warningFishSign;
    private Coroutine spawnIceCor, spawnFishCor, spawnHeartCor, spawnClockCor;

    [Header("Stat")]
    [SerializeField] private List<StateStat> stateStatList;
    private bool isAlreadyIce, isAlreadyFish, isAlreadyHeart, isAlreadyClock;

    public void Setup()
    {
        isAlreadyIce = false;
        isAlreadyFish = false;
        isAlreadyHeart = false;
        isAlreadyClock = false;
        SetStateStat(0);
    }

    public void SetStateStat(int totalTime)
    {
        foreach (var stat in stateStatList)
        {
            if (totalTime == stat.totalTime)
            {
                minSpawnIceDeltaTime = stat.spawnIceDeltaTime.x;
                maxSpawnIceDeltaTime = stat.spawnIceDeltaTime.y;
                minSpawnFishDeltaTime = stat.spawnFishDeltaTime.x;
                maxSpawnFishDeltaTime = stat.spawnFishDeltaTime.y;
                minSpawnHeartDeltaTime = stat.spawnHeartDeltaTime.x;
                maxSpawnHeartDeltaTime = stat.spawnHeartDeltaTime.y;
                minSpawnClockDeltaTime = stat.spawnClockDeltaTime.x;
                maxSpawnClockDeltaTime = stat.spawnClockDeltaTime.y;
                GameplayManager.Instance.landManager.SetStateStat(stat.spawnPenguinDeltaTime);

                if (maxSpawnIceDeltaTime > 0 && isAlreadyIce == false)
                {
                    isAlreadyIce = true;
                    StartSpawnIce();
                }
                if (maxSpawnFishDeltaTime > 0 && isAlreadyFish == false)
                {
                    isAlreadyFish = true;
                    StartSpawnFish();
                }
                if (maxSpawnHeartDeltaTime > 0 && isAlreadyHeart == false)
                {
                    isAlreadyHeart = true;
                    StartSpawnHeart();
                }
                if (maxSpawnClockDeltaTime > 0 && isAlreadyClock == false)
                {
                    isAlreadyClock = true;
                    StartSpawnClock();
                }
                return;
            }
        }

    }
    public void StartSpawnIce()
    {
        spawnIceCor = StartCoroutine(CorSpawnIce());
    }
    private void SpawnIce()
    {
        int randPrefab = UnityEngine.Random.Range(0, floatIcePrefabList.Count);
        int randIceData = UnityEngine.Random.Range(0, iceDataList.Count);
        FloatIce ice = Instantiate(floatIcePrefabList[randPrefab]);
        ice.Setup(iceDataList[randIceData]);
        GameObject wanringSign = Instantiate(warningIceSign, iceDataList[randIceData].warningPos, Quaternion.identity).gameObject;
        Destroy(wanringSign, 2);
    }
    IEnumerator CorSpawnIce()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnIceDeltaTime, maxSpawnIceDeltaTime));
        SpawnIce();
        spawnIceCor = StartCoroutine(CorSpawnIce());
    }

    public void StartSpawnFish()
    {
        spawnFishCor = StartCoroutine(CorSpawnFish());
    }
    private void SpawnFish()
    {
        int randFishData = UnityEngine.Random.Range(0, fishDataList.Count);
        BigFish fish = Instantiate(bigFishPrefab);
        fish.Setup(fishDataList[randFishData]);
        GameObject wanringSign = Instantiate(warningFishSign, fishDataList[randFishData].spawnPos, Quaternion.identity).gameObject;
        Destroy(wanringSign, 2);
    }
    IEnumerator CorSpawnFish()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnFishDeltaTime, maxSpawnFishDeltaTime));
        SpawnFish();
        spawnFishCor = StartCoroutine(CorSpawnFish());
    }

    public void StartSpawnHeart()
    {
        spawnHeartCor = StartCoroutine(CorSpawnHeart());
    }
    private void SpawnHeart()
    {
        int randIceData = UnityEngine.Random.Range(0, iceDataList.Count);
        FloatItem heartItem = Instantiate(heartItemPrefab);
        heartItem.Setup(iceDataList[randIceData]);
    }
    IEnumerator CorSpawnHeart()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnHeartDeltaTime, maxSpawnHeartDeltaTime));
        SpawnHeart();
        spawnHeartCor = StartCoroutine(CorSpawnHeart());
    }

    public void StartSpawnClock()
    {
        spawnHeartCor = StartCoroutine(CorSpawnClock());
    }
    private void SpawnClock()
    {
        int randIceData = UnityEngine.Random.Range(0, iceDataList.Count);
        FloatItem clockItem = Instantiate(clockItemPrefab);
        clockItem.Setup(iceDataList[randIceData]);
    }
    IEnumerator CorSpawnClock()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnClockDeltaTime, maxSpawnClockDeltaTime));
        SpawnClock();
        spawnClockCor = StartCoroutine(CorSpawnClock());
    }
}
