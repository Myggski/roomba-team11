using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cat Variables", menuName = "Cat/Cat Variables")]
public class CatVariables : ScriptableObject
{
    [Header("Player Roomba")]
    public GameObject _playerGameObject;
    
    [Header("Roaming and footprints")]
    public bool _showWalkAreaBox;
    public Vector3 RandomWalkBoxPos;
    public Vector3 RandomWalkBoxPos1;
    public Vector2 delayBetweenWalking;
    public Vector2 amountFootstepsToLeave;
    public Vector2 delayBetweenFootprints;

    [Header("Sleeping")] 
    public float wakeUpRangeFromPlayer;
    
    [Header("Game Events")]
    public GameEvent makeFootstepsEvent;
    public GameEvent stopMakeFootstepsEvent;
    public GameEvent changeToSleepState;
    public GameEvent changeToWalkToRandomState;

    [Header("Sounds")] 
    public string MeowSound;
    public Vector2 MeowSoundDelay;
}