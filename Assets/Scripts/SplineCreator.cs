using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Linq;

public class SplineCreator : MonoBehaviour
{

    public class TrackStructure{
        public TrackUnit.Track tUnit;
        public bool used;
        
        public int index;

        public TrackStructure(TrackUnit.Track t, bool b){
            tUnit = t;
            used = b;
        }
    }
    
    public GameObject trackObject;
    GameObject parent;
    GameObject splineObject;
    SplineContainer container;
    Spline spline;
    
    int turnParser = 0;
    int loopParser = 0;
    float pastHeight = 0f;
    private int xModifier; //Size of x of actual track unit
    private float yModifier; //y of actual track unit
    private int zModifier; //z of actual track unit

    private List<TrackStructure> trackList;

    // Start is called before the first frame update
    void Start()
    {
        //firstTurn = false;
    }

    public void createSplineContainer(){
        turnParser = 0;
        if(parent == null){
            parent = GameObject.Find("CustomTrack");
            //Debug.Log("Creating ");
            splineObject = new GameObject("SplineTrack");
            splineObject.transform.position = parent.transform.position;
            
            splineObject.transform.parent = parent.transform;
            container = splineObject.AddComponent<SplineContainer>();
            container.RemoveSplineAt(0);
            spline = container.AddSpline();
        }
        else if(splineObject == null){
            splineObject = new GameObject("SplineTrack");
            splineObject.transform.position = parent.transform.position;
            splineObject.transform.parent = parent.transform;
            container = splineObject.AddComponent<SplineContainer>();
            container.RemoveSplineAt(0);
            spline = container.AddSpline();
        }
        //creat array
        trackList = new List<TrackStructure>();
    }
    public void handleTrackUnitObject(TrackUnit.Track tIn){
        //handles track unit object sent by file reader
        //UPPER FIRST LETTER TO ACCESS ATTRIBUTE
        /*
        string trackType;
        Vector3 coordinates; 
        Vector3 adjustedCoordinates; 
        int direction; 
        string trackClassification;
        string trackSlope; 
        string trackBank; 
        int speed;
        int lateralGForce;
        int verticalGForce;
        */
        //adjustCoordinateValuesV1(tIn);

        //FOR V2
        if(tIn.TrackType.Contains("TURN")){
            if(turnParser == 0){
                adjustCoordinateValuesV2(tIn);
                if(tIn.TrackType.Contains("5")){
                    turnParser = 7;
                }
                if(tIn.TrackType.Contains("3")){
                    turnParser = 4;
                }
            }
            else if(turnParser == 1){
                adjustCoordinateValuesV2(tIn);
                
            }
            turnParser--;
        }
        else if(tIn.TrackType.Contains("LOOP")){
            //Debug.Log("LOOP:" + tIn.Coordinates);
            if(pastHeight == tIn.Coordinates.y){
                if(loopParser % 2 == 0){
                    trackList.RemoveAt(trackList.Count - 1);
                }
                else{
                    adjustCoordinateValuesV2(tIn);
                }
                loopParser++;
            }
            else{
                adjustCoordinateValuesV2(tIn);
            }
            pastHeight = tIn.Coordinates.y;
            turnParser = 0;
            
        } 
        else{
            loopParser = 0;
            turnParser = 0;
            adjustCoordinateValuesV2(tIn);
        } 
       
        
    }
    private void adjustCoordinateValuesV1(TrackUnit.Track tIn){
        //title + add the unit to track list if conditions are ok!!!
        xModifier = 20;
        yModifier = 5f;
        zModifier= 20;

        TrackStructure tr;
        if(tIn.TrackType.Contains("TURN")){
            //turn track
            if(!trackList.Last().tUnit.TrackType.Contains("TURN")){//first turn track? -->
                tIn.AdjustedCoordinates = new Vector3(
                    tIn.Coordinates.x * xModifier,
                    tIn.Coordinates.y * yModifier,
                    tIn.Coordinates.z * zModifier
                    ); 
                tr = new TrackStructure(tIn, true);
                trackList.Add(tr);
            }
            else if(tIn.Coordinates != trackList.Last().tUnit.Coordinates){
                //calculate the adjusted value
                tIn.AdjustedCoordinates = new Vector3(
                    (tIn.Coordinates.x * xModifier + trackList.Last().tUnit.AdjustedCoordinates.x)/2,
                    (tIn.Coordinates.y * yModifier + trackList.Last().tUnit.AdjustedCoordinates.y)/2,
                    (tIn.Coordinates.z * zModifier + trackList.Last().tUnit.AdjustedCoordinates.z)/2
                );
                tr = new TrackStructure(tIn, true);
                trackList.Add(tr);
            }
        }
        else{//normal -- or extra track
            if(trackList.Count == 0){
                tIn.AdjustedCoordinates = new Vector3(
                tIn.Coordinates.x * xModifier,
                tIn.Coordinates.y * yModifier,
                tIn.Coordinates.z * zModifier
                );
                tr = new TrackStructure(tIn, true);
                trackList.Add(tr);
            }
            else{
                if(
                tIn.Coordinates != trackList.Last().tUnit.Coordinates){
                    tIn.AdjustedCoordinates = new Vector3(
                    tIn.Coordinates.x * xModifier,
                    tIn.Coordinates.y * yModifier,
                    tIn.Coordinates.z * zModifier
                    );
                    tr = new TrackStructure(tIn, true);
                    trackList.Add(tr);
                }
            }
            
        }
        //TEST NEW TRACK
        /*if(trackList.Count == 0){
            tIn.AdjustedCoordinates = new Vector3(
                tIn.Coordinates.x * xModifier,
                tIn.Coordinates.y * yModifier,
                tIn.Coordinates.z * zModifier
                );
                tr = new TrackStructure(tIn, true);
                trackList.Add(tr);
        }
        else if(tIn.Coordinates != trackList.Last().tUnit.Coordinates){
                tIn.AdjustedCoordinates = new Vector3(
                tIn.Coordinates.x * xModifier,
                tIn.Coordinates.y * yModifier,
                tIn.Coordinates.z * zModifier
                );
                tr = new TrackStructure(tIn, true);
                trackList.Add(tr);
        }*/
    }
    private void adjustCoordinateValuesV2(TrackUnit.Track tIn){
        xModifier = 20;
        yModifier = 5f;
        zModifier= 20;

        TrackStructure tr;
         //if turn track -- add it normally - will be editing the coordinates when instantiating actual spline
        if(trackList.Count == 0){
            tIn.AdjustedCoordinates = new Vector3(
            tIn.Coordinates.x * xModifier,
            tIn.Coordinates.y * yModifier,
            tIn.Coordinates.z * zModifier
            );
            tr = new TrackStructure(tIn, true);
            trackList.Add(tr);
        }
        else{
            if(
            tIn.Coordinates != trackList.Last().tUnit.Coordinates){

                tIn.AdjustedCoordinates = new Vector3(
                tIn.Coordinates.x * xModifier,
                tIn.Coordinates.y * yModifier,
                tIn.Coordinates.z * zModifier
                );
                tr = new TrackStructure(tIn, true);
                trackList.Add(tr);
            }
        }
    }
    public void createTrackV1(){
        //create the track from list
        int j = 0; //to keep track of index in spline.
        for(int i = 0; i < trackList.Count; i++){
            TrackStructure t = trackList[i];
            if(t.used){
                if(t.tUnit.TrackType.Contains("TURN")){
                    if(
                    (i + 1 < trackList.Count && trackList[i + 1].tUnit.TrackType.Contains("TURN")) ||
                    (i - 1 >= 0 && trackList[i - 1].tUnit.TrackType.Contains("TURN"))
                    ){
                        createTurnKnotV1(t.tUnit);
                    }
                    else{
                        createSmallTurnKnotV1(t.tUnit);
                    }
                    
                }
                else{
                    createNormalKnotV1(t.tUnit);
                }
                t.index = j;
                j++;
                //createNormalKnot(t.tUnit);
            }
        }
    }

