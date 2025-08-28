using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;



public class UIControl : MonoBehaviour
{
    [Header("Shared UIControl")]
    [SerializeField] public GameObject MainScreenCanvasObject;
    [SerializeField] public GameObject ExerciseScreenCanvasObject;
    [SerializeField] public Canvas MainScreenCanvas;
    [SerializeField] public Canvas ExerciseScreenCanvas;
    [SerializeField] public Canvas DisplayWhenQuitButtonIsClickedCanvas;
    [SerializeField] public GameObject QuitExerciseButton;
    [SerializeField] public GameObject PromptScreenCanvas;
    [SerializeField] public GeneralScriptableObj GeneralScriptableObj;
    [SerializeField] public GeneralMainSceneUI GeneralMainSceneUI;
    [SerializeField] public GeneralRoomSceneUI GeneralRoomSceneUI;
    [SerializeField] public TMP_Text MovieMainTitle;
    [SerializeField] public TMP_Text DisplayWhenQuitButtonIsClickedTitle;
    [SerializeField] public TMP_Text ExerciseMainTitle;
    [SerializeField] public TMP_Text MoviePromptUITitle;

}
