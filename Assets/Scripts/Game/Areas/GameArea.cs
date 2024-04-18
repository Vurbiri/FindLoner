using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    [SerializeField] private GameLevel _gameLevel;
    [SerializeField] private BonusLevels _bonusLevels;
    [Space]
    [SerializeField] private Timer _timer;


    private void Awake()
    {
        
    }
}
