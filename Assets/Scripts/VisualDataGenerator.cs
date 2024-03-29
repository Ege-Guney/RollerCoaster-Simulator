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
    Dictionary<Vector3, int> results;
    
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
        //figure out the closest knot index!
        Vector3 futurePos = transform.position;
        if(cur_index + 1 < locationList.Count){
            DataPoint<float4> unit1 = locationList[cur_index];
            DataPoint<float4> unit2 = locationList[cur_index + 1];
            Vector3 v1 = new Vector3(unit1.Value.x, unit1.Value.y, unit1.Value.z);
            Vector3 v2 = new Vector3(unit2.Value.x, unit2.Value.y, unit2.Value.z);
            if(Vector3.Distance(v2, futurePos) <= Vector3.Distance(v1,futurePos)){
                cur_index++;
                dataAcquired = false;
            }
        }
        //Debug.Log("INDEX: " + cur_index);
    }

    int findAnticipationData(){
        int result = 0;
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
                result++;
            }
        }
        return result;
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
        results = new Dictionary<Vector3, int>();
        dataSent = false;
        splineObject = GameObject.Find("SplineTrack");
    }

    // Update is called once per frame
    void Update()
    {
        
            if(cur_index < spline.Count - 1){
                setClosestLocationIndex();
                if(!dataAcquired){
                    int res = findAnticipationData();
                    //write to file...
                    Debug.Log("Writing..." + res);
                    results.Add(this.transform.position, res);
                    dataAcquired = true;
                }
            }
    }

    void OnApplicationQuit()//Now that we are quiting the application we can write our data to a file
    {
        FileWriter fw = GameObject.Find("SplineSpawner").GetComponent<FileWriter>();
        fw.writeDataToFile(results);
    }
}
