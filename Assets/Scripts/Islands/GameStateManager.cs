using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using DentedPixel;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _complaintsText;
    [SerializeField] private TextMeshProUGUI _prepText;
    [SerializeField] private TextMeshProUGUI _roundStartedText;

    [SerializeField] private Image _stopwatchImage;

    [SerializeField] private float _roundTimeInMins;

    private float _gameTimer;
    private int _complaints = 0;

    public static Action OnPlayerWins;
    public static Action OnPlayerLoses;

    private void OnEnable()
    {
        EnemyTracker.OnEnemyReachedDestination += IncrementComplaints;
        RoundManager.OnPrepTimeStarted += OnPrepTimeStarted;
        RoundManager.OnRoundStarted += OnRoundStarted;

        _complaintsText.text = $"Complaints {_complaints}/10";
    }

    private void OnDisable()
    {
        EnemyTracker.OnEnemyReachedDestination += IncrementComplaints;
        RoundManager.OnPrepTimeStarted -= OnPrepTimeStarted;
        RoundManager.OnRoundStarted -= OnRoundStarted;

    }

    private void OnRoundStarted()
    {
        LeanTween.scale(_roundStartedText.gameObject, new Vector3(1, 1, 1), 1.0f).setEaseOutBounce().setOnComplete(() =>
        {
            LeanTween.scale(_roundStartedText.gameObject, new Vector3(0, 0, 0), 1.0f).setEaseInBounce();
        });
    }

    private void OnPrepTimeStarted()
    {
        LeanTween.scale(_prepText.gameObject, new Vector3(1, 1, 1), 1.0f).setEaseOutBounce().setOnComplete( () => 
        {
            LeanTween.scale(_prepText.gameObject, new Vector3(0, 0, 0), 1.0f).setEaseInBounce();
        });
    }

    private void IncrementComplaints()
    {
        _complaints += 1;

        _complaintsText.text = $"Complaints {_complaints}/10";
    }

    void Update()
    {
        _gameTimer += Time.deltaTime;
        _stopwatchImage.fillAmount = _gameTimer / (_roundTimeInMins * 60);

        CheckIfPlayerWins();

        CheckIfPlayerLost();
    }

    private void CheckIfPlayerWins()
    {
        if (_gameTimer >= _roundTimeInMins)
        {
            OnPlayerWins?.Invoke();
        }
    }

    private void CheckIfPlayerLost()
    {
        if (_complaints > 10)
        {
            OnPlayerLoses?.Invoke();
        }
    }
}
