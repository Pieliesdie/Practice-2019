using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    void OnGUI()
    {
        const int buttonWidth = 84;
        const int buttonHeight = 60;

        Rect buttonRect = new Rect(
              Screen.width / 2 - (buttonWidth / 2),
              (2 * Screen.height / 3) - (buttonHeight / 2),
              buttonWidth,
              buttonHeight
            );

        if (GUI.Button(buttonRect, "You are lost!"))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}

