using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Strategy_Cube : MonoBehaviour
{
    #region variables
    [SerializeField]
    private Vector3 _gridPos;
    [SerializeField]
    private string uid;

    #endregion

    #region Get/Set
    //A one time set UID
    public string UID
    {
        get { return uid; }
        set
        {
            if (uid == "")
            {
                uid = value;
            }
        }
    }
    public Vector3 GridPos
    {
        get { return _gridPos; }
    }

    #endregion

    public override string ToString()
    {
        Debug.Log(base.ToString());

        var retval = "";
        retval += UID + "| ";
        retval += " (" + _gridPos.x + ", " + _gridPos.y + ", " + _gridPos.z + ") - " + this.name;


        return retval;
    }
}

