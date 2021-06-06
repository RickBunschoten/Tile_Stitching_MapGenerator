using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public enum MoodTypes
{
    DEV = 0,

}

public class RoomsManager : MonoBehaviour
{
    private static RoomsManager instance;
    public static RoomsManager Instance
    {
        get 
        {
            if (instance == null)
            {
                //Debug.Log("Instance GET");
                GameObject GO = new GameObject();
                GO.AddComponent<RoomsManager>();

                var GOi = Instantiate(GO);
                instance = GOi.GetComponent<RoomsManager>();
            }
                return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            //Debug.Log("Setting instance: " + this.gameObject.name);
            instance = this;
        }
        if (instance != this)
        {
            //Debug.Log("Destroying: " + this.gameObject.name + "\n - instance on: " + instance.gameObject.name);
            Destroy(this);
        }
    }


    [SerializeField]
    private MoodTypes activeMood;

    [SerializeField]
    private List<RoomCollection> collections;

    public RoomCollection CurrentRoomCollection
    {
        get
        {
            return collections.Where(x => x.Mood == activeMood).FirstOrDefault();
        }
    }

    //[SerializeField]
    //private List<string> corridors;
    //[SerializeField]
    //private List<string> chambers;
    //[SerializeField]
    //private List<GameObject> chambersOBJ;
    //private const string resourcesFindPath = "Assets/Resources/Prefabs/";
    //private const string resourcesLoadPath = "/Prefabs/";

    //// Start is called before the first frame update
    //void Start()
    //{
    //    OnLoadPrefabs(resourcesFindPath + mood.ToString());
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //private void OnLoadPrefabs(string path)
    //{
    //    if (Directory.Exists(path))
    //    {
    //        OnLoadCorridors(path + "/Corridor/");
    //        OnLoadChambers(path + "/Chambers/");
    //    }
    //    else
    //    {
    //        Debug.LogError(path + " - did not exist");
    //    }

    //    //foreach (FileInfo f in info)
    //    //{
    //    //    if (f.Extension == ".prefab")
    //    //    {
    //    //        string tempName = f.Name;
    //    //        string extension = f.Extension;
    //    //        string strippedName = tempName.Replace(extension, "");
    //    //        PlayersInFolder.Add(strippedName);
    //    //    }
    //    //}
    //}

    //private void OnLoadCorridors(string path)
    //{
    //    if (Directory.Exists(path))
    //    {
    //        DirectoryInfo dir = new DirectoryInfo(path);
    //        FileInfo[] info = dir.GetFiles("*.*");
    //        foreach (FileInfo f in info)
    //        {
    //            if (f.Extension == ".prefab")
    //            {
    //                string p = resourcesLoadPath + mood.ToString() + "/Corridor/" + f.Name;
    //                corridors.Add(p);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError(path + " - did not exist");
    //    }
    //}
    //private void OnLoadChambers(string path)
    //{
    //    if (Directory.Exists(path))
    //    {
    //        DirectoryInfo dir = new DirectoryInfo(path);
    //        FileInfo[] info = dir.GetFiles("*.*");
    //        foreach (FileInfo f in info)
    //        {
    //            if (f.Extension == ".prefab")
    //            {
    //                string p = resourcesLoadPath + mood.ToString() + "/Chambers/" + f.Name;
    //                chambers.Add(p);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError(path + " - did not exist");
    //    }
    //}
}
