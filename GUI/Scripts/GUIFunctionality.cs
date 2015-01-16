using UnityEngine;
using System.Collections;

public class GUIFunctionality : MonoBehaviour
{

    public void LoadLevel(string level)
    {
        Application.LoadLevel(level);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

}
