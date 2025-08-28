using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralDashBoardUI : UIControlDashboard
{
    private void Start(){
        SetUI();
        ToggleScene();
    }
    public void SetUI(){
        // Clear UI Content
        DashboardScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControl>().clearContent();
        if(GeneralScriptableObj.CheckGameState(GeneralScriptableObj.ExperimentState.AtDashboard.ToString())){
            DashBoardMainTitle.SetText("Kindly Choose the Experiment Group of the current individuals");
            DashboardScreenCanvas.GetComponent<Canvas>().GetComponent<UIDynamicControl>().UpdateUI();
            DashboardScreenCanvasObject.SetActive(true);
        }
    }

    public void ToggleScene(){
        if(GeneralScriptableObj.ActiveScene == ""){
            GeneralScriptableObj.ActiveScene = "Main Scene";

        }else if(GeneralScriptableObj.ActiveScene == "Main Scene"){
            GeneralScriptableObj.ActiveScene = "Main Scene 2";
        }else if(GeneralScriptableObj.ActiveScene == "Main Scene 2"){
            GeneralScriptableObj.ActiveScene = "Main Scene";
        }else{
            GeneralScriptableObj.ActiveScene = "Main Scene";
        }
        DashBoardToggleSceneButton.SetText("ToggleScene from ( "+ GeneralScriptableObj.ActiveScene +" )");
    }
}
