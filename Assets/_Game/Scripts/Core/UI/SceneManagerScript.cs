using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {
    private Coroutine _levelCompleteCoroutine;
    [SerializeField]
    private GameObject _scoreManager;
    [SerializeField] 
    private FloatVariable _score;
    
    
    public void GoToGameScene()
    {
        SceneManager.LoadScene("MAINSCENE");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToLevelComplete(float delayTime) {
        if (ReferenceEquals(_levelCompleteCoroutine, null))
        {
            _score.ChangeValue(_scoreManager.gameObject.GetComponent<Score>().FinalScore());
            _levelCompleteCoroutine = StartCoroutine(ChangeToLevelComplete(delayTime));    
        }
    }

    private IEnumerator ChangeToLevelComplete(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        
        
        SceneManager.LoadScene("LevelComplete");
       
        
        _levelCompleteCoroutine = null;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
