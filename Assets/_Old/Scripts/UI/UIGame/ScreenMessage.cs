using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScreenMessage : MonoBehaviour
{
    [SerializeField] private Game _game;
    [Space]
    [SerializeField] private Message _empty;
    [SerializeField] private Message _best;
    [SerializeField] private Message _newStage;
    [SerializeField] private Message _gameOver;

    private DataGame _dataGame;
    private WaitQueue _queueCoroutines;

    private void Awake()
    {
        _dataGame = DataGame.Instance;
        _queueCoroutines = new(this);

        SoundSingleton sound = SoundSingleton.InstanceF;
        TMP_Text thisText = GetComponent<TMP_Text>();

        _empty.Initialize(thisText, null);
        _best.Initialize(thisText, sound.PlayNewRecord);
        _newStage.Initialize(thisText, sound.PlayNewStage);
        _gameOver.Initialize(thisText, sound.PlayGameOver);

        ClearOff();

        _game.EventStartGame += ClearOff;
        _game.EventGameOver += OnGameOver;

        _dataGame.EventNewRecord += OnNewRecord;
        _dataGame.EventNewStage += OnNewStage;

        #region Local Function
        void OnGameOver()
        {
            _queueCoroutines.StopAndClear();
            _gameOver.Send();
            gameObject.SetActive(true);
        }
        #endregion
    }

    private void OnNewRecord()
    {
        gameObject.SetActive(true);
        StartCoroutine(OnNewRecordCoroutine());

        #region Local Function
        IEnumerator OnNewRecordCoroutine()
        {
            yield return _queueCoroutines.Add(_best.SendCoroutine());
            gameObject.SetActive(false);
        }
        #endregion
    }

    private void OnNewStage()
    {
        gameObject.SetActive(true);
        StartCoroutine(OnNewStageCoroutine());

        #region Local Function
        IEnumerator OnNewStageCoroutine()
        {
            yield return _queueCoroutines.Add(_newStage.SendFormatCoroutine(_dataGame.Stage + 1));
            gameObject.SetActive(false);
        }
        #endregion
    }

    private void ClearOff()
    {
        _empty.Send();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (DataGame.Instance == null)
            return;

        _dataGame.EventNewRecord -= OnNewRecord;
        _dataGame.EventNewStage -= OnNewStage;
    }

    #region Nested Classe
    [Serializable]
    private class Message
    {
        [SerializeField] private string _key = string.Empty;
        [SerializeField] private string _textAdd = string.Empty;
        [SerializeField] private Color _color = Color.white;
        [Space]
        [SerializeField] private float _timeMessage = 1.5f;

        private readonly Localization _localization;
        
        private TMP_Text _text;
        private Action _playSound;

        public Message()
        {
            _localization = Localization.Instance;
        }

        public void Initialize(TMP_Text text, Action playSound)
        {
            _text = text;
            _playSound = playSound;
        }

        public void Send()
        {
            _playSound?.Invoke();
            _text.text = _localization.GetText(_key) + _textAdd;
            _text.color = _color;
        }

        public IEnumerator SendCoroutine()
        {
            _playSound?.Invoke();
            _text.text = _localization.GetText(_key) + _textAdd;
            _text.color = _color;

            yield return new WaitForSecondsRealtime(_timeMessage);
        }

        public IEnumerator SendFormatCoroutine( int value)
        {
            _playSound?.Invoke();
            _text.text = _localization.GetTextFormat(_key, value) + _textAdd;
            _text.color = _color;

            yield return new WaitForSecondsRealtime(_timeMessage);
        }

    }
    #endregion
}
