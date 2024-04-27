using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileWriter : MonoBehaviour
{
    StreamWriter writer;
    StreamWriter writer2;


    // Start is called before the first frame update
    void Start()
    {
    }

    void openFile(){
        string path = "Assets/ResultingData/Anticipation.txt"; //HEYHEY
        
        writer = new StreamWriter(path); 
    }

    void openFile2(){
        string path2 = "Assets/ResultingData/AnticipationNumber.txt";
        writer2 = new StreamWriter(path2); 
    }
    public void writeDataToFile(Dictionary<Vector3, List<Vector3>> results){
        openFile();
        foreach(var (key,value) in results){
            writer.Write("Current Location: " + key + " Result : ");
            foreach(var val in value){
                writer.Write(val + ";");
            }
            writer.Write("\n");
        }
        writer.Close();
    }

    public void writeDataToFile2(Dictionary<Vector3, List<Vector3>> results){
        openFile2();
        foreach(var (key,value) in results){
            writer2.Write("Current Location: " + key + " Result : " + value.Count);
            writer2.Write("\n");
        }
        writer2.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
