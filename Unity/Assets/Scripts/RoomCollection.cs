using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomCollection
{
    [SerializeField]
    private string Name;

    [SerializeField]
    private MoodTypes mood;

    [SerializeField]
    private List<GameObject> startChamberPrefabs;

    [SerializeField]
    private List<GameObject> corridorPrefabs;
    [SerializeField]
    private List<int> corridorPrefabChances;
    [SerializeField]
    private List<GameObject> chamberPrefabs;
    [SerializeField]
    private List<int> chamberPrefabsChances;


    [SerializeField]
    private List<GameObject> endChamberPrefabs;

    public List<GameObject> StartChamberPrefabs
    {
        get
        {
            return startChamberPrefabs;
        }
    }
    public List<GameObject> CorridorPrefabs
    {
        get
        {
            return corridorPrefabs;
        }
    }
    public List<int> CorridorPrefabsChances
    {
        get
        {
            return corridorPrefabChances;
        }
    }
    public List<GameObject> ChamberPrefabs
    {
        get
        {
            return chamberPrefabs;
        }
    }
    public List<int> ChamberPrefabsChances
    {
        get
        {
            return chamberPrefabsChances;
        }
    }
    public List<GameObject> EndChamberPrefabs
    {
        get
        {
            return endChamberPrefabs;
        }
    }

    public MoodTypes Mood
    {
        get { return mood; }
    }

}