    public void createTrackV2(){

        bool breakpoint = false;

        for(int i = 0; i < trackList.Count; i++){
            //TODO: create a turn from the second-third connections midpoint
            TrackStructure t = trackList[i];
            if(t.used){
                createNormalKnotV2(t.tUnit);
            }   
           
        }
    }
    private void createNormalKnotV1(TrackUnit.Track trackUnit){
        BezierKnot knot = new BezierKnot(trackUnit.AdjustedCoordinates);
        //knot.Rotation = knot.Rotation * checkForBank(trackUnit,knot);
        //Debug.Log(knot.Rotation);
        spline.Add(knot, TangentMode.Continuous);
    }
    private void createSmallTurnKnotV1(TrackUnit.Track trackUnit){

        if(trackUnit.TrackType.Contains("RIGHT")){
            //right turn modifications
            if(trackUnit.Direction == 1){
                //Debug.Log(trackUnit.Coordinates);
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x , trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z + 20);
            }
            else if(trackUnit.Direction == 2){
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x + 20, trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z);
            }
            else if(trackUnit.Direction == 3){
                //Debug.Log(trackUnit.Coordinates);
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x, trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z - 20);
            }
            else if(trackUnit.Direction == 0){
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x - 20, trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z);
            }
        }
        else{
            if(trackUnit.Direction == 1){
                //Debug.Log(trackUnit.Coordinates);
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x, trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z + 20);
            }
            else if(trackUnit.Direction == 2){
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x + 20, trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z);
            }
            else if(trackUnit.Direction == 3){
                //Debug.Log(trackUnit.Coordinates);
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x, trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z - 20);
            }
            else if(trackUnit.Direction == 0){
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x - 20, trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z);
            }
        }
        BezierKnot knot = new BezierKnot(trackUnit.AdjustedCoordinates);
        //knot.Rotation = knot.Rotation * checkForBank(trackUnit,knot);
        spline.Add(knot, TangentMode.Continuous);
    }
    private void createNormalKnotV2(TrackUnit.Track trackUnit){
        //TODO: FIX
        BezierKnot knot = new BezierKnot(trackUnit.AdjustedCoordinates);
        if(trackUnit.TrackType.Contains("LOOP")){
            knot.Rotation = new Quaternion(0,0,0,0);
        }
        //knot.Rotation = knot.Rotation * checkForBank(trackUnit,knot);
        spline.Add(knot, TangentMode.Continuous);
    }
    private void createSmallTurnKnotV2(TrackUnit.Track trackUnit){
        //TODO: FIX
        BezierKnot knot = new BezierKnot(trackUnit.AdjustedCoordinates);
        //knot.Rotation = knot.Rotation * checkForBank(trackUnit,knot);
        spline.Add(knot, TangentMode.Continuous);
    }

    private void createTurnKnotV1(TrackUnit.Track trackUnit){
        //BIG TURN
        if(trackUnit.TrackType.Contains("RIGHT")){
            //right turn modifications
            if(trackUnit.Direction == 1 || trackUnit.Direction == 0){
                //Debug.Log(trackUnit.Coordinates);
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x , trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z +10);
            }
            else{
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x, trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z - 10);
            }
        }
        else{
            if(trackUnit.Direction == 1 || trackUnit.Direction == 2){
                //Debug.Log(trackUnit.Coordinates);
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x , trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z + 10);
            }
            else{
                trackUnit.AdjustedCoordinates = new Vector3(trackUnit.AdjustedCoordinates.x, trackUnit.AdjustedCoordinates.y, trackUnit.AdjustedCoordinates.z - 10);
            }
        }
        
        BezierKnot knot = new BezierKnot(trackUnit.AdjustedCoordinates);
        //knot.Rotation = knot.Rotation * checkForBank(trackUnit,knot);
        spline.Add(knot, TangentMode.Continuous);
    }

    private Quaternion checkForBank(TrackUnit.Track trackUnit, BezierKnot knot){     
         //rotate to obj1's local space   
        if(trackUnit.TrackBank == "LEFT"){
            if(trackUnit.TrackType.Contains("TURN")){
                if(trackUnit.Direction == 0 || trackUnit.Direction == 3){
                    var d = Quaternion.AngleAxis(45, Vector3.right);
                    return d;
                }
            }
            var q = Quaternion.AngleAxis(-45, Vector3.right);
            return q;
        }
        else if(trackUnit.TrackBank == "RIGHT"){
            //Debug.Log("RIGHT?");
            var q = Quaternion.AngleAxis(45, Vector3.right);
            return q;
        }
        else{
            var q = Quaternion.AngleAxis(0, Vector3.right);
            return q;
        }

    }

    
    private void fixBanks(){
        foreach(TrackStructure t in trackList){
            if(t.used && t.tUnit.TrackBank != "NONE"){ //track is used and there is bank
                var q =  Quaternion.AngleAxis(45, Vector3.right);
                if(t.tUnit.TrackBank == "RIGHT"){
                    
                    if(t.tUnit.Direction == 0 || t.tUnit.Direction == 3){
                        q = Quaternion.AngleAxis(-45, Vector3.right);
                    }
                    else{
                         q = Quaternion.AngleAxis(45, Vector3.right);
                    }
                }
                else{
                   if(t.tUnit.Direction == 0 || t.tUnit.Direction == 3){
                        q = Quaternion.AngleAxis(45, Vector3.right);
                    }
                    else{
                         q = Quaternion.AngleAxis(-45, Vector3.right);
                    }
                }
                BezierKnot knot = new BezierKnot(t.tUnit.AdjustedCoordinates);
                knot.Rotation = q * knot.Rotation;
                spline.SetKnot(t.index, knot);
            }
            else if(t.used){
                BezierKnot knot = new BezierKnot(t.tUnit.AdjustedCoordinates);
                knot.Rotation = new Quaternion(0,0,0,0);
                spline.SetKnot(t.index, knot);
            }
        }
    }

    public void InstantiateTrack(){
        addSpeedDataToSpline();
        addLocationDataToSpline();
        spline.Closed = true;
        SplineRange r = new SplineRange(0,spline.Count);
        //IMPORTANT -- AUTOSMOOTH
        
        fixBanks();
        spline.SetTangentMode(r, TangentMode.AutoSmooth);
        fixBanks();
        //Debug.Log("Track instantiation starting...");
        SplineInstantiate si = splineObject.AddComponent<SplineInstantiate>();
        
        
        
        SplineInstantiate.InstantiableItem item = new SplineInstantiate.InstantiableItem();
        item.Prefab = trackObject;
        SplineInstantiate.InstantiableItem[] temp_s = new SplineInstantiate.InstantiableItem[1];
        temp_s[0] = item;

        si.itemsToInstantiate = temp_s;
        si.itemsToInstantiate[0].Prefab = trackObject;
        si.MaxSpacing = 1.75f;
        si.MinSpacing = 1.70f;
        
        splineObject.tag = "Spline";
        //si.UpdateInstances();
        //SplineInstantiate.InstantiableItem item = new SplineInstantiate.InstantiableItem(trackObject);;
        /*SplineInstantiate.InstantiableItem item = new SplineInstantiate.InstantiableItem();
        item.Prefab = trackObject;
        si.itemsToInstantiate = new SplineInstantiate.InstantiableItem[1]();
        si.itemsToInstantiate[0] = trackObject;*/
    }

    
    private void addSpeedDataToSpline(){
        SplineData<float> speedList = spline.GetOrCreateFloatData("SpeedList");
        foreach(TrackStructure t in trackList){
            if(t.used){
                DataPoint<float> unit = new DataPoint<float>(t.index,t.tUnit.Speed);
                speedList.Add(unit);
            }
        }
    }
    private void addLocationDataToSpline(){
        SplineData<float4> locationList = spline.GetOrCreateFloat4Data("LocationList");
        foreach(TrackStructure t in trackList){
            if(t.used){
                DataPoint<float4> unit = new DataPoint<float4>(t.index,new float4(t.tUnit.AdjustedCoordinates.x,t.tUnit.AdjustedCoordinates.y,t.tUnit.AdjustedCoordinates.z,0));
                locationList.Add(unit);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

