using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class FileReaderSpline : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private static TrackUnit.Track createTrackV1(string[] formatted){
        string bank = "NONE";
        string formatted_Bank = formatted[8];
        if(formatted_Bank.Contains("LEFT")){
            bank = "LEFT";
        }
        else if(formatted_Bank.Contains("RIGHT")){
            bank = "RIGHT";
        }
        TrackUnit.Track tUnit = new TrackUnit.Track(
                formatted[0], 
                new Vector3(Int32.Parse(formatted[1]), Int32.Parse(formatted[3]), Int32.Parse(formatted[2])),
                new Vector3(0,0,0),
                Int32.Parse(formatted[4]),
                formatted[5],
                formatted[6], //6-7 slopes
                bank, //8-9 banks
                Int32.Parse(formatted[10]),
                Int32.Parse(formatted[11]),
                Int32.Parse(formatted[12])
                );
        return tUnit;
    }
    private static TrackUnit.Track createTrackV2(string[] formatted){
        string bank = "NONE";
        string formatted_Bank = formatted[7];
        if(formatted_Bank.Contains("LEFT")){
            bank = "LEFT";
        }
        else if(formatted_Bank.Contains("RIGHT")){
            bank = "RIGHT";
        }
        TrackUnit.Track tUnit = new TrackUnit.Track(
                formatted[0], 
                new Vector3(Int32.Parse(formatted[1]), Int32.Parse(formatted[3]), Int32.Parse(formatted[2])),
                new Vector3(0,0,0),
                0,
                formatted[4],
                formatted[5], //6-7 slopes
                bank, //8-9 banks
                Int32.Parse(formatted[9]),
                Int32.Parse(formatted[10]),
                Int32.Parse(formatted[11])
                );
        return tUnit;
    }
    [MenuItem("Tools/CreateSpline")]
    static void ReadString()
    {
        SplineCreator tc = GameObject.Find("SplineSpawner").GetComponent<SplineCreator>();
        //V1
        //string path = "Assets/ScriptDependencies/test_tracks/curves.txt"; //CURVES
        //string path = "Assets/ScriptDependencies/test_tracks/loop.txt"; //LOOP
        //string path = "Assets/ScriptDependencies/test_tracks/loop_downcurve.txt"; //LOOP_DOWNCURVE
        //string path = "Assets/ScriptDependencies/test_tracks/loop_two_hills.txt"; //LOOP_TWO_HILLs
        //string path = "Assets/ScriptDependencies/test_tracks/diag.txt"; //diag
        //string path = "Assets/ScriptDependencies/test_tracks/diag2.txt"; //diag2
        //string path = "Assets/ScriptDependencies/test_tracks/Hey! Hey!.txt"; //HEYHEY

        //V2
        //string path = "Assets/ScriptDependencies/newtest_tracks/curves.txt"; //CURVES
        //string path = "Assets/ScriptDependencies/newtest_tracks/loop.txt"; //LOOP
        //string path = "Assets/ScriptDependencies/newtest_tracks/loop_downcurve.txt"; //LOOP_DOWNCURVE
        //string path = "Assets/ScriptDependencies/newtest_tracks/loop_two_hills.txt"; //LOOP_TWO_HILLs
        //string path = "Assets/ScriptDependencies/newtest_tracks/diag.txt"; //diag
        //string path = "Assets/ScriptDependencies/newtest_tracks/diag2.txt"; //diag2
        string path = "Assets/ScriptDependencies/newtest_tracks/Hey! Hey!.txt"; //HEYHEY

        StreamReader reader = new StreamReader(path); 
        tc.createSplineContainer();
        while (reader.Peek() >= 0)
        {
            string line = reader.ReadLine();
            char[] separators = new char[] {' ', ',' ,'<','>'};
            string[] formatted = line.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
            
            //Debug.Log(formatted[0] + formatted[1] + formatted[2] + formatted[3]);
            //TrackUnit.Track tUnit = createTrackV1(formatted);
            TrackUnit.Track tUnit = createTrackV2(formatted);

            tc.handleTrackUnitObject(tUnit);
            //tc.handleTrackInput(formatted[0], -Int32.Parse(formatted[2]), Int32.Parse(formatted[3]), Int32.Parse(formatted[1]), Int32.Parse(formatted[4]));
            
        }
        //tc.createTrackV1();
        tc.createTrackV2(); //calls from tc
        //height and 
        tc.InstantiateTrack();
        //Debug.Log(reader.ReadToEnd());
        GenerateCoasterExternalities gce = GameObject.Find("SplineSpawner").GetComponent<GenerateCoasterExternalities>();
        gce.startCreation();
        reader.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
