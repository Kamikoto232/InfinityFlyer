using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager sm;

    public Transform minX, maxY, sTr;
    public GameObject[] Map;
    public GameObject[] Trash;
    public GameObject[] Boosters;
    public GameObject FinishObject;
    public Transform LastSpawnPoint;
    float MapDist = 40;
    Coroutine mapspawn;
    int PosCounter;
    Vector3 OldPos;
    public float TriggerShipOffset;

    void Awake()
    {
        sm = this;
    }

    private void Start()
    {
        EventManager.StopGameHandler += StopInfinitySpawnMap;
    }

    void StopInfinitySpawnMap()
    {
        CancelInvoke("SpawnTrash");
        if (sm.mapspawn != null)
            sm.StopCoroutine(sm.mapspawn);
    }

    public static void StartInfinitySpawnMap()
    {
        sm.mapspawn = sm.StartCoroutine(sm.InfinitySpawnMap());
        sm.InvokeRepeating("SpawnTrash", 0.01f, 1);
    }

    void SpawnMap()
    {
        if (Map.Length > 0)
            Instantiate(Map[Random.Range(0, Map.Length)], sTr.position, Quaternion.identity);
    }

    void SpawnMap(GameObject map)
    {
        Instantiate(map, sTr.position, Quaternion.identity);
        LastSpawnPoint.position = sTr.position;
    }

    void SpawnTrash()
    {
        OldPos = PlayerManager.pm.ModelTransform.position;
        OldPos.z = PlayerManager.pm.ModelTransform.position.z;
        if (Vector3.Distance(OldPos, PlayerManager.pm.ModelTransform.position) < TriggerShipOffset)
        {
            PosCounter++;
        }
        else PosCounter = 0;

        Vector3 randomPos;
        int offset = 5;
        if (PosCounter > 3) offset = 2;

        randomPos.x = Mathf.Clamp(PlayerManager.pm.ModelTransform.GetChild(0).GetChild(1).position.x + Random.Range(-offset, offset), minX.position.x, maxY.position.x);
        randomPos.y = Mathf.Clamp(PlayerManager.pm.ModelTransform.GetChild(0).GetChild(1).position.y + Random.Range(-offset, offset), minX.position.y, maxY.position.y);
        randomPos.z = minX.position.z;

        if (Random.Range(0,10) < 8)
        {
            if (Trash.Length > 0)
                Instantiate(Trash[Random.Range(0, Trash.Length)], randomPos, Quaternion.identity);
        }
        else SpawnBooster();
        SpawnAmbientTrash();
        //SpawnAmbientTrash();
    }

    void SpawnAmbientTrash()
    {
        Vector3 randomPos;
        int offset = 10;

        randomPos.x = Mathf.Clamp(PlayerManager.pm.ModelTransform.GetChild(0).GetChild(1).position.x + Random.Range(-offset, offset), minX.position.x, maxY.position.x);
        randomPos.y = Mathf.Clamp(PlayerManager.pm.ModelTransform.GetChild(0).GetChild(1).position.y + Random.Range(-offset, offset), minX.position.y, maxY.position.y);
        randomPos.z = minX.position.z;

        if (Random.Range(0, 10) < 8)
        {
            if (Trash.Length > 0)
                Instantiate(Trash[Random.Range(0, Trash.Length)], randomPos, Quaternion.identity);
        }
        else SpawnBooster();

    }

    void SpawnBooster()
    {
        Instantiate(Boosters[Random.Range(0, Boosters.Length)],
                   new Vector3(Random.Range(minX.position.x, maxY.position.x), Random.Range(minX.position.y, maxY.position.y), Random.Range(minX.position.z, maxY.position.z)),
                   Quaternion.identity);
    }

    void SpawnHealth()
    {

    }

    IEnumerator InfinitySpawnMap()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.2f);
        while (true)
        {
            if (Vector3.Distance(LastSpawnPoint.position, sTr.position) > MapDist)
            {
                SpawnMap();
                LastSpawnPoint.position = sTr.position;
            }
            yield return wait;
        }
    }

    public static void StartStageSpawnMap(StageInfo stage)
    {
        sm.StartCoroutine(sm.StageSpawnMap(stage));
    }

    IEnumerator StageSpawnMap(StageInfo s)
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.2f);
        for (int i = 0; i < s.Maps.Length; i++)
        {
            while (!CheckDistance(s, i))
            {
                yield return wait;
            }

            SpawnMap(s.Maps[i]);
        }
        SpawnFinish();
    }

    bool CheckDistance(StageInfo s, int index)
    {
        return Vector3.Distance(LastSpawnPoint.position, sTr.position) > s.MapSpawnDistances[index];
    }

    void SpawnFinish()
    {
        SpawnMap(FinishObject);
    }

    public static void Finish()
    {
        //ScoreManager.sm.StagePassed();
    }
}