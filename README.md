# ROLLER COASTER VISULIZER

Visualizing RollerCoasters created in Roller Coaster Tycoon 2 as a simulation.

## Description

Unity Project to import roller coasters from Roller Coaster Tycoon 2. Has 2 different methods of accessing track unit data. Any .txt file with the correct format can be used to create and simulate the roller coaster. After running, the simulation generates "anticipation data" from every track piece - based on the user experience.

## Getting Started

After cloning the project, edit which script you want to run by commenting out the code from File Reader script.

### Details

Unity splines are used to create the roller coaster.


### Specifics To Edit

Modifiers can be changed to make it more similar to RCT2 in SplineCreator.cs in AdjustCoordinatesV2 function.
```
 xModifier = 20;
 yModifier = 5f;
 zModifier= 20;
```

To change the version used, comment every V2 method and remove comments from V1 methods, including the input files.
