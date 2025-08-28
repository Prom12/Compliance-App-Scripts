using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonText : MonoBehaviour
{
    public TMP_Text messageText;

    public void changeText(string message){
        messageText.SetText(message);
    }
}
