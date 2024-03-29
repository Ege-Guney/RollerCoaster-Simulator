using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System;


public class GenerateCoasterExternalities : MonoBehaviour
{
    public GameObject supportPrefab;
    SplineContainer target;
    Spline spline;
    SplineData<float4> locationList;
    SplineData<float> speedList;
    public void findContainer(){
        target = GameObject.Find("CustomTrack/SplineTrack").GetComponent<SplineContainer>();
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
    private bool checkDownwardObstacle(Vector3 location){
        //RaycastHit hitPoint;
        /*RaycastHit hit;
		//Ray ray = Physics.Raycast(location, -Vector3.up, out hit);
		if(Physics.Raycast(location, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
		{
            Debug.Log("Hitting Something " + hit.collider.tag);
			if(hit.collider.tag == "Spline")
			{
				Debug.Log("Hit Track"); 
                return true;
			}
        }*/
        foreach(DataPoint<float4> loc in locationList){
            if(location.y != loc.Value.y){
                if(Math.Abs(location.x - loc.Value.x) <= 5 && Math.Abs(location.z - loc.Value.z) <= 5){
                    return true;
                }
            }
        }
        //Debug.DrawRay(location, -Vector3.up, Color.red, Mathf.Infinity);
        return false;
    }
    private void createSupports(){
        GameObject supports = new GameObject("Supports");
        supports.transform.parent = (GameObject.Find("CustomTrack")).transform;
        foreach(DataPoint<float4> loc in locationList){
            var location = new Vector3(loc.Value.x,loc.Value.y,loc.Value.z);
            
            Debug.DrawRay(location, -Vector3.up * 100, Color.red, Mathf.Infinity);
            if(loc.Value.y >= 7 && !checkDownwardObstacle(location)){
                var c = Instantiate(supportPrefab, new Vector3(loc.Value.x, 0, loc.Value.z), Quaternion.identity);
                var ch = c.transform.GetChild(0).gameObject;
                ch.transform.localScale += new Vector3(0,loc.Value.y + 2f,0);
                c.transform.parent = supports.transform;
            } 
        }
    }

    public void startCreation(){
        findContainer();
        getSpline();
        locationList = getLocationData();
        speedList = getSpeedData();
        createSupports();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
