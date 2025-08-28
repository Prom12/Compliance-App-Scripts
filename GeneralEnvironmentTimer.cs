using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneralEnvironmentTimer : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GeneralScriptableObj GeneralScriptableObj;
    [SerializeField] private UserData UserData;
    [SerializeField] public VoiceOverAudio VoiceOverAudio;
    public float timerDuration;
    [SerializeField] private bool GeneralTimerStarted = false;
    private float currentTime = 0f;
    public bool TimerHasEnded = false;
    
    // Start is called before the first frame update
    void Start(){
        UserData.Create_Csv_File();
        OnUserEntry();
    }
    
    public void OnUserEntry(){
        UserData.CreateUser();
        TimerHasEnded = false;
        timerDuration = GeneralScriptableObj.totalScenePeriodInMins * 60f;
        GeneralScriptableObj.totalScenePeriodAvailableInMins = GeneralScriptableObj.totalScenePeriodInMins;
        GeneralScriptableObj.ChangeGameState(GeneralScriptableObj.ExperimentState.Initial.ToString());
        GeneralScriptableObj.reloadUI = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GeneralTimerStarted){
            // Update the timer
            currentTime += Time.deltaTime;
            GeneralScriptableObj.totalScenePeriodAvailableInMins = (timerDuration - currentTime) / 60f;
            // Check if the timer has reached its duration
            if (GeneralScriptableObj.totalScenePeriodAvailableInMins <=  0.1f){
                // if timer stops, clear the room something is wrong here
                TimerHasEnded = true;

                // endUserSession();
            }
        }
    }
    public void toggleTimer(bool GeneralTimerStarted){
        this.GeneralTimerStarted = GeneralTimerStarted;
    }


    public void endUserSession(){
        currentTime = 0;
        GeneralTimerStarted = false;
        UserData.ClearUserData();
        GeneralScriptableObj.resetScriptable();
        OnUserEntry();
        GeneralScriptableObj.UserData.resetMasterControl = true;
         // Timer is complete
        Debug.Log("Main Timer Completed!");
    }
}
