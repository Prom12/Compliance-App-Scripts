using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class RoomTv : MonoBehaviour
{
   [Header("Empty Camera Object Location")]
    [SerializeField] private GameObject OfficeEmptyObject;
    [SerializeField] private GameObject GymEmptyObject;
   
    private VideoPlayer videoPlayer;
    private Material tvOriginalProperties;
    private bool startVideo = false;

    [Header("Other")]
    public TwoMTimerController TwoMTimerController;
    public GeneralEnvironmentTimer GeneralEnvironmentTimer;
    public GameObject OfficeCamera;
    public GeneralScriptableObj generalScriptableObj;
    public UIControl UIControl;
    public AI AI;
    [SerializeField] private GymTvControl GymTvControl;

    private Vector3 officeLocation = new Vector3(3.04f, 1.643f, 3.492f);
    private Vector3 officeRotation = new Vector3(0f, -92.63f, 0f);
    private Vector3 gymLocation = new Vector3(1.86f, 1.47f, 19.17f);
    private Vector3 gymRotation = new Vector3(-13.6f, -92.63f, 0f);
    private bool Office = true;

    private bool isPaused = false;
    private float videoLength;
    private bool runRandom = false;
    private bool toggleOffice = true;
    private int repeatCount = 0;
    private int maxRepeats = 4;
    public bool GotoGym = false;
    // public int no_of_prompts_left = 4;
    public bool continueToNextPrompt = false;

    private void Start()
    { 
        MasterControlStart();
    }
    public void MasterControlStart(){
        officeLocation = OfficeEmptyObject.GetComponent<Transform>().position;
        officeRotation = OfficeEmptyObject.GetComponent<Transform>().eulerAngles;
        gymLocation = GymEmptyObject.GetComponent<Transform>().position;
        gymRotation = GymEmptyObject.GetComponent<Transform>().eulerAngles;
      
        if (TwoMTimerController == null)
        {
            Debug.Log("TimerController reference is missing. Attach it in the Inspector.");
        }
    }
    void Awake(){
        GetTvProperties();
        GetVideoProperties();
    }
    private void Update()
    {
        if(generalScriptableObj.UserData.resetMasterControl || GeneralEnvironmentTimer.TimerHasEnded){

            videoPlayer.clip = null;
            runRandom = false;
            TwoMTimerController.HasStopped = false;
            // TwoMTimerController.isTimerRunning = false;
            // TwoMTimerController.StopTimer();
            UIControl.GeneralRoomSceneUI.CallStart = true;
            // UIControl.MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().initialExerciseNode = 0;
            GeneralEnvironmentTimer.endUserSession();
            generalScriptableObj.UserData.resetMasterControl = false;
        }
        
        if(GotoGym){
            OndoSomethingButtonClick();
            GotoGym = false;
        }
        HandleUserInput();
        HandleRandomRequests();
    }
    private void HandleUserInput()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // StartVideoOnClick();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnContinueButtonClick();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            OndoSomethingButtonClick();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            toggleScenes();
        }
    }
    public void StartVideoOnClickFromPanel(VideoClip clip){
        videoPlayer.clip = clip;
        GetTvProperties(true);
        StartVideoOnClick();
        
    }
    public void StartVideoOnClick()
    {
        
        generalScriptableObj.ChangeGameState(GeneralScriptableObj.ExperimentState.InMovie.ToString());
        UIControl.GeneralRoomSceneUI.SetUI();
        if(generalScriptableObj.CheckModelActive()){
            AI.StartAnimation();
            AI.IsSitting();
        }
        if (!startVideo)
        {
            PlayVidOnTriggerPress();
            TwoMTimerController.StartTimer();
            startVideo = true;
        }
        else
        {
            PlayVidOnTriggerPress();
            // TwoMTimerController.PausePlayTimer(isPaused);
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
    private void PlayVidOnTriggerPress()
    {
        if (videoPlayer.isPlaying)
        {
            PauseVideo();
        }
        else
        {
            ResumeVideo();
        }
    }
    private void PauseVideo()
    {
        isPaused = true;
        videoPlayer.Pause();
        if(tvOriginalProperties != null){
        tvOriginalProperties.color = Color.black;

        }
    }
    private void ResumeVideo()
    {
        isPaused = false;
        videoPlayer.Play();
         if(tvOriginalProperties != null){

        tvOriginalProperties.color = Color.white; 

        }
    }
    public void OnContinueButtonClick()
    {
        isPaused = false;
                generalScriptableObj.UserData.initialExerciseNode++;

        // UIControl.MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().IncreaseinitialExerciseNode();     

    }
    public void OndoSomethingButtonClick()
    {
        // UIControl.MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().initialExerciseNode = UIControl.MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().initialExerciseNode - 1;
        // no_of_prompts_left = UIControl.MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControlRoom>().initialExerciseNode;
                generalScriptableObj.UserData.initialExerciseNode++;

        StartCoroutine(DoSomethingCoroutine());
    }
    public void SkipScene(bool toggleOffice = false)
    {
        toggleScenes(toggleOffice);
    }
    private void toggleScenes(bool toggleOffice = false){
        if(toggleOffice){
            changeLocation();
        }else{
            changeLocation(false);
        }
    } 
    private IEnumerator DoSomethingCoroutine()
    {

        changeLocation(Office = false);
        UIControl.GeneralRoomSceneUI.prompt(false);
        UIControl.GeneralRoomSceneUI.MainScreenCanvasObject.SetActive(false);
        // Coroutine Starting
        yield return new WaitUntil(()=>GymTvControl.Called);

        if (repeatCount < maxRepeats)
        {
            OndoSomethingButtonClick();
        }
        // Coroutine Ending
    }
    private void  changeLocation(bool Office = true){
      if (Office){
        generalScriptableObj.ChangeGameState(GeneralScriptableObj.ExperimentState.InMovie.ToString());
        OfficeCamera.transform.position = officeLocation;
        OfficeCamera.transform.rotation = Quaternion.Euler(officeRotation);
      }else{
        OfficeCamera.transform.position = gymLocation;
        OfficeCamera.transform.rotation = Quaternion.Euler(gymRotation);
        generalScriptableObj.ChangeGameState(GeneralScriptableObj.ExperimentState.InGym.ToString());
        // GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverToPlay(VoiceOverTitles.ChooseExerToStart.ToString());

      }    
    generalScriptableObj.reloadUI = true;
    }
    private void HandleRandomRequests()
    {
        if (TwoMTimerController.HasStopped && !runRandom)
        {
            StartCoroutine(StartRandomRequest());
            continueToNextPrompt = true;
            runRandom = true;
        }
    }
    private IEnumerator StartRandomRequest()
    {
        while (generalScriptableObj.UserData.initialExerciseNode < generalScriptableObj.exerciseTypes.Count)
        {
            // Number of time it should run
            if(!(generalScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.InGym.ToString()) || generalScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.InGymOnSelection.ToString())) && continueToNextPrompt){
                // generalScriptableObj.UserData.initialExerciseNode++;
                PauseVideo();
                UIControl.GeneralRoomSceneUI.DisplayWhenQuitButtonIsClickedTitle.SetText("");
                UIControl.GeneralRoomSceneUI.prompt(true);
                if(generalScriptableObj.CheckModelActive()){
                    AI.IsSitTalking();
                }
                if(!generalScriptableObj.CheckPromptActive()){
                    GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverToPlay(VoiceOverTitles.HyeeItsExerciseTime.ToString());
                }
            }
            yield return new WaitUntil(() => !isPaused);
            if(generalScriptableObj.CheckModelActive()){
                AI.IsSitting();
            }
            if (!isPaused)
            {
                if(generalScriptableObj.UserData.initialExerciseNode == 4){
                    StartCoroutine(Premovieended());
                    StartCoroutine(EndedMovie());
                }
                GeneralEnvironmentTimer.VoiceOverAudio.StopAudio();
                UIControl.GeneralRoomSceneUI.prompt(false);
                ResumeVideo();
            }
            GymTvControl.Called = false;
            
            yield return new WaitForSeconds(GetWaitTime(generalScriptableObj.UserData.initialExerciseNode));
        }
    }

    private float GetWaitTime(int count)
    {
        switch (count)
        {
            case 3: return 120f; // 2 minutes
            case 2: return 180f; // 3 minutes
            case 1: return 60f;  // 1 minute
            default: return 0f;
        }
    }

    public IEnumerator Premovieended(){
        UIControl.GeneralRoomSceneUI.prompt(false);
        UIControl.GeneralRoomSceneUI.FinishExperimentMovieRoom.SetActive(true);
        yield return new WaitForSeconds(10f);
        UIControl.GeneralRoomSceneUI.FinishExperimentMovieRoom.SetActive(false);
    }


    
    public void OnClipEnd(VideoPlayer vp)
    {
        StartCoroutine(EndedMovie());
    }
    public IEnumerator EndedMovie()
    {   
        startVideo = false;
        // no_of_prompts_left = 4;
        continueToNextPrompt = false;
        TwoMTimerController.HasStopped = false;
        // TvContentNavButtons.SetActive(false);
        videoPlayer.clip = null;
        GetTvProperties();
        
        SkipScene(true);
        if(generalScriptableObj.CheckModelActive()){
            AI.StopAnimation();
        }
        GeneralEnvironmentTimer.VoiceOverAudio.VoiceOverToPlay(VoiceOverTitles.CompletedExperiment.ToString());
        yield return new WaitUntil(() => GeneralEnvironmentTimer.VoiceOverAudio.audioFinished);
        generalScriptableObj.ChangeGameState(GeneralScriptableObj.ExperimentState.Initial.ToString());
        generalScriptableObj.UserData.resetMasterControl = true;
        generalScriptableObj.reloadUI = true;
        generalScriptableObj.UserData.initialExerciseNode = 0;     
        TwoMTimerController.StopTimer();
  

        // StopCoroutine(DoSomethingCoroutine());
        // StopCoroutine(StartRandomRequest());
        StopAllCoroutines();
    }
}
