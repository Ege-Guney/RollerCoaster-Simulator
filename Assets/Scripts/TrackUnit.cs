using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackUnit : MonoBehaviour
{
    public class Track{

        private string trackType; //type of track -- name
        private Vector3 coordinates; //coordinates in txt file
        private Vector3 adjustedCoordinates; //coordinates in unity
        private int direction; //direction in game - rct2
        private string trackClassification; //type of track -- track_flat
        private string trackSlope; 
        private string trackBank; //left or right

        private int speed;
        private int lateralGForce;
        private int verticalGForce;

        public Track(string type, Vector3 coord, Vector3 adjusted, int dir, string clas, string slope, string bank, int spd, int lat,int vert){
            this.trackType = type;
            this.coordinates = coord;
            this.adjustedCoordinates = adjusted;
            this.direction = dir;
            this.trackClassification = clas;
            this.trackSlope = slope;
            this.trackBank = bank;
            this.speed = spd;
            this.lateralGForce = lat;
            this.verticalGForce = vert;
        }


        public string TrackType
        {
            get { return trackType; }
            set { trackType = value; }
        }

        public Vector3 Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }

        public Vector3 AdjustedCoordinates
        {
            get { return adjustedCoordinates; }
            set { adjustedCoordinates = value; }
        }

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public string TrackClassification
        {
            get { return trackClassification; }
            set { trackClassification = value; }
        }

        public string TrackSlope
        {
            get { return trackSlope; }
            set { trackSlope = value; }
        }

        public string TrackBank
        {
            get { return trackBank; }
            set { trackBank = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int LateralGForce
        {
            get { return lateralGForce; }
            set { lateralGForce = value; }
        }

        public int VerticalGForce
        {
            get { return verticalGForce; }
            set { verticalGForce = value; }
        }
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
