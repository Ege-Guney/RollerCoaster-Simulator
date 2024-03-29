using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileWriter : MonoBehaviour
{
    StreamWriter writer; 

    // Start is called before the first frame update
    void Start()
    {
    }

    void openFile(){
        string path = "Assets/ResultingData/Anticipation.txt"; //HEYHEY

        writer = new StreamWriter(path); 
    }

    public void writeDataToFile(Dictionary<Vector3, int> results){
        openFile();
        foreach(var (key,value) in results){
            writer.WriteLine("Current Location:" + key.x + ","+ key.y + "," + key.z + " ; Result : " + value);
        }
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
