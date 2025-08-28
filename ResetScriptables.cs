using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScriptables : MonoBehaviour
{
    [SerializeField] public UserData UserData;

    public void ResetScriptable(){
        UserData.ResetUserDataAndGeneralScript();
    }    
}
