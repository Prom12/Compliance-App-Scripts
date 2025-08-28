using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayExperiments : MonoBehaviour
{
    [SerializeField] private UIControlDashboard UIControl;
    [SerializeField] private GeneralScriptableObj GeneralScriptableObj;
    

    // Start is called before the first frame update
    void Start()
    {
        GeneralScriptableObj.ChangeGameState(GeneralScriptableObj.ExperimentState.AtDashboard.ToString());
        GeneralScriptableObj.reloadUI = true;
    } 

}
