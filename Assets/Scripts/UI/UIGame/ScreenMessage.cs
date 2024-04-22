using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text _textCaption;
    [SerializeField] private TMP_Text _textComment;
    [SerializeField] private Image _imageSeparator;
    [SerializeField] private Button  _buttonStart;
    [Space]
    [SerializeField] private Message _gameLevel;
    [SerializeField] private Message _bonusLevelS;
    [SerializeField] private Message _bonusLevelP;
    [SerializeField] private Message _gameOver;
    [SerializeField] private Message _best;

    private readonly WaitActivate _waitActivate = new();

    private DataGame _dataGame;


    private void Awake()
    {
        GameObject _goButton = _buttonStart.gameObject;

        _dataGame = DataGame.Instance;
        SoundSingleton sound = SoundSingleton.InstanceF;

        _gameLevel.Initialize(_textCaption, _textComment, _imageSeparator, _goButton, sound.PlayNewStage);
        _bonusLevelS.Initialize(_textCaption, _textComment, _imageSeparator, _goButton, null);
        _bonusLevelP.Initialize(_textCaption, _textComment, _imageSeparator, _goButton, null);
        _gameOver.Initialize(_textCaption, _textComment, _imageSeparator, _goButton, sound.PlayGameOver);
        _best.Initialize(_textCaption, _textComment, _imageSeparator, _goButton, sound.PlayNewRecord);

        Clear();

        _buttonStart.onClick.AddListener(OnClick);
        _dataGame.EventNewRecord += OnNewRecord;

        #region Local function
        //======================
        void OnClick()
        {
            _waitActivate.Activate();
            _goButton.SetActive(false);
        }
        #endregion
    }

    public WaitActivate GameLevel_Wait()
    {
        WaitActivate wait = new();
        gameObject.SetActive(true);
        StartCoroutine(GameLevel_Coroutine());
        return wait;

        #region Local function
        //=================================
        IEnumerator GameLevel_Coroutine()
        {

            _gameLevel.SendFormatCaption_Wait(_dataGame.Level.ToString());
            yield return _waitActivate.Deactivate();
            yield return _gameLevel.Fide();
            wait.Activate();
            Clear();
        }
        #endregion
    }

    public WaitActivate BonusLevelSingle_Wait()
    {
        WaitActivate wait = new();
        gameObject.SetActive(true);
        StartCoroutine(BonusLevelSingle_Coroutine());
        return wait;

        #region Local function
        //=================================
        IEnumerator BonusLevelSingle_Coroutine()
        {

            yield return _bonusLevelS.Send_Wait();
            yield return _bonusLevelS.Fide();
            wait.Activate();
            Clear();
        }
        #endregion
    }

    public WaitActivate BonusLevelPair_Wait()
    {
        WaitActivate wait = new();
        gameObject.SetActive(true);
        StartCoroutine(BonusLevelPair_Coroutine());
        return wait;

        #region Local function
        //=================================
        IEnumerator BonusLevelPair_Coroutine()
        {

            yield return _bonusLevelP.Send_Wait();
            yield return _bonusLevelP.Fide();
            wait.Activate();
            Clear();
        }
        #endregion
    }



    private void OnNewRecord()
    {
        gameObject.SetActive(true);
        StartCoroutine(OnNewRecord_Coroutine());

        #region Local function
        //=================================
        IEnumerator OnNewRecord_Coroutine()
        {
            yield return _best.Send_Wait();
            yield return _best.Fide();
            Clear();
        }
        #endregion
    }

    private void Clear()
    {
        _textCaption.text = string.Empty;
        _textComment.text = string.Empty;
        //_imageSeparator.gameObject.SetActive(false);
        //_buttonStart.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventNewRecord -= OnNewRecord;

        _gameLevel.OnDestroy();
        _bonusLevelS.OnDestroy();
        _bonusLevelP.OnDestroy();
        _gameOver.OnDestroy();
        _best.OnDestroy();
    }

    #region Nested Classe
    //********************************************
    [Serializable]
    private class Message
    {
        [SerializeField] private Text _caption;
        [SerializeField] private bool _isSeparator = true;
        [SerializeField] private Text _comment;
        [SerializeField] private bool _isButton = false;
        [Space]
        [SerializeField] private float _timeMessage = 5f;
        [Space]
        [SerializeField] private float _appearDuration = 0.5f;
        [SerializeField] private float _fadeDuration = 0.5f;

        private Image _separator;
        private GameObject _objButton;
        private GameObject _objSeparator;
        private Action _playSound;
        
        public void Initialize(TMP_Text caption, TMP_Text comment, Image separator, GameObject button, Action playSound)
        {
            _caption.Initialize(caption);
            _comment.Initialize(comment);
            _separator = separator;
            _objSeparator = separator.gameObject;
            _objButton = button;
            _playSound = playSound;
        }

        public WaitForSeconds Send_Wait()
        {
            ActivateObject();
            _caption.Send(_appearDuration);
            _comment.Send(_appearDuration);
            if (_isSeparator)
                _separator.Appear(_separator.color, _appearDuration);

            return new WaitForSeconds(Mathf.Max(_timeMessage, _appearDuration));
        }

        public WaitForSeconds SendFormat_Wait(string value1, string value2)
        {
            ActivateObject();
            _caption.SendFormat(value1, _appearDuration);
            _comment.SendFormat(value2, _appearDuration);
            if (_isSeparator)
                _separator.Appear(_separator.color, _appearDuration);

            return new WaitForSeconds(Mathf.Max(_timeMessage, _appearDuration));
        }

        public WaitForSeconds SendFormatCaption_Wait(string value)
        {
            ActivateObject();
            _caption.SendFormat(value, _appearDuration);
            _comment.Send(_appearDuration);
            if (_isSeparator)
                _separator.Appear(_separator.color, _appearDuration);

            return new WaitForSeconds(Mathf.Max(_timeMessage, _appearDuration));
        }

        public WaitForSeconds SendFormatComment_Wait(string value)
        {
            ActivateObject();
            _caption.Send(_appearDuration);
            _comment.SendFormat(value, _appearDuration);
            if (_isSeparator)
                _separator.Appear(_separator.color, _appearDuration);

            return new WaitForSeconds(Mathf.Max(_timeMessage, _appearDuration));
        }

        public WaitForSeconds Fide()
        {
            _caption.Fade(_fadeDuration);
            _comment.Fade(_fadeDuration);
            if (_isSeparator)
                _separator.Fade(_separator.color, _fadeDuration);

            return new WaitForSeconds(_fadeDuration);
        }

        public void OnDestroy()
        {
            _caption.OnDestroy();
            _comment.OnDestroy();
        }

        private void ActivateObject()
        {
            _playSound?.Invoke();
            _objSeparator.SetActive(_isSeparator);
            _objButton.SetActive(_isButton);
        }

        #region Nested Classe
        //*******************************
        [Serializable]
        private class Text
        {
            [SerializeField] private string _key = string.Empty;
            [SerializeField] private Color _color = Color.white;

            private TMP_Text _text;
            private Localization _localization;
            private string _value = null;
            private bool _isActive = false;

            public void Initialize(TMP_Text text)
            {
                _text = text;
                _localization = Localization.Instance;
                _localization.EventSwitchLanguage += ReLocalize;
            }

            public void Send(float appearDuration)
            {
                _value = null;
                _text.text = (_isActive = !string.IsNullOrEmpty(_key)) ? _localization.GetText(_key) : string.Empty;
                if (_isActive)
                    _text.Appear(_color, appearDuration);
            }

            public void SendFormat(string value, float appearDuration)
            {
                _value = value;
                _text.text = (_isActive = !string.IsNullOrEmpty(_key)) ? _localization.GetTextFormat(_key, value) : string.Empty;
                if (_isActive)
                    _text.Appear(_color, appearDuration);
            }

            public void Fade(float fadeDuration)
            {
                if (!_isActive)
                    return;

                _isActive = false;
                _text.Fade(_color, fadeDuration);
            }

            public void OnDestroy()
            {
                if(Localization.Instance != null)
                    _localization.EventSwitchLanguage -= ReLocalize;
            }

            private void ReLocalize()
            {
                if (!_isActive)
                    return;

                if (_value != null)
                    _text.text = _localization.GetTextFormat(_key, _value);
                else
                    _text.text = _localization.GetText(_key);
            }
        }
        #endregion

    }
    #endregion
}
