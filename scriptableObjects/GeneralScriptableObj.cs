using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public enum VoiceOverTitles {HelloChooseM, HelloChooseStart, HyeeItsExerciseTime,ChooseBnTOptions, ChooseExerToStart, ChooseStartToStart, SayUntil1,SayUntil2,
    SayUntil3,SayUntil4,SayUntil5,SayUntil6,SayUntil7,SayUntil8,SayUntil9,SayUntil10,AreYouSureQuit, HurrayUCompleted,RedirectFew,CompletedExperiment, FourMORE,ThreeMORE,TwoMORE, OneMORE,ZeroMORE};


[Serializable]
public class Movie
{
    public string videoTitle;
    public string category;
    public VideoClip video;

    public Movie(string VideoTitle, VideoClip Video, string Category)
    {
        videoTitle = VideoTitle;
        video = Video;
        category = Category;
    }
}

[Serializable]
public class ExerciseType
{
    public string activityTitle;
    public string activityCategory;
    public VideoClip videoTutorial;
    public bool watchedAlready;
    public int assignedCurrency;

    public ExerciseType(string ActivityTitle, VideoClip VideoTutorial, bool WatchedAlready = false, int AssignedCurrency = 0)
    {
        activityTitle = ActivityTitle;
        videoTutorial = VideoTutorial;
        watchedAlready = WatchedAlready;
        assignedCurrency = AssignedCurrency;
    }
}

[Serializable]
public class ExperimentType
{
    public string experimentTitle;
    public bool state;
    public bool active;
    public bool audio_prompt;
    public bool ai_model;

    public ExperimentType(string ExperimentTitle, bool State, bool Active = false, bool Audio_prompt = false,bool Model = false)
    {
        experimentTitle = ExperimentTitle;
        state = State;
        active = Active;
        audio_prompt = Audio_prompt;
        ai_model = Model;
    }
}

[Serializable]
public class AudioVoiceOvers
{
    public string voiceTitle;
    [Header("Dropdown Options")]
    [SerializeField] public VoiceOverTitles voiceTitless;
    
    public AudioClip clip;
    public bool loopAudio;
    public int points;


    public AudioVoiceOvers(string voiceTitle, AudioClip clip,bool loopAudio,int points = 0)
    {
        this.voiceTitle = voiceTitle;
        this.clip = clip;
        this.loopAudio = loopAudio;
        this.points = points;
    }
}
[CreateAssetMenu(fileName="GeneralScriptableObj", menuName="ScriptableObject/GeneralScriptable")]
public class GeneralScriptableObj : ScriptableObject
{
    [SerializeField] public UserData UserData;

    [Header("Time Frame")]
    public float totalScenePeriodInMins = 30f;
    public float totalScenePeriodAvailableInMins = 0f;

    public enum ExperimentState {Initial, AtDashboard, InMovie, InGym, InGymOnSelection, Final};
    
    public enum UserExerciseMode {TEXT, AUDIO};
    
    [Header("Dynamic Data Categories")]
    public List<ExperimentType> experimentType = new List<ExperimentType>();
    public List<Movie> shortMovieClips = new List<Movie>();
    public List<ExerciseType> exerciseTypes = new List<ExerciseType>();
    public List<AudioVoiceOvers> AudioVoiceOvers = new List<AudioVoiceOvers>();
    public string ActiveScene = "";

    [Header("Dynamic Data State")]
    [SerializeField] private string experimentCurrentState;
    [SerializeField] private ExperimentType experimentCurrentType;
    public bool loadDynamicButtons = false;
    public bool reloadUI = false;
    public void ChangeGameState(string state)
    {
        experimentCurrentState = state;
        
    }
    public bool CheckGameState(string state)
    {
        if(experimentCurrentState == state)
            return true;
        
        return false;
    }
    public void SelectExperimentType(ExperimentType experimentType)
    {
        if(experimentType.state){
            experimentCurrentType = experimentType;
            UserData.experimentCurrentType = experimentType;
        }
    }

    public string SelectExerciseMode(string mode){
        if(mode == UserExerciseMode.AUDIO.ToString()){
            return UserExerciseMode.TEXT.ToString();
        }
        return UserExerciseMode.AUDIO.ToString();
    }

    public bool CheckExperimentTypePermission()
    {
        bool selectMovie = false;
        bool selectExercise = false;
        switch (experimentCurrentType.experimentTitle.ToString())
        {
            case "Control":
                selectMovie = true;
                // Don't select movie or exercise
                break;
            case "Intervention":
                selectMovie = true;
                // Don't select exercise
                break;
            case "Choose Movie and Exercise":
                selectMovie = true;
                selectExercise = true;
                break;
            case "Choose Movie, Exercise and Add Coach":
                selectMovie = true;
                selectExercise = true;
                break;
        }

        // Make the decision for movie based on the current state
        if (experimentCurrentState == ExperimentState.Initial.ToString())
        {
            return MakeMovieDecision(selectMovie);
        
        }

        // Make the decision for exercise based on the current state
        if (experimentCurrentState == ExperimentState.InGym.ToString())
        {
            return MakeExerciseDecision(selectExercise);
        }
        return false;
    }
    public bool MakeMovieDecision(bool SelectMovie = false)
    {
        if(SelectMovie){
            loadDynamicButtons = true;
            return true; 
        }
        loadDynamicButtons = false;
        return false; 
    }

    public bool MakeExerciseDecision(bool SelectExercise = false)
    {
        if(SelectExercise){
            loadDynamicButtons = true;
            return true; 
        }
        loadDynamicButtons = false;
        return false; 
    }
    public bool CheckPromptActive(){
        if(experimentCurrentType.audio_prompt){
            return true;
        }
        return false;
    }
    public bool CheckModelActive(){
        if(experimentCurrentType.ai_model){
            return true;
        }
        return false;
    }
    public void resetScriptable(){
        totalScenePeriodAvailableInMins = 0f;
        foreach (ExerciseType exercise in exerciseTypes){
            exercise.watchedAlready = false;
        }
        experimentCurrentState = ExperimentState.Initial.ToString();
        reloadUI = true;
    }

}

   

