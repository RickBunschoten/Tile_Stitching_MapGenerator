using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPoints : MonoBehaviour
{
    [SerializeField]
    private bool connected;

    [SerializeField]
    private Room originalRoom;

    [SerializeField]
    private Room connectedRoom;

    public bool Connected
    {
        get
        {
            return connected;
        }
    }

    public Room OriginalRoom
    {
        get { return originalRoom; }
    }
    public Room ConnectedRoom
    {
        get
        {
            return connectedRoom;
        }
        set
        {
            if (connectedRoom != value)
            {
                connectedRoom = value;
                if (connectedRoom != null)
                {
                    connected = true;
                }
            }
        }
    }
}
