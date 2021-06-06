using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField]
    private GameObject Parent;

    public bool generate = false;
    public bool generateDelayBetweeenRecursion = false;

    public List<GameObject> rooms;

    public Vector3 StartGenerationPosition;

    public int MinChamberCount = 5;
    public int MinCorridorLength = 5;
    public int MaxCorridorLength = 5;

    public int StepDistance = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
        {
            generate = false;
            StartGenerate();
        }
    }

    private void StartGenerate()
    {
        var GOref = Instantiate(GetStartRoom());
        GOref.transform.position = StartGenerationPosition;
        GOref.transform.SetParent(Parent.transform);
        GOref.name = "Room" + count++;

        rooms.Add(GOref);

        float start = Time.realtimeSinceStartup;
        Generate(GOref.GetComponent<Room>());
        float end = Time.realtimeSinceStartup;

        Debug.LogWarning("start: " + start + " - end: " + end);
    }
    private int count = 0;
    private async void Generate(Room room, int corridorLength = 0)
    {
        //Debug.Log(corridorLength);
        if (generateDelayBetweeenRecursion) await Task.Delay(1000);

        foreach (var cPoint in room.ConnectionPoints)
        {
            if (cPoint.Connected == false)
            {
                if (corridorLength < MinCorridorLength)
                {
                    //I have to spawn a corridor to get to the minimum length


                    /*
                    PSEUDO CODE

                    - Spawn a new random corridor
                    - Get it's position and get from the corridor class it's dimensions matrix (MaxSize)
                    - Based on the above values calculate the boundaries of the candidateRoom

                    - Loop trough all current rooms
                    - Assume that the ones in the list are final
                    - Get it's position and get from the corridor class it's dimensions matrix (MaxSize)
                    - Based on the above values calculate the boundaries of the CheckingRoom

                    - If any position overlaps with the Checkingroom
                    - either reshoufle a new random corridor a few times (Like 5 times)
                    - if no corridor fits in 5 tries clear my current room since it leads to a problematic situation
                    */

                    //Debug string
                    string debugGen = "";


                    //Randomize Seed
                    Random.InitState(System.DateTime.Now.Millisecond);
                    //Get random value
                    var r = Random.Range(1, 101);
                    //Use random value to get a corridor Piece and Instantiate the object, saving a local reference to said object
                    var CandidateObject = Instantiate(GetCorridorPart(r));
                    Room cRoom = CandidateObject.GetComponent<Room>();


                    //Set generic data such as position
                    SetGenericGenerateData(CandidateObject, cPoint, ref debugGen);
                    debugGen += "\n";
                    var anglesUsed = new List<Quaternion>();
                    anglesUsed.Add(CandidateObject.transform.rotation);
                    debugGen += "\n\n";

                    //change rotation to match up with the cirrent checking cPoint.
                    //TODO: not only change rotation, but also move the room if it's not alligned
                    //TODO EXTRA: Cycle trough all possible rooms befor simply discarding this place as not an option
                    var correctCPoint = RotateRoomToAllignConPoints(CandidateObject, cPoint, anglesUsed, ref debugGen);
                    debugGen += "\n";
                    if (correctCPoint != null)
                    {
                        debugGen += "Succesfully Rotated the room to allign the connectionPoints";
                        cPoint.ConnectedRoom = cRoom;
                        correctCPoint.ConnectedRoom = room;
                    }
                    else
                    {
                        debugGen += "Unsuccesfully Rotated the room to allign the connectionPoints";
                        Debug.Log(debugGen);
                        Destroy(CandidateObject);
                        return;
                    }

                    //Checkl whether the now correctly places, rotated and again placed room is free of any other rooms
                    //such that there's no "overlap" or "Colission" with ANY other room
                    //So we do a check for the CandidateRoom against all other Currently Added rooms to see if there's ANY overlap
                    //If so we discard this CandidateRoom as an option

                    bool passed = true;
                    foreach (var checkRoom in rooms)
                    {
                        if (CheckOverlap(cRoom, checkRoom.GetComponent<Room>(), ref debugGen)) passed = false;
                    }
                    debugGen += "\n PASSED: " + passed + "\n\n";
                    if (passed == false)
                    { 
                        Debug.Log(debugGen);
                        Destroy(CandidateObject);
                        return;
                    }



                    //CandidateObject.GetComponent<Room>().ConnectionPoints.ForEach(p => debugGen += "current pos: " + cPoint.transform.position + " | CRcp: " + p.transform.position + "\n");
                    //CandidateObject.GetComponent<Room>().ConnectionPoints.ForEach(p => debugGen += "current Localpos: " + cPoint.transform.localPosition + " | CRlcp: " + p.transform.localPosition + "\n");
                    //debugGen += "\n";

                    //IMPORTANT: this check can only happen after the position has been set above here for correct placement of the object so that we can check the rotation;
                    //Need to rotate the object, and perhaps move the object slightly to make the connection points allign then check if it's allowed to be there
                    //Set the rotation? more like need to calculate the rotation so that any new connectionpoint matches

                    //CandidateObject.transform.rotation = Quaternion.Euler(fw*90);


                    rooms.Add(CandidateObject);
                    Generate(cRoom, (corridorLength + 1));
                    #region angle test
                    /*
                    float angle;

                    if (fw.z != 0)
                    {
                        angle = rt.x / fw.z;
                    }
                    else
                    {
                        angle = fw.x / rt.z;
                    }
                    angle = comb.x / comb.z;
                    debugGen += "Angle: " + angle + "\n";


                    var possibleRotation = Mathf.Atan(angle);
                    debugGen += "Possible Rotation TAN-1 (RAD): " + possibleRotation + "\n";
                    debugGen += "Possible Rotation TAN-1 (DEG): " + ((possibleRotation*180)/Mathf.PI) + "\n\n";


                    possibleRotation = Mathf.Acos(angle);
                    debugGen += "Possible Rotation COS-1 (RAD): " + possibleRotation + "\n";
                    debugGen += "Possible Rotation COS-1 (DEG): " + ((possibleRotation * 180) / Mathf.PI) + "\n\n";


                    possibleRotation = Mathf.Asin(angle);
                    debugGen += "Possible Rotation SIN-1 (RAD): " + possibleRotation + "\n";
                    debugGen += "Possible Rotation SIN-1 (DEG): " + ((possibleRotation * 180) / Mathf.PI) + "\n\n";

                    possibleRotation = Mathf.Tan(angle);
                    debugGen += "Possible Rotation TAN (RAD): " + possibleRotation + "\n";
                    debugGen += "Possible Rotation TAN (DEG): " + ((possibleRotation * 180) / Mathf.PI) + "\n\n";


                    possibleRotation = Mathf.Cos(angle);
                    debugGen += "Possible Rotation COS (RAD): " + possibleRotation + "\n";
                    debugGen += "Possible Rotation COS (DEG): " + ((possibleRotation * 180) / Mathf.PI) + "\n\n";


                    possibleRotation = Mathf.Sin(angle);
                    debugGen += "Possible Rotation SIN (RAD): " + possibleRotation + "\n";
                    debugGen += "Possible Rotation SIN (DEG): " + ((possibleRotation * 180) / Mathf.PI) + "\n\n";
                    */
                    #endregion

                    Debug.Log(debugGen);
                    /// BACKUP
                    ////if corridor length is still not minimum length
                    ////generate new corridor
                    //var r = Random.Range(1, 101);
                    //var CoObj = GetCorridorPart(r);

                    ////Get correct room offset
                    //Vector3 pos = room.MaxSize + CoObj.GetComponent<Room>().MaxSize;
                    //var fw = cPoint.transform.forward;
                    //pos = new Vector3(pos.x * fw.x, pos.y * fw.y, pos.z * fw.z);

                    ////Set corridor parts
                    //var offset = (CoObj.GetComponent<Room>().MaxSize / 2);
                    //var offsetToDirection = new Vector3(offset.x * fw.x, offset.y * fw.y, offset.z * fw.z);
                    //var newPos = cPoint.transform.position + (offsetToDirection * StepDistance);

                    ////Debug.Log(CoObj);
                    ////Debug.Log(newPos);
                    //if (GridManager.Instance.SetGridPosFull(newPos))
                    //{
                    //    var CoRef = Instantiate(CoObj);


                    //    CoRef.transform.position = newPos;
                    //    CoRef.transform.SetParent(Parent.transform);
                    //    CoRef.name = "Room" + count++;

                    //    //Debug.Log(CoRef);
                    //    //Debug.Log(cPoint.transform.position);
                    //    //CoRef.GetComponent<Room>().ConnectionPoints.ForEach(x => Debug.Log(x + " - " + x.transform.position));

                    //    //Set the connected room of current point to the new room
                    //    cPoint.ConnectedRoom = CoRef.GetComponent<Room>();

                    //    //try set the connectionpoint of the new room to this room
                    //    var temp = CoRef.GetComponent<Room>().ConnectionPoints.Where(x => x.transform.position == cPoint.transform.position).FirstOrDefault();
                    //    if (temp != null)
                    //    {
                    //        temp.ConnectedRoom = room;
                    //    }
                    //    else
                    //    {
                    //        //Debug.LogError("No connectionpoints at the right position, perhaps rotate a tile?");
                    //    }
                    //    rooms.Add(CoRef);
                    //    Generate(CoRef.GetComponent<Room>(), corridorLength + 1);
                    //}
                }
            }
        }
    }
    private void SetGenericGenerateData(GameObject CandidateObject, ConnectionPoints cPoint, ref string debugGen)
    {
        //Get the forward of the current ConnectionPoint (The possitive X direction of this object in worldspace)
        //This is the agreed direction that a new room will spawn to
        var fw = cPoint.transform.forward;
        var rt = cPoint.transform.right;
        var up = cPoint.transform.up;
        var comb = (fw + rt + up);
        debugGen += "connection points forward: " + fw + "\n";
        debugGen += "connection points right: " + rt + "\n";
        debugGen += "connection points up: " + up + "\n";
        debugGen += "connection points Combined: " + comb + "\n\n";


        //Get the Candidate rooms center
        var CRCenter = (CandidateObject.GetComponent<Room>().MaxSize / 2);
        debugGen += "Candidate room center" + CRCenter + "\n";


        //Get the coordinates of the center offsetted by the current forward, this effectivly should give the candidates room position
        var offset = new Vector3(CRCenter.x * fw.x, CRCenter.y * fw.y, CRCenter.z * fw.z);
        debugGen += "Candidate room's center corrected with the forward (offset)" + offset + "\n";

        var offsetToGraphics = (offset * StepDistance);
        debugGen += "The offset multiplied by the stepdistance" + offsetToGraphics + "\n";

        //Finalised the position of the candidate room
        var newPos = cPoint.transform.position + offsetToGraphics;
        debugGen += "The Calculated position of the candidate room" + newPos + "\n";



        //Debug.Log(Mathf.Acos(fw.x) + " - " + Mathf.Asin(fw.z));
        //Debug.Log(Mathf.Atan(fw.z/fw.x));
        //Debug.Log(Mathf.Atan(fw.x/fw.z));

        //Set the new position
        CandidateObject.transform.position = newPos;
        //Set the parent for a clean result in hierarchy
        CandidateObject.transform.SetParent(Parent.transform);
        //Set the objects name to a counted room so we can easily keep track of the generation order
        CandidateObject.name = "Room" + count++;

        debugGen += "Room name: " + CandidateObject.name + "\n";
    }

    private ConnectionPoints RotateRoomToAllignConPoints(GameObject candidateRoom, ConnectionPoints cPoint, List<Quaternion> anglesUsed, ref string debugGen)
    {
        debugGen += "\n";
        var targetConPoints = candidateRoom.GetComponent<Room>().ConnectionPoints;
        var conAtPosition = targetConPoints.Where(p => p.transform.position == cPoint.transform.position).FirstOrDefault();

        debugGen += "conAtPosition: " + (conAtPosition == null ? "null\n" : (conAtPosition + "\n"));

        string temp = "\n";
        if (anglesUsed != null)
        {
            anglesUsed.ForEach(au => temp += "Angle used: " + au + "\n");
        }
        else
        {
            temp += "Angles Used was null";
        }
        debugGen += temp;

        //if we tested all angles yet, break
        if (anglesUsed.Contains(   Quaternion.Euler(new Vector3(0, 0, 0)))
            && anglesUsed.Contains(Quaternion.Euler(new Vector3(0, 90, 0)))
            && anglesUsed.Contains(Quaternion.Euler(new Vector3(0, 180, 0)))
            && anglesUsed.Contains(Quaternion.Euler(new Vector3(0, 270, 0)))
            )
        {
            debugGen += "Tested all options so return null\n";
            return null;
        }else
        {
            debugGen += "An Angle should still be available\n";
        }


        if (conAtPosition != null)
        {
            //If the cPoint forward and right and up match the conAtPosition -forward and -right and up 
            //then the conectionpoint is not only at the right position but the room is also in a correct rotation
            if (cPoint.transform.forward == -conAtPosition.transform.forward &&
                cPoint.transform.right == -conAtPosition.transform.right &&
                cPoint.transform.up == conAtPosition.transform.up)
            {
                debugGen += "New room should be correctly orriented, no need to rotate in any way\n";
                return conAtPosition;
            }
            else
            {
                //Find a way to rotate the whole CandidateRoom around and reposition it so that any connection point meets up

                debugGen += "New room isn't correctly orriented, find a way to solve\n";
                if (candidateRoom.transform.rotation == Quaternion.Euler(Vector3.zero))
                {
                    debugGen += "Get random start angle";
                    candidateRoom.transform.rotation = GetRandomStartAngle();
                    anglesUsed.Add(candidateRoom.transform.rotation);
                }
                else
                {
                    debugGen += "Get random Angle";
                    candidateRoom.transform.rotation = GetRandomAngle(anglesUsed, ref debugGen);
                    anglesUsed.Add(candidateRoom.transform.rotation);
                }
                return RotateRoomToAllignConPoints(candidateRoom, cPoint, anglesUsed, ref debugGen);
            }

            //debugGen += "conAtPosition, Forward:" + conAtPosition.transform.forward + "\n";
            //debugGen += "conAtPosition, Right:" + conAtPosition.transform.right + "\n";
            //debugGen += "conAtPosition, Up:" + conAtPosition.transform.up + "\n\n";

            //debugGen += "conAtPosition, -Forward:" +(-conAtPosition.transform.forward) + "\n";
            //debugGen += "conAtPosition, -Right:" + (-conAtPosition.transform.right) + "\n";
            //debugGen += "conAtPosition, -Up:" + (-conAtPosition.transform.up) + "\n\n";
        }
        else
        {
            //Find a way to rotate the whole CandidateRoom around and reposition it so that any connection point meets up

            if (candidateRoom.transform.rotation == Quaternion.Euler(Vector3.zero))
            {
                debugGen += "Get random start angle";
                candidateRoom.transform.rotation = GetRandomStartAngle();
                anglesUsed.Add(candidateRoom.transform.rotation);
            }
            else
            {
                debugGen += "Get random Angle";
                candidateRoom.transform.rotation = GetRandomAngle(anglesUsed, ref debugGen);
                anglesUsed.Add(candidateRoom.transform.rotation);
            }
            return RotateRoomToAllignConPoints(candidateRoom, cPoint, anglesUsed, ref debugGen);
        }

    }




    //Support methods
    private Quaternion GetRandomStartAngle()
    {
        Quaternion retval = Quaternion.identity;


        //Randomize Seed
        Random.InitState(System.DateTime.Now.Millisecond);
        //Get random value
        int r = Random.Range(0, 3);

        switch (r)
        {
            case 0:
                retval = Quaternion.Euler(0, 90, 0);
                break;
            case 1:
                retval = Quaternion.Euler(0, 180, 0);
                break;
            case 2:
                retval = Quaternion.Euler(0, 270, 0);
                break;
        }

        return retval;

    }
    private Quaternion GetRandomAngle(List<Quaternion> anglesUsed, ref string debugGen)
    {
        

        Quaternion retval = Quaternion.identity;

        List<Quaternion> options = new List<Quaternion>();
        options.Add(Quaternion.Euler(new Vector3(0, 0, 0)));
        options.Add(Quaternion.Euler(new Vector3(0, 90, 0)));
        options.Add(Quaternion.Euler(new Vector3(0, 180, 0)));
        options.Add(Quaternion.Euler(new Vector3(0, 270, 0)));

       

        debugGen += "\n";
        foreach (var item in anglesUsed)
        {
            debugGen += "checking item: " + item + " - ";
            var reduc = options.Where(tem => tem.x == item.x && tem.y == item.y && tem.z == item.z && tem.w == item.w).FirstOrDefault();
            
            debugGen += "checking reduc: " + reduc + " options contains item (" + options.Contains(reduc) + ")   ";
            

            if (options.Contains(reduc))
            {
                debugGen += " - removing option: " + reduc;
                options.Remove(reduc);
            }
            debugGen += "\n";
        }

        string temp = "\n";
        if (options != null)
        {
            options.ForEach(au => temp += "Options: " + au.eulerAngles + " - " +au + "\n");
        }
        else
        {
            temp += "options was null";
        }
        debugGen += temp;


        //Randomize Seed
        Random.InitState(System.DateTime.Now.Millisecond);
        //Get random value
        int r = Random.Range(0, options.Count);

        retval = options[r];

        return retval;

    }
    private GameObject GetStartRoom()
    {
        var SCP = RoomsManager.Instance.CurrentRoomCollection.StartChamberPrefabs;
        var max = SCP.Count;
        return SCP[Random.Range(0, max)];
    }
    private GameObject GetCorridorPart(int randomVal)
    {
        var CP = RoomsManager.Instance.CurrentRoomCollection.CorridorPrefabs;
        var CPC = RoomsManager.Instance.CurrentRoomCollection.CorridorPrefabsChances;

        int index = -1;
        for (int i = 0; i < CPC.Count; i++)
        {
            if (index != -1)
            {
                if (randomVal <= CPC[i] && CPC[index] > CPC[i]) index = i;
            }
            else
            {
                index = i;

            }
        }

        if (index == -1)
        {
            Debug.LogError("somehow counldn't cope with: " + randomVal + " as a random value, Setting to 0 to not crash");
            index = 0;
        }
        else
        {
            //Debug.LogError("handled: " + randomVal + " as a random value, using index: " + index + " | " + CPC[index] + " | " + CP[index]);
        }

        return CP[index];

    }
    /// <summary>
    /// Returns a float array containing the data of all extreme points of a cube
    /// </summary>
    /// <param name="cubeCenter">Center of the cube</param>
    /// <param name="cubeMaxSize">Maximum size of a cube, in all directions. Can be considered the longest edge of a cube</param>
    /// <returns>a 6 float array with the following order, xMin, yMin, zMin, xMax, yMax, zMax</returns>
    private float[] GetCubeDimensions(Vector3 cubeCenter, Vector3 cubeMaxSize)
    {
        float[] retval = new float[6];

        retval[0] = cubeCenter.x - (cubeMaxSize.x / 2);
        retval[1] = cubeCenter.y - (cubeMaxSize.y / 2);
        retval[2] = cubeCenter.z - (cubeMaxSize.z / 2);
        retval[3] = cubeCenter.x + (cubeMaxSize.x / 2);
        retval[4] = cubeCenter.y + (cubeMaxSize.y / 2);
        retval[5] = cubeCenter.z + (cubeMaxSize.z / 2);

        return retval;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
    /// <param name="checkVal1"></param>
    /// <param name="checkVal2"></param>
    /// <param name="IncludeSameNumber"></param>
    /// <returns></returns>
    private bool IsValueBetween(float val, float checkVal1, float checkVal2, bool IncludeSameNumber)
    {
        if (checkVal1 == checkVal2) { throw new System.Exception("CheckVal1 == CheckVal2"); return false; }


        float min, max;
        if (checkVal1 > checkVal2)
        {
            min = checkVal2;
            max = checkVal1;
        }else
        {
            min = checkVal1;
            max = checkVal2;
        }


        if (IncludeSameNumber == true)
        {
            return (val >= min && val <= max);
        }
        else
        {
            return (val > min && val < max);
        }
    }

    private bool CheckOverlap(Room candidateRoom, Room checkRoom, ref string debugGen)
    {

        debugGen += "\n\nCHECK OVERLAP:  " + candidateRoom.name + " & " + checkRoom.name + "\n";

        //Check same position
        if (candidateRoom.transform.position == checkRoom.transform.position)
        {
            debugGen += "overlap same position";
            return false;
        }

        #region Get Data Candidate Room
        Vector3 cRc = candidateRoom.gameObject.transform.position;
        Vector3 cRb = candidateRoom.MaxSize * StepDistance;
        float[] cRd = GetCubeDimensions(cRc, cRb);
        float cRxMin = cRd[0];
        float cRyMin = cRd[1];
        float cRzMin = cRd[2];
        float cRxMax = cRd[3];
        float cRyMax = cRd[4];
        float cRzMax = cRd[5];

        debugGen += "candidateroom pos: " + cRc.x + ", " + cRc.y + ", " + cRc.z+ "\n";
        debugGen += "candidateroom min: " + cRxMin + ", " + cRyMin + ", " + cRzMin + "\n";
        debugGen += "candidateroom max: " + cRxMax + ", " + cRyMax + ", " + cRzMax + "\n\n";
        #endregion

        #region Get Data Checking Room
        Vector3 chRc = checkRoom.gameObject.transform.position;
        Vector3 chRb = checkRoom.MaxSize * StepDistance;
        float[] chRd = GetCubeDimensions(chRc, chRb);
        float chRxMin = chRd[0];
        float chRyMin = chRd[1];
        float chRzMin = chRd[2];
        float chRxMax = chRd[3];
        float chRyMax = chRd[4];
        float chRzMax = chRd[5];

        debugGen += "checkRoom pos: " + chRc.x + ", " + chRc.y + ", " + chRc.z + "\n";
        debugGen += "checkRoom min: " + chRxMin + ", " + chRyMin + ", " + chRzMin + "\n";
        debugGen += "checkRoom max: " + chRxMax + ", " + chRyMax + ", " + chRzMax + "\n\n";
        #endregion

        #region broad check

        float dist = Vector3.Distance(chRc, cRc);
        float radiuschR = Mathf.Sqrt(Mathf.Pow(chRb.x/2, 2) + Mathf.Pow(chRb.y/2, 2));
        float radiuscR = Mathf.Sqrt(Mathf.Pow(cRb.x/2, 2) + Mathf.Pow(cRb.y/2, 2));

        if (dist > (radiuschR + radiuscR))
        {
            debugGen += "overlap dist (" + dist + ") > (radiuschR + radiuscR) - (" + radiuschR + " + " + radiuscR + ") = " + (radiuschR + radiuscR);
            return false;
        }
        #endregion



        #region Precise check
        bool xminClear = (cRxMin > chRxMin && cRxMin < chRxMax);
        bool xmaxClear = (cRxMax > chRxMin && cRxMax < chRxMax);

        debugGen += "x min clear: " + xminClear + " | x max clear: " + xmaxClear + "\n";

        bool yminClear = (cRyMin > chRyMin && cRyMin < chRyMax);
        bool ymaxClear = (cRyMax > chRyMin && cRyMax < chRyMax);

        debugGen += "y min clear: " + yminClear + " | y max clear: " + ymaxClear + "\n";

        bool zminClear = (cRzMin > chRzMin && cRzMin < chRzMax);
        bool zmaxClear = (cRzMax > chRzMin && cRzMax < chRzMax);

        debugGen += "z min clear: " + zminClear + " | z max clear: " + zmaxClear + "\n";

        bool xClear = xminClear || xmaxClear;
        bool yClear = yminClear || ymaxClear;
        bool zClear = zminClear || zmaxClear;

        debugGen += "x clear: " + xClear + "\n";
        debugGen += "y clear: " + yClear + "\n";
        debugGen += "z clear: " + zClear + "\n\n";

        bool xyClear = (xClear && yClear);
        bool xzClear = (xClear && zClear);
        bool yzClear = (yClear && zClear);

        debugGen += "xy clear: " + xyClear + "\n";
        debugGen += "xz clear: " + xzClear + "\n";
        debugGen += "yz clear: " + yzClear + "\n\n";

        //check if it's within bounds in any combination of the axis's
        bool allClear = (xyClear || xzClear || yzClear);

        debugGen += allClear + "\n";

        #endregion
        return allClear;
    }
}
