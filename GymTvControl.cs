using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;



public class GymTvControl : MonoBehaviour
{
    [SerializeField] public GeneralScriptableObj GeneralScriptableObj;
    [SerializeField] public UIControl UIControl;
    [SerializeField] public GameObject GameObjectWithMasterScript;
    [SerializeField] public GameObject GameObjectWithRoomTv;
    
    [SerializeField] private VoiceOverAudio VoiceOverAudio;
    [SerializeField] private bool RoomScene;
    private VideoPlayer videoPlayer;
    private Material tvOriginalProperties;
    private MasterControl masterControl;
    private RoomTv roomTv;
    public int video = 0;
    private bool isMuted = false;
    public bool Called = false;
    public int clicked = 1;

    private float videoLength;
    // Start is called before the first frame update
     void Awake(){
        GetTvProperties();
        GetVideoProperties();
    }
    void Start(){
        if(RoomScene){
            roomTv   =  GameObjectWithRoomTv.GetComponent<RoomTv>();
        }else{
            masterControl =  GameObjectWithMasterScript.GetComponent<MasterControl>();
        }
    }

    private void GetVideoProperties()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoLength = (float)videoPlayer.length;
        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnClipEnd;
    }
    private void GetTvProperties(bool autoStart = false)
    {

        Renderer objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            tvOriginalProperties = objectRenderer.material;
            if (tvOriginalProperties != null)
            {
                if(autoStart){
                    tvOriginalProperties.color = Color.white;
                   
                }
                else{
                    tvOriginalProperties.color = Color.black;
                 
                }
                // tvOriginalProperties.color = Color.black;
            }
            else
            {
                Debug.LogError("No material found on this GameObject's Renderer.");
            }
        }
        else
        {
            Debug.LogError("No Renderer component found on this GameObject.");
        }
    }

    public void StartVideoOnClickFromPanelTest(VideoClip clip){

        if(clip != null){
            GeneralScriptableObj.ChangeGameState(GeneralScriptableObj.ExperimentState.InGymOnSelection.ToString());
            GeneralScriptableObj.reloadUI = true;
            clicked = 1;
            videoPlayer.clip = clip;
            StartVideoOnClick();
        }
        
    }
    public void StartVideoOnClickFromPanel(VideoClip clip){

        GeneralScriptableObj.ChangeGameState(GeneralScriptableObj.ExperimentState.InGymOnSelection.ToString());
        GeneralScriptableObj.reloadUI = true;

        videoPlayer.clip = clip;
        StartVideoOnClick();
        video++;
    }

    public void StartVideoOnClick()
    {
        // Mute the audio if UserExerciseMode is TEXT
        if (GeneralScriptableObj.UserData.UserExerciseMode == GeneralScriptableObj.UserExerciseMode.TEXT.ToString()){
            ToggleMute(true);
        }
        else{
            ToggleMute();
        }


        PlayVidOnTriggerPress();
    }
    
    private void PlayVidOnTriggerPress()
    {
        ResumeVideo();
    }
    private void ToggleMute(bool mute = false){
        if (mute){
            videoPlayer.SetDirectAudioMute(0, mute);
        }else{
            videoPlayer.SetDirectAudioMute(0, mute);
        }
    }

    private void ResumeVideo()
    {
        if(videoPlayer.clip != null){
            videoPlayer.Play();
            tvOriginalProperties.color = Color.white;
        }
    
    }

    public void OnQuitButtonClicked(){
        if(clicked >= 2){
            UIControl.QuitExerciseButton.SetActive(false);
            StartCoroutine(QuitConfirmed());
            clicked = 1;
        }else{
            UIControl.ExerciseScreenCanvasObject.SetActive(false);
        
            if(GeneralScriptableObj.CheckPromptActive()){
                if(RoomScene){
                    UIControl.GeneralRoomSceneUI.DisplayWhenQuitButtonIsClickedTitle.SetText("You have " + CalculateExercisesLeft(GeneralScriptableObj.UserData.exerciseTypeOrder.Count) +" exercises to go. Are you sure you want to quit ? ");
                }else{
                    UIControl.GeneralMainSceneUI.DisplayWhenQuitButtonIsClickedTitle.SetText("You have " + CalculateExercisesLeft(GeneralScriptableObj.UserData.exerciseTypeOrder.Count) +" exercises to go. Are you sure you want to quit ? ");
                }
                UIControl.QuitExerciseButton.SetActive(true);
            }else{
                if(RoomScene){
                    UIControl.GeneralRoomSceneUI.DisplayWhenQuitButtonIsClickedTitle.SetText("");
                }else{
                    UIControl.GeneralMainSceneUI.DisplayWhenQuitButtonIsClickedTitle.SetText("");
                }

                playAudio(GeneralScriptableObj.UserData.exerciseTypeOrder.Count);
                UIControl.QuitExerciseButton.SetActive(true);
            }

            clicked++;
        }
        
    }

    private void playAudio(int number_of_exercise){
        switch(4 - number_of_exercise){
            case 0:
                if(RoomScene){
                    roomTv.GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverAudioPointsQuit(VoiceOverTitles.ZeroMORE.ToString());
                }else{
                    masterControl.GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverAudioPointsQuit(VoiceOverTitles.ZeroMORE.ToString());
                } 
                break;
            case 1:
                if(RoomScene){
                    roomTv.GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverAudioPointsQuit(VoiceOverTitles.OneMORE.ToString());
                }else{
                    masterControl.GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverAudioPointsQuit(VoiceOverTitles.OneMORE.ToString());
                } 
                break;
            case 2:
                if(RoomScene){
                    roomTv.GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverAudioPointsQuit(VoiceOverTitles.TwoMORE.ToString());
                }else{
                    masterControl.GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverAudioPointsQuit(VoiceOverTitles.TwoMORE.ToString());
                } 
                break;
            case 3: 
                if(RoomScene){
                    roomTv.GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverAudioPointsQuit(VoiceOverTitles.ThreeMORE.ToString());
                }else{
                    masterControl.GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverAudioPointsQuit(VoiceOverTitles.ThreeMORE.ToString());
                }
                break; 
            default:
                if(RoomScene){
                    roomTv.GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverAudioPointsQuit(VoiceOverTitles.FourMORE.ToString());
                }else{
                    masterControl.GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverAudioPointsQuit(VoiceOverTitles.FourMORE.ToString());
                }
                break; 
        }
    }
    private int CalculateExercisesLeft(int number_of_exercise){
        switch(4 - number_of_exercise){
            case 0: 
                return 0;
            case 1:
                return 1; 
            case 2: 
                return 2;
            case 3: 
                return 3;
            default: 
                return 4;
        }
    }
    private void OnClipEnd(VideoPlayer vp)
    {   
        StartCoroutine(DisplayPointsToUser());   
    }
    public IEnumerator QuitConfirmed()
    {
        videoPlayer.Stop();
        videoPlayer.clip = null;
        GetTvProperties();
        yield return Quit();
    }
    private IEnumerator DisplayPointsToUser(){
        // Ensure the video player stops and the clip is set to null
        videoPlayer.Stop();
        videoPlayer.clip = null;
        GetTvProperties();

        if (RoomScene){
            UIControl.ExerciseScreenCanvas.GetComponent<UIDynamicControlRoom>().AddExerciseandPoint();
        }else{
            UIControl.ExerciseScreenCanvas.GetComponent<UIDynamicControl>().AddExerciseandPoint();
        }

        if (GeneralScriptableObj.UserData.initialExerciseNode >= 4) {
            yield return Quit();
        } else {
            if(RoomScene){
                roomTv.GotoGym = true;
            }else{
                masterControl.GotoGym = true;
            }
            yield return new WaitForSeconds(0f); // No need to wait if going to the gym
        }

    }
    private IEnumerator Quit(){
        // Show the results canvas
        if (RoomScene){
            UIControl.GeneralRoomSceneUI.ResultsCanvas.ResultsCanvasObject.SetActive(true);
        }else{
            UIControl.GeneralMainSceneUI.ResultsCanvas.ResultsCanvasObject.SetActive(true);
        }

        // Play voice over audio points
        VoiceOverAudio.VoiceOverAudioPoints(GeneralScriptableObj.UserData.exercisePoint);

        // Update points in the results canvas
        if (RoomScene){
            UIControl.GeneralRoomSceneUI.ResultsCanvas.ChangePoints(GeneralScriptableObj.UserData.exercisePoint);
        }else{
            UIControl.GeneralMainSceneUI.ResultsCanvas.ChangePoints(GeneralScriptableObj.UserData.exercisePoint);
        }

        // Update the user's exercise mode
        GeneralScriptableObj.UserData.UserExerciseMode = GeneralScriptableObj.SelectExerciseMode(GeneralScriptableObj.UserData.UserExerciseMode.ToString());
        
        if(RoomScene){
            yield return new WaitUntil(() => roomTv.GeneralEnvironmentTimer.VoiceOverAudio.audioFinished);
        }else{
            yield return new WaitUntil(() => masterControl.GeneralEnvironmentTimer.VoiceOverAudio.audioFinished);
        }

        // Optionally, hide the results canvas after the updates
        if(RoomScene){
            UIControl.GeneralRoomSceneUI.ResultsCanvas.ResultsCanvasObject.SetActive(false);
        }else{
            UIControl.GeneralMainSceneUI.ResultsCanvas.ResultsCanvasObject.SetActive(false);
        }
        // End Movie
        if(RoomScene){
            StartCoroutine(roomTv.EndedMovie());
        }else{
            StartCoroutine(masterControl.EndedMovie());
        }
    }
    void OnDestroy()
    {
        // Unsubscribe from the loopPointReached event to prevent memory leaks
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnClipEnd;
        }
    }
   
}
