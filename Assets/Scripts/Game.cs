using System;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameLevel _gameLevel;
    [SerializeField] private BonusLevels _bonusLevels;
    [Space]
    [SerializeField] private bool _isGame;

    private void Start()
    {
        if (_isGame)
            _gameLevel.StartLevel();
        else
            _bonusLevels.StartLevel();
    }
}
