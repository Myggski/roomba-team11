using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class HighscoreSave : MonoBehaviour
{

    [SerializeField] UIDocument scoreAndHighscore;
    [SerializeField] FloatVariable score;
    [SerializeField] FloatVariable highScore;
    private Label playerScoreTXT;
    private Label highScoreTXT;
    //private float playerScore;


    void OnEnable()
    {
        //playerScore = score.Value;
        var rootVisualElement = scoreAndHighscore.rootVisualElement;
        playerScoreTXT = rootVisualElement.Q<Label>("PlayerScore");
        highScoreTXT = rootVisualElement.Q<Label>("HighScore");

        playerScoreTXT.text = $"{score.Value}";
        CompairPlayerScoreToHighscore();
        highScoreTXT.text = $"{highScore.Value}";

        
    }

    private void CompairPlayerScoreToHighscore()
    {
        if(score.Value > highScore.Value)
        {           
            Debug.Log("playerscore: " + score.Value);
            Debug.Log("Highscore: " + highScore.Value);

            highScore.ChangeValue(score.Value);
        }
    }
}
