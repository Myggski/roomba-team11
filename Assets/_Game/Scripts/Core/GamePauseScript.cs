using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePauseScript : MonoBehaviour
{
    public static bool gamePaused = false;
    [SerializeField] private GameObject pauseUI;

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if(gamePaused)
        {
            resumeGame();
        }
        else
        {
            pauseGame();
        }
    }

    public void pauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void resumeGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }
}
