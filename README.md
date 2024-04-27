# ROLLER COASTER VISUALIZER

Visualizing RollerCoasters created in Roller Coaster Tycoon 2 as a simulation on UNITY.

## Description

Unity Project to import roller coasters from Roller Coaster Tycoon 2. Has 2 different methods of accessing track unit data. Any .txt file with the correct format can be used to create and simulate the roller coaster. After running, the simulation generates "anticipation data" from every track piece - based on the user experience.

FURTHER EXPLANATION CAN BE FOUND IN THE REPORT
<br />
[Roller_Coaster_Project_Report.pdf](https://github.com/Ege-Guney/RollerCoaster-Simulator/files/15139512/Roller_Coaster_Project_Report.pdf)

## Getting Started

After cloning the project, edit which script you want to run by commenting out the code from File Reader script.
Then run the simulation with Unity Play button to see the results.
<img width="723" alt="Screenshot 2024-04-27 at 14 48 02" src="https://github.com/Ege-Guney/RollerCoaster-Simulator/assets/119132789/025beb0a-7a1b-47ac-9101-b6cdbe19c416">

## Creating Track

<img width="274" alt="Screenshot 2024-04-27 at 14 42 47" src="https://github.com/Ege-Guney/RollerCoaster-Simulator/assets/119132789/9f2d6a38-3c67-4012-82fd-64b1a006d97d">
<br />
As seen in the picture, Track creation is started with "Create Spline" button.
Manual Track editing and the animation could be played with after the successful creation.

## Retrieving Anticipation Data

After Creating the track, we can bake the instances from within the track to make all trackunit game objects: inside the SplineTrack Object in Spline Instantiation Script.
Data will be visible after playing the simulation under Resulting Data folder in Assets.
Format: Location: (x1,y1,z1); Results: (x2,y2,z2);...
Result explanation can be found in the report.

### Dependencies

- Install Unity Version (2022.3.9) for best results.
- Import Unity Splines https://docs.unity3d.com/Packages/com.unity.splines@2.2/manual/index.html


### Specifics To Edit

Modifiers can be changed to make it more similar to RCT2 in SplineCreator.cs in AdjustCoordinatesV2 function.

```
 xModifier = 20;
 yModifier = 5f;
 zModifier= 20;
```

To change the version used, comment every V2 method and remove comments from V1 methods, including the input files.
