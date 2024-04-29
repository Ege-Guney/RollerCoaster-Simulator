# Roller Coaster Visualizer

A Unity project for visualizing roller coasters created in Roller Coaster Tycoon 2 as a simulation.

## Description

This Unity project is designed to import roller coasters from Roller Coaster Tycoon 2. It offers two different methods for accessing track unit data. Any .txt file with the correct format can be used to create and simulate the roller coaster. After running the simulation, "anticipation data" is generated from every track piece, based on the user experience.

For further details, please refer to the [Roller Coaster Project Report](https://github.com/Ege-Guney/RollerCoaster-Simulator/files/15139512/Roller_Coaster_Project_Report.pdf).

## Getting Started

After cloning the project, select the script you want to run by commenting out the code from the File Reader script. Then run the simulation using the Unity Play button to observe the results.

![Screenshot](https://github.com/Ege-Guney/RollerCoaster-Simulator/assets/119132789/025beb0a-7a1b-47ac-9101-b6cdbe19c416)

## Creating Track

![Track Creation](https://github.com/Ege-Guney/RollerCoaster-Simulator/assets/119132789/9f2d6a38-3c67-4012-82fd-64b1a006d97d)

Track creation begins with the "Create Spline" button. After successful creation, manual track editing and animation playback are available.

## Retrieving Anticipation Data

After creating the track, instances can be baked from within the track to generate all track unit game objects inside the SplineTrack Object in the Spline Instantiation Script. The resulting data can be found under the Resulting Data folder in Assets after playing the simulation. The format is as follows: Location: (x1, y1, z1); Results: (x2, y2, z2);... For a detailed explanation of the results, please consult the report.

### Dependencies

- Unity Version 2022.3.9 for optimal performance.
- Unity Splines package, available [here](https://docs.unity3d.com/Packages/com.unity.splines@2.2/manual/index.html).

### Specifics to Edit

To adjust modifiers and make it more similar to RCT2 in the SplineCreator.cs file, modify the AdjustCoordinatesV2 function as follows:

```csharp
xModifier = 20;
yModifier = 5f;
zModifier = 20;
```

To change the version used, comment every V2 method and remove comments from V1 methods, including the input files.
