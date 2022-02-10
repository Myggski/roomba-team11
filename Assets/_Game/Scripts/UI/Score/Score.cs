using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Score : MonoBehaviour
{
    
    private float _score;
    private float _choreScore;
    private float _totalScore;
    private float _scoreMultiplier = 1f;
    
    [Header("Score")]
    [SerializeField] 
    private float _ScoreMAXMultiplier;
    [SerializeField] 
    private float _comboScoreBonusGrowth;
    [SerializeField] 
    private float _comboScoreBonusLifeTime;
    [SerializeField] 
    private float _comboScoreBonusShrink;
    [SerializeField] 
    private float _comboScoreReduceBonusDelay;
    
    private float _comboEndTime;
    private float _comboReduceTime;
    
    [Header("Final Score")]
    [SerializeField] 
    private float _fintalScoreMAXMultiplier;
    private float _finalScoreMultiplier;
    private float _finaltScore;

    [SerializeField] 
    private float _avgCompleteTime;

    [SerializeField] 
    private FloatVariable _timeLeft;

    private Label _scoreLabel;
    private Label _multiplierLabel;
    [SerializeField] 
    private List<ScoreValueInformation> scoreValueInformationList;

    [SerializeField] 
    private GameEventChoreItemSet onChoreItemCompleted;
    public GameEventChoreItemSet GetAddChoreScoreGameEvent => onChoreItemCompleted;

    [SerializeField] private UIDocument _UIDocument;

    private Dictionary<Type, int> _points =
        new Dictionary<Type, int>();

    private void Awake()
    {
        var rootVisualElement = _UIDocument.rootVisualElement;
        
        _scoreLabel = rootVisualElement.Q<Label>("ScoreText");
        _multiplierLabel = rootVisualElement.Q<Label>("MultiplierText");

        scoreValueInformationList.ForEach(scoreInformation => _points.Add(scoreInformation.Set.GetType(), scoreInformation.ChorePoints));
    }

    private void Update()
    {
        if (_scoreMultiplier > 1f)
        {
            float time = Time.time;
            if (time > _comboEndTime && time > _comboReduceTime)
            {
                DecreaseScoreMultiplier();
            }
        }
    }
    
    private void DecreaseScoreMultiplier()
    {
        _comboReduceTime = Time.time + _comboScoreReduceBonusDelay;
        _scoreMultiplier = Mathf.Clamp(_scoreMultiplier - _comboScoreBonusShrink, 1f, _ScoreMAXMultiplier);
        UpdateUIMultiplierTxt();
    }
    
    private void IncreaseScoreMultiplier()
    {
        _comboEndTime = Time.time + _comboScoreBonusLifeTime;
        if (_scoreMultiplier >= _ScoreMAXMultiplier)
            return;
        _scoreMultiplier = Mathf.Clamp(_scoreMultiplier + _comboScoreBonusGrowth, 1, _ScoreMAXMultiplier);
        UpdateUIMultiplierTxt();
    }
    
    public float FinalScore()
    {
        _finalScoreMultiplier = Mathf.Clamp(_avgCompleteTime / _timeLeft.Value, 1, _fintalScoreMAXMultiplier);
        _finaltScore = _totalScore * _finalScoreMultiplier;
        return _finaltScore;
    }

    public void Add(float scoreToAdd)
    {
        _score += scoreToAdd * _scoreMultiplier;
        UpdateTotalScore();
    }

    public void Remove(float scoreToRemove)
    {
        _score = _score - scoreToRemove <= 0 ? _score = 0 : _score -= scoreToRemove;
        UpdateTotalScore();
    }

    public void UpdateChoreScore(RuntimeSetBase set) {
        ScoreValueInformation choreInformation = scoreValueInformationList.Find(x => ReferenceEquals(x.Set, set));
        _choreScore += choreInformation.ChorePoints * _scoreMultiplier;
        UpdateTotalScore();
        IncreaseScoreMultiplier();
    }

    private void UpdateTotalScore()
    {
        _totalScore = _score + _choreScore;
        UpdateUIScoreTxt();
    }

    private void UpdateUIScoreTxt()
    {
        _scoreLabel.text = $"{_totalScore}";
    }

    private void UpdateUIMultiplierTxt()
    {
        _multiplierLabel.text = $"{_scoreMultiplier}";
    }

    [Serializable]
    private class ScoreValueInformation {
        public RuntimeSetBase Set;
        public int ChorePoints;
    }
}
