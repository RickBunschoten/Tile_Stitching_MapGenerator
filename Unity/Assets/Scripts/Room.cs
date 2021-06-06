using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //[SerializeField]
    //private Vector4 SideLengths;
    [SerializeField]
    private Vector3 maxSize = Vector3.one;

    [SerializeField]
    private List<ConnectionPoints> connections;

    public Vector3 MaxSize
    {
        get { return maxSize; }
    }
    public List<ConnectionPoints> ConnectionPoints
    {
        get { return connections; }
    }
}
