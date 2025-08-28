using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;



public class UIControlDashboard : MonoBehaviour
{
    [SerializeField] public GameObject DashboardScreenCanvasObject;
    [SerializeField] public Canvas DashboardScreenCanvas;
    [SerializeField] public GeneralScriptableObj GeneralScriptableObj;
    [SerializeField] public GeneralDashBoardUI GeneralDashBoardUI;
    [SerializeField] public TMP_Text DashBoardMainTitle;
    [SerializeField] public TMP_Text DashBoardToggleSceneButton;
}
