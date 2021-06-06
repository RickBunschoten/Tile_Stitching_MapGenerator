using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridData
{
    public int ID;
    public Vector3 Position = Vector3.zero;
    public bool Used = false;

    public override string ToString()
    {
        return "ID: " + ID + " |Pos: " + Position + " |Used: " + Used;
    }
}


public class GridManager : MonoBehaviour
{
    private static GridManager instance;
    public static GridManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject GO = new GameObject();
                GO.AddComponent<GridManager>();

                var GOi = Instantiate(GO);
                instance = GOi.GetComponent<GridManager>();
            }
            return instance;
        }
    }

    private Vector3 GridSize = Vector3.one * 100;
    //[SerializeField]
    private List<GridData> Grid = new List<GridData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int z = 0; z < GridSize.z; z++)
                {
                    GridData G = new GridData();
                    G.ID = int.Parse("" + x + y + z);
                    G.Position = new Vector3(x, y, z) - (GridSize / 2);
                    G.Used = false;
                    Grid.Add(G);

                    //if (x == 0 && y == 0 && z == 0)
                    //{
                    //    Debug.Log(G.Position);
                    //}
                    //if (x == (GridSize.x-1) && y == (GridSize.y-1) && z == (GridSize.z-1))
                    //{
                    //    Debug.Log(G.Position);
                    //}
                }
            }
        }
    }

    public bool IsGridFull(Vector3 pos)
    {
        string debugString = "Set Grid Pos Full: " + pos + "\n\n";

        var xBV = BetweenVals(pos.x);
        var yBV = BetweenVals(pos.y);
        var zBV = BetweenVals(pos.z);


        debugString += "x between values: " + xBV + "\n" +
        "y between values: " + yBV + "\n" +
        "z between values: " + zBV + "\n\n";


        var subGrid = Grid.Where(cell => cell.Used == true).ToList();

        foreach (var cell in subGrid)
        {
            //Debug.Log("x between values: " + (cell.Position.x >= xBV.x) + ", " + (cell.Position.x < xBV.y) + "\n" +
            //"y between values: " + (cell.Position.y >= yBV.x) + ", " + (cell.Position.y < yBV.y) + "\n" +
            //"z between values: " + (cell.Position.z >= zBV.x) + ", " + (cell.Position.z < zBV.y));

            if (cell.Position.x >= xBV.x && cell.Position.x < xBV.y &&
                cell.Position.y >= yBV.x && cell.Position.y < yBV.y &&
              cell.Position.z >= zBV.x && cell.Position.z < zBV.y)
            {
                debugString += "Found a cell: " + cell;
                Debug.Log(debugString);
                return true;
            }
        }
        Debug.Log(debugString);

        return false;
    }

    public bool SetGridPosFull(Vector3 pos)
    {
        //if the position is already filled return false since we can't complete the opperation
        if (IsGridFull(pos))
        {
            Debug.Log("Grid at position: " + pos + "was full");
            return false;
        }
        else
        {
            string debugString = "Set Grid Pos Full: " + pos + "\n\n";

            //Get the betweenVals for our current Position
            var xBV = BetweenVals(pos.x);
            var yBV = BetweenVals(pos.y);
            var zBV = BetweenVals(pos.z);

            debugString += "x between values: " + xBV + "\n" +
            "y between values: " + yBV + "\n" +
            "z between values: " + zBV + "\n\n";

            //Get only the sells that are unused
            var subGrid = Grid.Where(cell => cell.Used == false).ToList();

            foreach (var cell in subGrid)
            {
                //Check if any of the unused cells is between our position (or rather our position is inside the box, since this is a crude detection)
                if (cell.Position.x >= xBV.x && cell.Position.x < xBV.y &&
                    cell.Position.y >= yBV.x && cell.Position.y < yBV.y &&
                    cell.Position.z >= zBV.x && cell.Position.z < zBV.y)
                {
                    debugString += "Found a unused cell: " + cell;

                    Debug.Log(debugString);
                    cell.Used = true;
                    return true;
                }else
                {
                    //debugString += "Cell with pos " + cell.Position + " didn't fit the conditions\n";
                }
            }
            debugString += "None of the unused cells can contain our position";
            Debug.LogWarning(debugString);
            //None of the unused cells can contain our position
            return false;
        }
    }
    private bool GreaterThenZero(int val)
    {
        if (val >= 0) return true;
        else return false;
    }
    private Vector2 BetweenVals(float refVal)
    {
        int floor = Mathf.FloorToInt(refVal);
        int ceil = Mathf.CeilToInt(refVal);

        if (ceil == floor) ceil += 1;

        return new Vector2(floor, ceil);
    }
}
