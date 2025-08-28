using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DeviceType : MonoBehaviour
{
    public bool isOculus()
    {
        // Get the list of available XR display subsystems
        List<XRDisplaySubsystem> displays = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances(displays);

        // Check if any of the display subsystems is running
        foreach (var display in displays)
        {
            if (display.running)
            {
                return true;
            }
        }

        return false;
    }
    
}
