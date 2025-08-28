using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using OculusSampleFramework;
public class SwitchScenes : MonoBehaviour
{
    [Header("Empty Camera Object Location")]
    [SerializeField] private GameObject OfficeEmptyObject;
    [SerializeField] private GameObject GymEmptyObject;
    public GameObject OfficeCamera;
    public string sceneName;

    public bool movieRoom = false;
    private Vector3 movieRoomLocation = new Vector3(4.088f, 0.518f, 2.9f);
    private Vector3 movieRoomRotation = new Vector3(0f, 270f, 0f);

    private Vector3 officeLocation = new Vector3(4.642f, 0.25f, -0.339f);
    private Vector3 officeRotation = new Vector3(0f, -92.63f, 0f);
    private Vector3 gymLocation = new Vector3(4.64f, 0.25f, 13.07f);
    private Vector3 gymRotation = new Vector3(-13.6f, -92.63f, 0f);
    private bool Office = true;
    private bool toggleOffice = true;
     private void Update()
    {
        HandleUserInput();
    }

    private void HandleUserInput()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Two))
        {
            toggleScenes();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            toggleScenes();
        }
    }

     private void toggleScenes(){
        if(toggleOffice){
            changeLocation();
        }else{
            changeLocation(Office = false);
        }
        toggleOffice = !toggleOffice;
    }

    private void  changeLocation(bool Office = true){
      if (Office){
        if(movieRoom){
                OfficeCamera.transform.position = movieRoomLocation;
                OfficeCamera.transform.rotation = Quaternion.Euler(movieRoomRotation);
        }else{
                OfficeCamera.transform.position = officeLocation;
                OfficeCamera.transform.rotation = Quaternion.Euler(officeRotation);
        }
      }else{
        OfficeCamera.transform.position = gymLocation;
        OfficeCamera.transform.rotation = Quaternion.Euler(gymRotation);
      }
    }

    public void SwitchScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    } 
}
