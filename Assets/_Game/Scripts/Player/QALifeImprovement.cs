using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QALifeImprovement : MonoBehaviour
{

    private GamePauseScript gamePauseScriptreference;
    // Start is called before the first frame update
    void Start()
    {
        gamePauseScriptreference = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<GamePauseScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            gamePauseScriptreference.resumeGame();
            SceneManager.LoadScene("MAINSCENE");
        }
    }
}
