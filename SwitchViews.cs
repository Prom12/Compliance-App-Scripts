using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchViews : MonoBehaviour
{
    public GeneralDashBoardUI GeneralDashBoardUI;
    public string Active;
    
    public void GoToViewofScene(){
        if(GeneralDashBoardUI.GeneralScriptableObj.ActiveScene == "Main Scene"){
            SceneManager.LoadScene("View 1");
        }else if(GeneralDashBoardUI.GeneralScriptableObj.ActiveScene == "Main Scene 2"){
            SceneManager.LoadScene("View 2");
        }else{
            Debug.LogWarning("No Scene to load");
        }
    }
}
