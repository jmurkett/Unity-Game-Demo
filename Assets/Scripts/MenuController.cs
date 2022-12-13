using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private bool mouseDown = false;
    private void Update()
    {
        // Start the game once the mouse has been pressed and released
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0) && mouseDown)
        {
            SceneManager.LoadScene(1);
        } 
    }
}
