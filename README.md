# Compliance-App-Scripts
This repository contain C# scripts designed to support an experiment conducted to identify perceived compliance drivers in a  Virtual Reality Context.

# Scripting Overview
An object oriented scripting methodology was adopted, hence each function is named to represent the specific operation or computational task it performs.To ensure logical organisation, similar functions were grouped within major script files, with each file dedicated to a distinct aspect of the workflow.

# Compliance measurement approach

Participants who accept the prompt to "go to the gym" further receive an exercise cue that requires them to "click to start" an exercise set or to quit. once selected, the routine is locally stored in *"scriptableObjects/UserData/exerciseTypeOrder"*. The "exerciseTypeOrder" list, stores all details pertaining to the selected excercise including video name, points allocated, status (watched or not). Next, the exercise routine is played on screen for participants to follow. Upon completing the routine, a congratulatory message and the allocated points are displayed on the Gym TV screen temporarily until it is time for the next gym sequence. This process is repeated until all exercise routines are exhausted. Once a participant select exit or completes the exercise routines, The accumulated points calculation, exercise (order, type, category), movie (type, category) and accompanied participant number is stored in a csv file in *"scriptableObjects/UserData/SaveDataToExcel"*. In this scenerio, compliance to exercise is measured according to the number of times a participant presses the "click to start button". It should be noted that, since the virtual reality system was not recording the participants physical orientation, researchers as observers were employed.   

NB: All exercise prompts contained an exit option.


# Folder Structure
  - All files
  - A scriptable Folder
    - scriptable Scripts

# Main Scripts and Functionalities
## GeneralMainSceneUI
This script allows participants to toggle between the room and gym environments.

----  This Page is under development -----
