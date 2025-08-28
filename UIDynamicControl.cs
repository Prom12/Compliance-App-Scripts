using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIDynamicControl : MonoBehaviour
{
   [SerializeField] private Transform scrollviewContent;
   [SerializeField] private GameObject ButtonsToDuplicate;
   [SerializeField] private GeneralScriptableObj generalScriptableObj;
   [SerializeField] private UserData UserData;
   [SerializeField] private MasterControl Tv;
   [SerializeField] private GymTvControl TvGym;
   [SerializeField] private Button StartButton;
   [SerializeField] private Button GymSecondStartButton;
   [SerializeField] private UIControl UIControl;
   [SerializeField] private GeneralMainSceneUI GeneralMainSceneUI;
   

   [SerializeField] private List<Movie> MoviePool;
   [SerializeField] private List<ExerciseType> GymPool;
   public int initialExerciseNode = 0;
    public ExerciseType tempSelected;
    public int activeIndex;

    public void UpdateUI(){
        if(generalScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.Initial.ToString())){
            MovieUIMainUpdate();
        }else if(generalScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.InGym.ToString())){
            ActivityUIMainUpdate();
        }else if(generalScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.AtDashboard.ToString())){
            ExperimentTypeUIMainUpdate();
        }
    }

    private void ExperimentTypeUIMainUpdate(){
        
        // clear any default state
        clearExperimentState();
        foreach (ExperimentType experimentType in generalScriptableObj.experimentType){
            if (experimentType.active) {
                GameObject newExperimentType = Instantiate(ButtonsToDuplicate, scrollviewContent);
                newExperimentType.GetComponent<ButtonText>().changeText(experimentType.experimentTitle);
                newExperimentType.GetComponent<Button>().onClick.RemoveAllListeners();
                newExperimentType.GetComponent<Button>().onClick.AddListener(() => navigateToMainScene(experimentType));
            }
        }
    }

    private void MovieUIMainUpdate(){
        foreach (Movie movie in generalScriptableObj.shortMovieClips){
            if(generalScriptableObj.loadDynamicButtons){
                GameObject newMovie = Instantiate(ButtonsToDuplicate, scrollviewContent);
                newMovie.GetComponent<ButtonText>().changeText(movie.videoTitle);
                newMovie.GetComponent<Button>().onClick.RemoveAllListeners();
                newMovie.GetComponent<Button>().onClick.AddListener(() => sendMovie(movie));
            }
        }
    }
    
    private void ActivityUIMainUpdate(){
        int index = 0;
        foreach (ExerciseType exercise in generalScriptableObj.exerciseTypes){
            int currentIndex = index;
            if(generalScriptableObj.loadDynamicButtons && !exercise.watchedAlready){
                GameObject newExercise = Instantiate(ButtonsToDuplicate, scrollviewContent);
                newExercise.GetComponent<ButtonText>().changeText(exercise.activityTitle + " (" + exercise.assignedCurrency + " EP)");
                newExercise.GetComponent<Button>().onClick.RemoveAllListeners();
                newExercise.GetComponent<Button>().onClick.AddListener(() => sendExerciseVideo(exercise, newExercise,currentIndex));
            }
            index++;
        }
    }

    public void sendSequentialMovie(){
        int randomIndex = Random.Range(0, generalScriptableObj.shortMovieClips.Count);
        UserData.shortMovieClip = generalScriptableObj.shortMovieClips[randomIndex];
        StartButton.onClick.RemoveAllListeners();
        int currentIndex = randomIndex;
        StartButton.onClick.AddListener(() => {

            GeneralMainSceneUI.VoiceOverAudio.StopAudio();
            Tv.StartVideoOnClickFromPanel(generalScriptableObj.shortMovieClips[currentIndex].video);
        });
    }

    // public void sendSequentialGym()
    // {
    //     int sequenceIndex = selectActiveSequence();
    //     if (sequenceIndex < 0 || sequenceIndex >= generalScriptableObj.exerciseTypes.Count)
    //     {
    //         Debug.LogWarning("Selected sequence index is out of range.");
    //         return;
    //     }

    //     ExerciseType selectedExercise = generalScriptableObj.exerciseTypes[sequenceIndex];
    //     // Disable or remove the selected exercise
    //     selectedExercise.watchedAlready = true;
    //     StartButton.onClick.RemoveAllListeners();

    //     if (selectedExercise.videoTutorial != null && TvGym != null)
    //     {
    //         StartButton.onClick.AddListener(() => TvGym.StartVideoOnClickFromPanelTest(selectedExercise.videoTutorial));
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Trying to use a null video clip. Handle appropriately.");
    //     }

    //     if (selectedExercise.videoTutorial != null)
    //     {
    //         UserData.exerciseTypeOrder.Add(selectedExercise);
    //     }

    //     UserData.exercisePoint += selectedExercise.assignedCurrency;
    // }

    // private int selectActiveSequence()
    // {
    //     // Filter out inactive exercises
    //     List<ExerciseType> activeExercises = generalScriptableObj.exerciseTypes
    //         .Where(exercise => !exercise.watchedAlready)
    //         .ToList();

    //     if (activeExercises.Count > 0)
    //     {
    //         int sequenceIndex = initialExerciseNode % activeExercises.Count; // Ensure index wraps around if necessary
    //         ExerciseType selectedExercise = activeExercises[sequenceIndex];
    //         int originalIndex = generalScriptableObj.exerciseTypes.IndexOf(selectedExercise); // Find the original index
    //         IncreaseinitialExerciseNode();

    //         return originalIndex;
    //     }
    //     else
    //     {
    //         // Handle the case where all exercises are inactive
    //         Debug.LogWarning("No active exercises available.");
    //         return -1;
    //     }
    // }

    private int selectActiveSequence() {
        int totalExercises = generalScriptableObj.exerciseTypes.Count;

        // Iterate over the exercises starting from initialExerciseNode
        for (int i = 0; i < totalExercises; i++) {
            int currentIndex = (initialExerciseNode + i) % totalExercises;
            ExerciseType currentExercise = generalScriptableObj.exerciseTypes[currentIndex];

            // Check if the current exercise has not been watched already
            if (!currentExercise.watchedAlready) {
                // Set the initialExerciseNode for the next call to be the next exercise
                initialExerciseNode = currentIndex + 1;

                // Return the index of the current exercise
                return currentIndex;
            }
        }

        // Handle the case where all exercises are inactive
        Debug.LogWarning("No active exercises available.");
        return -1;
    }

    public void sendSequentialGym(){
        int sequenceIndex = selectActiveSequence();
        if (sequenceIndex < 0) return;
        ExerciseType selectedExercise = generalScriptableObj.exerciseTypes[sequenceIndex];
        // Disable or remove the selected exercise
        // selectedExercise.watchedAlready = true; // Assuming there's an IsActive property
        StartButton.onClick.RemoveAllListeners();
        GymSecondStartButton.onClick.RemoveAllListeners();
        if (selectedExercise.videoTutorial != null & TvGym != null) {
            selectedExercise.watchedAlready = true; // Assuming there's an IsActive property
            tempSelected = selectedExercise;
            activeIndex = sequenceIndex;

            StartButton.onClick.AddListener(() => TvGym.StartVideoOnClickFromPanelTest(selectedExercise.videoTutorial));
            GymSecondStartButton.onClick.AddListener(() => TvGym.StartVideoOnClickFromPanelTest(selectedExercise.videoTutorial));

        } else {
            Debug.LogWarning("Trying to use a null video clip. Handle appropriately.");
        }
        if(selectedExercise.videoTutorial != null ){
            tempSelected = selectedExercise;
        }
    }
    public void AddExerciseandPoint(){
        generalScriptableObj.exerciseTypes[activeIndex].watchedAlready = true;
        UserData.exerciseTypeOrder.Add(tempSelected);
        UserData.exercisePoint += tempSelected.assignedCurrency;
    }

    // private int selectActiveSequence(){
    //     int prevMovieNode = initialExerciseNode;
    //     // Filter out inactive exercises
    //     List<ExerciseType> activeExercises = generalScriptableObj.exerciseTypes
    //         .Where(exercise => !exercise.watchedAlready)
    //         .ToList();
    //     Debug.Log("ll" + activeExercises.Count);

    //     if (activeExercises.Count > 0){
    //         // Debug.Log("ooo" + sequenceIndex);
    //         IncreaseinitialExerciseNode();
    //         // If there are active exercises, select a random one
    //         return prevMovieNode;
    //     }else{            
    //         // Handle the case where all exercises are inactive
    //         Debug.LogWarning("No active exercises available.");
    //         return -1;
    //     }
    // }
    public void IncreaseinitialExerciseNode(){
        initialExerciseNode++;
    }
    

    private void sendMovie(Movie movie){
        GeneralMainSceneUI.VoiceOverAudio.StopAudio();
        UserData.shortMovieClip = movie;
        Tv.StartVideoOnClickFromPanel(movie.video);
    }
    private void sendExerciseVideo(ExerciseType exercise, GameObject SelectedButton, int index){
        TvGym.StartVideoOnClickFromPanel(exercise.videoTutorial);
        generalScriptableObj.exerciseTypes[index].watchedAlready = true;
        
        SelectedButton.GetComponent<Button>().interactable = false;
        UserData.exerciseTypeOrder.Add(exercise);
        UserData.exercisePoint += exercise.assignedCurrency;
    }

    private void navigateToMainScene(ExperimentType experimentType){
        experimentType.state = true;
        generalScriptableObj.SelectExperimentType(experimentType);
        SceneManager.LoadScene(generalScriptableObj.ActiveScene);
    }

    public void clearContent(){
        foreach (Transform button in scrollviewContent)
           Destroy(button.gameObject);
    }

    public void clearExperimentState(){
        foreach (ExperimentType experimentType in generalScriptableObj.experimentType){
            // clear any default state
            experimentType.state = false;
        }
    }
    public void clearExerciseState(){
        foreach (ExerciseType exerciseType in generalScriptableObj.exerciseTypes){
            // clear any default state
            exerciseType.watchedAlready = false;
        }
    }

}
