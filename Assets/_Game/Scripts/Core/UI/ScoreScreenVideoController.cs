using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UIElements;

public class ScoreScreenVideoController : MonoBehaviour
{

    [SerializeField] VideoPlayer congratzVideoPlayer;
    [SerializeField] VideoPlayer loopVideoPlayer;
    [SerializeField] Canvas congratzVideoPlayerCanvas;
    [SerializeField] Canvas loopVideoPlayerCanvas;
    [SerializeField] GameObject highScoreUI;

    [SerializeField] UIDocument scoreAndHighscore;
    
    private bool videoStarted = true;

    private int firstVideoLenght = 22;

    void Update()
    {
        if(videoStarted)
        {
            videoStarted = false;
            Invoke("ChangeVideo", firstVideoLenght);
        }
    }

    private void ChangeVideo()
    {
        congratzVideoPlayer.enabled = false;
        highScoreUI.SetActive(true);
        loopVideoPlayer.enabled = true;
        loopVideoPlayerCanvas.enabled = true;

        // Plan is to access the UI doc and get the text for score and highscore.
        // change them to be playerpref.getInt("score") & ("highscore").

        //scoreAndHighscore. - Get/set scoretext
        //scoreAndHighscore. - Get/set highscoretext

    }
}
