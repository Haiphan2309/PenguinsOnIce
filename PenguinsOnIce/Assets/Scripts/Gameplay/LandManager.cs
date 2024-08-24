using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour
{
    public static LandManager Instance { get; private set; }

    public List<Land> landList;
    [SerializeField] private Penguin penguinPrefab, goldenPenguinPrefab;
    private Coroutine spawnCor;
    [SerializeField] private float minSpawnDeltaTime, maxSpawnDeltaTime;
    [SerializeField] private MainIce mainIce;
    public List<Penguin> penguinList;
    private void Awake()
    {
        Instance = this;
    }

    public void Setup()
    {
        spawnCor = StartCoroutine(CorSpawnPenguin());
    }
    public void SetStateStat(Vector2 spawnDeltaTime)
    {
        minSpawnDeltaTime = spawnDeltaTime.x;
        maxSpawnDeltaTime = spawnDeltaTime.y;
    }
    private void SpawnPenguin()
    {
        int rand1 = UnityEngine.Random.Range(0, landList.Count);
        int rand2;
        do
        {
            rand2 = UnityEngine.Random.Range(0, landList.Count);
        }
        while (rand2 == rand1);
        Penguin penguin;
        if (Random.Range(0, 90) % 9 == 0)
        {
            penguin = Instantiate(goldenPenguinPrefab);
        }
        else
        {
            penguin = Instantiate(penguinPrefab);
        }
        penguin.Setup(rand1, rand2);
        landList[rand1].AddPenguin();
        penguinList.Add(penguin);
    }
    IEnumerator CorSpawnPenguin()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDeltaTime,maxSpawnDeltaTime));
        SpawnPenguin();
        spawnCor = StartCoroutine(CorSpawnPenguin());
    }
    public bool CheckCanMove(int landIndex)
    {
        return mainIce.CheckCanMove(landList[landIndex]);
    }
    public void AllPenguinOnIceJump()
    {
        foreach(var penguin in penguinList)
        {
            if (penguin.isOnMainIce)
            {
                penguin.JumpRandom();
            }
        }
    }
    public void RemovePenguin(Penguin penguin)
    {
        penguinList.Remove(penguin);
    }
}
