using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Strategy_Map : MonoBehaviour
{
    [SerializeField]
    private GameObject _templateObject;
    [SerializeField]
    private List<Strategy_Cube> _gridObjects;

    [SerializeField]
    private int _width = 100;
    [SerializeField]
    private int _depth = 100;
    [SerializeField]
    private int _height = 10;

    public void Generate()
    {

    }

    private void instantiateField()
    {
        int index = 0;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _depth; y++)
            {
                for (int z = 0; z < _height; z++)
                {
                    var obj = Instantiate(_templateObject);
                    SetBaseObjValues(x, y, z, index);
                    index++;
                }
            }
        }
    }

    private void SetBaseObjValues(int x, int y, int z, int index)
    {

    }
}

