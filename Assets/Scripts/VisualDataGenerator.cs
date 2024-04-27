using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
public class VisualDataGenerator : MonoBehaviour
{
    [SerializeField]
    int method = 0;

    SplineContainer target;

    SplinePath<Spline> splinePath;
    float splineLength;
    Spline spline;
    SplineData<float4> locationList;
    SplineData<int> anticipationData;
    Camera cam;
    GameObject splineObject;
    bool dataSent;
    Dictionary<Vector3, List<Vector3>> results;

    
    Transform closestTrackUnit;
    bool dataAcquired = false;
    int cur_index = 0;
    public void findContainer(){
        target = GameObject.Find("CustomTrack/SplineTrack").GetComponent<SplineContainer>();
    }
    void getSplinePath(){
        splinePath = new SplinePath<Spline>(target.Splines);
    }
    
    private SplineData<float4> getLocationData(){
        return spline.GetOrCreateFloat4Data("LocationList");
    }
    private void getSpline(){
        spline = target.Splines[0];
    }
    //same methods in Playcoaster
    private void setClosestLocationIndex(){
        //Vector3 futurePos = target.EvaluatePosition(splinePath, t + futureAdjuster);
        //figure out the closest knot index
        float res_pos = 1000000;     
        foreach(Transform child in splineObject.transform){
            float temp = Vector3.Distance(transform.position, child.position);
            if(temp < res_pos){
                res_pos = temp;
                closestTrackUnit = child;
            }
        }
    }

    List<Vector3> findAnticipationData(){
        //int result = 0;
        List<Vector3> res = new List<Vector3>();
        foreach(Transform child in splineObject.transform){
            //check if point is in sight
            Vector3 vpPos = cam.WorldToViewportPoint(child.position);
            if (vpPos.x >= 0f && vpPos.x <= 1f && vpPos.y >= 0f && vpPos.y <= 1f && vpPos.z > 0f) {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.TransformDirection(child.position), out hit, Mathf.Infinity)){
                    if(!hit.transform.CompareTag("Ground")){
                        continue;
                    }
                }
                res.Add(child.position);
            }
        }
        return res;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        findContainer();
        getSpline();
        getSplinePath();
        locationList = getLocationData();
        dataAcquired = false;
        results = new Dictionary<Vector3, List<Vector3>>();
        dataSent = false;
        splineObject = GameObject.Find("SplineTrack");
    }

    // Update is called once per frame
    void Update()
    {
        setClosestLocationIndex();
        if(!results.ContainsKey(closestTrackUnit.position)){
            List<Vector3> res = findAnticipationData();
            //write to file...
            Debug.Log("Writing..." + res);
            results.Add(closestTrackUnit.position, res);
            dataAcquired = true;
        }    
    }

    void OnApplicationQuit()//Now that we are quiting the application we can write our data to a file
    {
        FileWriter fw = GameObject.Find("SplineSpawner").GetComponent<FileWriter>();
        fw.writeDataToFile(results);
        fw.writeDataToFile2(results);
    }
}
