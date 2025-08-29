# Compliance-App-Scripts
This repository contain C# scripts that were designed to support an experiment conducted to identify compliance of participant to exercise using visual or audio feedback.

# Scripting Overview
An object oriented scripting methodology was adopted, hence each function is named to represent the specific operation or computational task it performs.To ensure logical organisation, similar functions were grouped within major script files, with each file dedicated to a distinct aspect of the workflow.

# Compliance measurement approach

Depending on participant experimental category (Intervention or Control), Participants who agree to "Go to the Gym" are shown an initial prompt that requires willing participants to "click to start" a preselected exercise routine, once selected, the routine is locally stored in *"scriptableObjects/UserData/exerciseTypeOrder"*. The "exerciseTypeOrder" list stores all details pertaining to the selected excercises including video name, points allocated, status (watched or not). Next, the exercise routine is played on screen for participants to follow. Upon completing the routine, a congratulatory message and the allocated points are displayed on the Gym TV screen temporarily until it is time for the next gym sequence. This process is repeated until all exercise routines are exhausted. Once a participant select exit or completes the exercise routines, The accumulated points calculation, exercise (order, type, category), movie (type, category) and accompanied participant number is stored in a csv file in *"scriptableObjects/UserData/SaveDataToExcel"*. In this scenerio, compliance to exercise is measured according to the number of times a participant presses the "click to start button". It should be noted that, since the virtual reality system was not recording the participants physical orientation, researchers as observers were employed.   

NB: All exercise prompts contained and exit option.


# Folder Structure
  - All files
  - A scriptable Folder
    - scriptable Scripts

# Main Scripts and Functionalities
## GeneralMainSceneUI
This activate and toggle between the room and gym environments.

----  This Page is under development -----
