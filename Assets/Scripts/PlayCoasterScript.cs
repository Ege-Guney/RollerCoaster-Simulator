using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayCoasterScript : MonoBehaviour
{
    [SerializeField]
    float startSpeed = 1f;

    [SerializeField]
    public float futureAdjuster = 0.03f;

    [SerializeField]
    bool m_PlayOnAwake = true;

    [SerializeField]
    float speedCoefficient = 0.002f;


    SplineContainer target;

   
    float m_NormalizedTime;
    float m_ElapsedTime;
    SplinePath<Spline> splinePath;
    float splineLength;
    Spline spline;
    List<SplineCreator.TrackStructure> tList;

    List<TrackUnit.Track> resList;

    SplineData<float4> locationList;
    SplineData<float> speedList;

    int cur_index = 0;

    float t = 0;

    float cur_speed = 0.01f;
    
    public void findContainer(){
        target = GameObject.Find("CustomTrack/SplineTrack").GetComponent<SplineContainer>();
    }

    void createResList(){
        foreach(SplineCreator.TrackStructure t in tList){
            if(t.used)
                resList.Add(t.tUnit);
        }
    }
    void getSplinePath(){
        splinePath = new SplinePath<Spline>(target.Splines);
    }
    
    private SplineData<float4> getLocationData(){
        return spline.GetOrCreateFloat4Data("LocationList");
    }
    private SplineData<float> getSpeedData(){
        return spline.GetOrCreateFloatData("SpeedList");
    }

    private void getSpline(){
        spline = target.Splines[0];
    }

    private void updatePosition(){
        transform.position = target.EvaluatePosition(splinePath, t);
    }

    private void updateRotation(){
        var forward = Vector3.Normalize(target.EvaluateTangent(splinePath, t));
        var up = target.EvaluateUpVector(splinePath, t);
        //var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(Vector3.forward,Vector3.up));
        
        transform.rotation = Quaternion.LookRotation(forward, up) ;
    } 

    private void setClosestLocationIndex(){
        Vector3 futurePos = target.EvaluatePosition(splinePath, t + futureAdjuster);
        //figure out the closest knot index!
        if(cur_index + 1 < locationList.Count){
            DataPoint<float4> unit1 = locationList[cur_index];
            DataPoint<float4> unit2 = locationList[cur_index + 1];
            Vector3 v1 = new Vector3(unit1.Value.x, unit1.Value.y, unit1.Value.z);
            Vector3 v2 = new Vector3(unit2.Value.x, unit2.Value.y, unit2.Value.z);
            if(Vector3.Distance(v2, futurePos) <= Vector3.Distance(v1,futurePos)){
                cur_index++;
            }
        }
        //Debug.Log("INDEX: " + cur_index);
    }

    private float adjustSpeed(float goalSpeed){
        
        if(Math.Abs(cur_speed - goalSpeed) <= 2){
            //slow increase or decrease -- instant speed change
            if(goalSpeed < 1){
                return 1f;
            }
            return goalSpeed;
        }
        if(cur_speed > goalSpeed){
            //decelerate
            return cur_speed - ((cur_speed - goalSpeed)* 1/4);
        }
        else{ //accelerate
            return cur_speed + ((goalSpeed - cur_speed) * 3/4);
        }
    }

    private void updateSpeed(){
        //cur_speed = 0.005f *  speedList[cur_index].Value;
        cur_speed = adjustSpeed(speedList[cur_index].Value);
       
        Debug.Log("Speed: " + cur_speed);
    }

    //increase t according to the speed knot you are closest to has
    //TODO: CHANGE INCREASE TO GRADUALLY INCREASING
    void Start()
    {
        //SplineCreator tc = GameObject.Find("SplineSpawner").GetComponent<SplineCreator>();
        //Debug.Log(tc);
        findContainer();
        getSpline();
        getSplinePath();
        locationList = getLocationData();
        speedList = getSpeedData();
        t = 0;
    }
    // Update is called once per frame
    void Update()
    {
        t += cur_speed * speedCoefficient * Time.deltaTime;
        setClosestLocationIndex();
        updateSpeed();
        updatePosition();
        updateRotation();
    }
}
