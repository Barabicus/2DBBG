using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FallingSandGUI : MonoBehaviour
{

    public Text selectedText;
    public FallingSandController controller;

    public void Update()
    {
        if (controller.SelectedString == null || controller.SelectedString.Equals(""))
            selectedText.text = "None";
        else
            selectedText.text = controller.SelectedString;
    }

}
