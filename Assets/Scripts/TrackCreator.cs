using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackCreator : MonoBehaviour
{
    public GameObject flatTrackPrefab;
    public GameObject flatTo25UpPrefab;
    public GameObject Up25ToFlatPrefab;
    public GameObject Up25Prefab;
    public GameObject rightTurn3Prefab;
    private int xModifier; //Size of x of actual track unit
    private float yModifier; //y of actual track unit
    public float currentHeight = 0f;
    private int zModifier; //z of actual track unit
    Transform parent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void handleTrackInput(string trackType, int x, int y, int z, int dir){
        yModifier = 7f;
        xModifier = 20;
        zModifier = 30;
        if(parent == null){
             parent = GameObject.Find("CustomTrack").transform;
             currentHeight = 0f;
        }
       
        if(trackType == "ELEM_END_STATION" || trackType == "ELEM_FLAT" || trackType == "ELEM_BEGIN_STATION" ||  trackType == "ELEM_MIDDLE_STATION"){
            createFlat(x,y,z,dir);
        }
        else if(trackType == "ELEM_FLAT_TO_25_DEG_UP"){
            createFlatTo25Up(x,y,z,dir);
        }
        else if(trackType == "ELEM_25_DEG_UP"){
            create25Up(x,y,z,dir);
        }
        else if(trackType == "ELEM_25_DEG_UP_TO_FLAT"){
            create25UpToFlat(x,y,z,dir);
        }
        else if(trackType == "ELEM_25_DEG_DOWN"){
            create25Down(x,y,z,dir);
        }
        else if(trackType == "ELEM_RIGHT_QUARTER_TURN_3_TILES"){
            createRightTurn3(x,y,z,dir);
        }


    }

    void createFlat(int x, int y, int z, int dir){
        Instantiate(flatTrackPrefab, parent.position + (new Vector3(xModifier * x,y * yModifier, z * zModifier)), Quaternion.identity, parent);
    }
    void createFlatTo25Up(int x, int y, int z, int dir){
        GameObject t = Instantiate(flatTo25UpPrefab, parent.position + (new Vector3(xModifier * x,(y-1) * yModifier, z * zModifier)), Quaternion.identity, parent);

        //currentHeight = (y + 1) * yModifier;
    }
    void create25Up(int x, int y, int z, int dir){
        //Height = 14 -- 7 is one unit
        GameObject t = Instantiate(Up25Prefab, parent.position + (new Vector3(xModifier * x,(y-1) * yModifier, z * zModifier)), Quaternion.identity, parent);
        Debug.Log("25UP - Y: " + y + " Y modifier = " + yModifier);
    }
    void create25UpToFlat(int x, int y, int z, int dir){
        GameObject t = Instantiate(Up25ToFlatPrefab, parent.position + (new Vector3(xModifier * x,y * yModifier, z * zModifier)), Quaternion.identity, parent);
        
        //currentHeight = (y + 1) * yModifier;
    }
    void create25Down(int x, int y, int z, int dir){
        
        
        //currentHeight = (y + 1)* yModifier;
    }
    void createRightTurn3(int x, int y, int z, int dir){
        GameObject t = Instantiate(rightTurn3Prefab, parent.position + (new Vector3(xModifier * x,y * yModifier, z * zModifier)), Quaternion.identity, parent);
    }
    //TODO: EITHER EDIT THE SCALE WITH ACTUAL LENGTH OR CHANGE LENGTH TO EXACTLY 1 UNIT FOR EVERY CASE
    //1 FLAT UNIT is 30 in length 10 in width and AROUND 4.5-5 in height (presumed height)
    // Update is called once per frame
    //USE SPLINES 
    //DISREGARD THIS ATTEMPT!!!
    void Update()
    {
        
    }
}
