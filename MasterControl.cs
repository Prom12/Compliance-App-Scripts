
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class MasterControl : MonoBehaviour
{
    [Header("Empty Camera Object Location")]
    [SerializeField] private GameObject OfficeEmptyObject;
    [SerializeField] private GameObject GymEmptyObject;
   
    private VideoPlayer videoPlayer;
    private Material tvOriginalProperties;
    private bool startVideo = false;
    
    [Header("Control Video and Location Movement")]
    public GameObject yesButton;
    public GameObject continueButton;
    public GameObject playPauseButton;
    public GameObject TvContentNavButtons;


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
            TwoMTimerController.StopTimer();
            UIControl.GeneralMainSceneUI.CallStart = true;
            UIControl.MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControl>().initialExerciseNode = 0;
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
        UIControl.GeneralMainSceneUI.SetUI();
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
        // UIControl.MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControl>().IncreaseinitialExerciseNode();     

    }
    public void OndoSomethingButtonClick()
    {
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
        // toggleOffice = !toggleOffice;
    } 
    private IEnumerator DoSomethingCoroutine()
    {

        changeLocation(Office = false);
        UIControl.GeneralMainSceneUI.prompt(false);
        UIControl.GeneralMainSceneUI.MainScreenCanvasObject.SetActive(false);
        // Coroutine Starting
        yield return new WaitUntil(()=>GymTvControl.Called);
        // OndoSomethingButtonClick();

        if (repeatCount < maxRepeats)
        {
            OndoSomethingButtonClick();
        }
        // Coroutine Ending
        // isPaused = false;
        // changeLocation();
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
            runRandom = true;
        }
    }
    private IEnumerator StartRandomRequest()
    {       
        int count = generalScriptableObj.exerciseTypes.Count;
        while (count > 0)
        {
            if(!(generalScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.InGym.ToString()) || generalScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.InGymOnSelection.ToString()))){

            // Number of time it should run
                count--;
                

                PauseVideo();
                // yesButton.SetActive(true);
                // continueButton.SetActive(true);
                // TvContentNavButtons.SetActive(true);
                if(generalScriptableObj.CheckModelActive()){
                    AI.IsSitTalking();
                }
                UIControl.GeneralMainSceneUI.prompt(true);
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
                GeneralEnvironmentTimer.VoiceOverAudio.StopAudio();
                // yesButton.SetActive(false);
                // continueButton.SetActive(false);
                // TvContentNavButtons.SetActive(false);
                UIControl.GeneralMainSceneUI.prompt(false);
                ResumeVideo();
            }
            GymTvControl.Called = false;
            if(count == 3){
                yield return new WaitForSeconds(120f); // 2 mins in sec
                
            }
            else if(count == 2){
                yield return new WaitForSeconds(180f); // 3 mins in sec
                
            }
            else if(count == 1){
                yield return new WaitForSeconds(60f);// 1min in sec
                
            }
           

        }
    }
    public void OnClipEnd(VideoPlayer vp)
    {
        StartCoroutine(EndedMovie());
    }
    public IEnumerator EndedMovie()
    {   
        TvContentNavButtons.SetActive(false);
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
        UIControl.MainScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControl>().initialExerciseNode = 0;     
        StopCoroutine(DoSomethingCoroutine());
        StopCoroutine(StartRandomRequest());
        TwoMTimerController.StopTimer();

    }
}