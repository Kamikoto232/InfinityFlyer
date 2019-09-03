using UnityEngine;
[CreateAssetMenu]
public class StageInfo : ScriptableObject
{
    [ColorUsage(false, false)]
    public Color color;
    public float TrashSpawnWait, MapSpawnDistance;
    public GameObject[] Trash;
    public GameObject[] Maps;

    public float[] MapSpawnDistances;
    public AudioClip StageMusicBG;
    public ShipData RecommendedShip;
    public int MinPlayerLevel, MinScore;
    public StageData stageData;
}

[System.Serializable]
public struct StageData
{
    public bool Passed, Open;
    public int Score;
}