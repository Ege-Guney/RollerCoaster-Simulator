using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class FileReader : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [MenuItem("Tools/CreateTrack")]
    static void ReadString()
    {
        TrackCreator tc = GameObject.Find("TrackSpawner").GetComponent<TrackCreator>();
        tc.currentHeight = 0f;
        string path = "Assets/ScriptDependencies/track.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); 
        while (reader.Peek() >= 0)
        {
            string line = reader.ReadLine();
            char[] separators = new char[] {' ', ',' ,'<','>'};
            string[] formatted = line.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
            
        // Debug.Log(formatted[0] + formatted[1] + formatted[2] + formatted[3]);
            
            tc.handleTrackInput(formatted[0], -Int32.Parse(formatted[2]), Int32.Parse(formatted[3]), Int32.Parse(formatted[1]), Int32.Parse(formatted[4]));
        }
        
        //height and 
        //Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
