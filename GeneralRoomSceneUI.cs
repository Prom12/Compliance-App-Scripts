using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class GeneralRoomSceneUI : UIControl
{
    [Header("Specific To General Theater Room")]
    [SerializeField] private Button StartButton;
    [SerializeField] public ResultsCanvas ResultsCanvas;
    [SerializeField] public VoiceOverAudio VoiceOverAudio;
    [SerializeField] public GameObject AIModel;
    [SerializeField] public GameObject FinishExperimentMovieRoom;
    
    [SerializeField] public bool CallStart = false;
    [SerializeField] public bool CallOnce = true;

    void Start(){
        // CallStart = true;
        prompt(false);
        activateModel();
        // CallOnce= true;
    }

    void Update() {
        if (CallStart)
        {
          UIControlStart();
          CallStart = false;
        }

        if (GeneralScriptableObj.reloadUI)
        {
            // If it changed, call the function
            SetUI();
            
            // Update the previous state to the current state
           GeneralScriptableObj.reloadUI = false;
        }
    }
     public void UIControlStart(){
        GeneralScriptableObj.reloadUI = true;
        ResultsCanvas.ResultsCanvasObject.SetActive(false);
    }

    public void SetUI(){
        // Clear UI Content
        MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().clearContent();
        
        if(GeneralScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.Initial.ToString())){
            // Clear Exercises
            MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().clearExerciseState();
            GeneralScriptableObj.UserData.ResetUserData();
            // Turn off Other Canvas
            ExerciseScreenCanvasObject.SetActive(false);
            PromptScreenCanvas.SetActive(false);
            QuitExerciseButton.SetActive(false);
            // 
            MovieMainTitle.SetText("Movie List");
            toggleUIMovie();
            MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().UpdateUI();
            if(CallOnce) {
                CallStart = true;
                CallOnce = false;
            }
        }else if(GeneralScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.InMovie.ToString())){
            MainScreenCanvasObject.SetActive(false);
            ExerciseScreenCanvasObject.SetActive(false);
            PromptScreenCanvas.SetActive(false);
        }else if(GeneralScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.InGym.ToString())){
            MainScreenCanvasObject.SetActive(false);
            PromptScreenCanvas.SetActive(false);
            ExerciseMainTitle.SetText("Activity List ("+ GeneralScriptableObj.UserData.exercisePoint + " EP)");
            toggleUIGym();
            ExerciseScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().UpdateUI();
            MainScreenCanvasObject.SetActive(false);
        }else if(GeneralScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.InGymOnSelection.ToString())){
            MainScreenCanvasObject.SetActive(false);
            ExerciseScreenCanvasObject.SetActive(false);
            PromptScreenCanvas.SetActive(false);
            QuitExerciseButton.SetActive(false);
            
        }else{
            ExerciseScreenCanvasObject.SetActive(false);
            MainScreenCanvasObject.SetActive(false);
        }

    }

    public void prompt(bool state) {
        if (GeneralScriptableObj.CheckPromptActive() && state){
            MoviePromptUITitle.SetText("Hey there!!, Did you know that 80 percent of your peers who watched this movie also exercised in the gym? Just a few minutes of exercise can boost your mood and increase your focus for the rest of the day. Ready to Exercise?");
            PromptScreenCanvas.SetActive(true);
        }else if(!GeneralScriptableObj.CheckPromptActive() && state){
            MoviePromptUITitle.SetText("");
            PromptScreenCanvas.SetActive(true);
        }else{
            PromptScreenCanvas.SetActive(false);
        }
    }
    public void activateModel() {
        if (GeneralScriptableObj.CheckModelActive()){
            AIModel.SetActive(true);
        }else{
            AIModel.SetActive(false);
        }
    }
    private void toggleUIMovie(){
        if (GeneralScriptableObj.CheckExperimentTypePermission()){
            StartButton.gameObject.SetActive(false);
            MainScreenCanvasObject.SetActive(true);
            
            VoiceOverAudio.VoiceOverToPlay(VoiceOverTitles.HelloChooseM.ToString());
        }else{
            MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().sendSequentialMovie();
            StartButton.gameObject.SetActive(true);
            MainScreenCanvasObject.SetActive(true);
            VoiceOverAudio.VoiceOverToPlay(VoiceOverTitles.HelloChooseStart.ToString());

        }
    }

    private void toggleUIGym(){
        if (GeneralScriptableObj.CheckExperimentTypePermission()){
            StartButton.gameObject.SetActive(false);
            MainScreenCanvasObject.SetActive(false);
            ExerciseScreenCanvasObject.SetActive(true);
            VoiceOverAudio.VoiceOverToPlay(VoiceOverTitles.ChooseExerToStart.ToString());
        }else{
            ExerciseScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().sendSequentialGym();
            StartButton.gameObject.SetActive(true);
            MainScreenCanvasObject.SetActive(false);
            ExerciseScreenCanvasObject.SetActive(true);
            VoiceOverAudio.VoiceOverToPlay(VoiceOverTitles.ChooseStartToStart.ToString());
        }
    }
}
