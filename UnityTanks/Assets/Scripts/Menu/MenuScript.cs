using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
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

        if (GUI.Button(buttonRect, "Start!"))
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
