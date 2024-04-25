using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenMessage : MonoBehaviour
{
    [SerializeField] private ObjectsMessage _objects;
    [Space]
    [SerializeField] private Message _gameLevel;
    [SerializeField] private Message _bonusLevelS;
    [SerializeField] private Message _bonusLevelP;
    [SerializeField] private Message _gameOver;
    [SerializeField] private Message _levelComplete;

    private readonly WaitActivate _waitActivate = new();

    private void Awake()
    {
        _objects.Initialize();

        SoundSingleton sound = SoundSingleton.Instance;

        _gameLevel.Initialize(_objects, sound.PlayNewLevel);
        _bonusLevelS.Initialize(_objects, sound.PlayNewLevel);
        _bonusLevelP.Initialize(_objects, sound.PlayNewLevel);
        _gameOver.Initialize(_objects, sound.PlayGameOver);
        _levelComplete.Initialize(_objects, sound.PlayLevelComplete);

        Clear();

        _objects.AddListener(OnClick);

        #region Local function
        //======================
        void OnClick()
        {
            _waitActivate.Activate();
            _objects.ButtonHide();
        }
        #endregion
    }

    public WaitActivate GameLevel_Wait(int level, int time)
    {
        WaitActivate wait = new();
        gameObject.SetActive(true);
        StartCoroutine(GameLevel_Coroutine());
        return wait;

        #region Local function
        //=================================
        IEnumerator GameLevel_Coroutine()
        {
            _gameLevel.SendFormat_Wait(level.ToString(), time.ToStringTime());
            yield return _waitActivate.Deactivate();
            yield return _gameLevel.Fide();
            wait.Activate();
            Clear();
        }
        #endregion
    }

    public WaitActivate BonusLevelSingle_Wait(int attempts) => BonusLevel_Wait(_bonusLevelS, attempts);
    public WaitActivate BonusLevelPair_Wait(int attempts) => BonusLevel_Wait(_bonusLevelP, attempts);
    private WaitActivate BonusLevel_Wait(Message bonusLevel, int attempts)
    {
        WaitActivate wait = new();
        gameObject.SetActive(true);
        StartCoroutine(BonusLevel_Coroutine());
        return wait;

        #region Local function
        //=================================
        IEnumerator BonusLevel_Coroutine()
        {
            yield return bonusLevel.SendFormatComment_Wait(attempts.ToString());
            yield return _waitActivate.Deactivate();
            yield return bonusLevel.Fide();
            wait.Activate();
            Clear();
        }
        #endregion
    }

    public WaitActivate GameOver()
    {
        WaitActivate wait = new();
        gameObject.SetActive(true);
        StartCoroutine(GameOver_Coroutine());
        return wait;

        #region Local function
        //=================================
        IEnumerator GameOver_Coroutine()
        {
            yield return _gameOver.Send_Wait();
            yield return _gameOver.Fide();
            wait.Activate();
            Clear();
        }
        #endregion
    }

    public void LevelComplete()
    {
        gameObject.SetActive(true);
        StartCoroutine(GameOver_Coroutine());

        #region Local function
        //=================================
        IEnumerator GameOver_Coroutine()
        {
            yield return _levelComplete.Send_Wait();
            yield return _levelComplete.Fide();
            Clear();
        }
        #endregion
    }

    private void Clear()
    {
        _objects.TextEmpty();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _gameLevel.OnDestroy();
        _bonusLevelS.OnDestroy();
        _bonusLevelP.OnDestroy();
        _gameOver.OnDestroy();
        _levelComplete.OnDestroy();
    }

    #region Nested classes
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

        private ObjectsMessage _objects;
        private Action _playSound;
        
        public void Initialize(ObjectsMessage objects, Action playSound)
        {
            _caption.Initialize(objects.textCaption);
            _comment.Initialize(objects.textComment);
            _objects = objects;
            _playSound = playSound;
        }

        public WaitForSeconds Send_Wait()
        {
            ActivateObject();
            _caption.Send(_appearDuration);
            _comment.Send(_appearDuration);
            if (_isSeparator)
                _objects.SeparatorAppear(_appearDuration);

            return new WaitForSeconds(Mathf.Max(_timeMessage, _appearDuration));
        }

        public WaitForSeconds SendFormat_Wait(string value1, string value2)
        {
            ActivateObject();
            _caption.SendFormat(value1, _appearDuration);
            _comment.SendFormat(value2, _appearDuration);
            if (_isSeparator)
                _objects.SeparatorAppear(_appearDuration);

            return new WaitForSeconds(Mathf.Max(_timeMessage, _appearDuration));
        }

        public WaitForSeconds SendFormatCaption_Wait(string value)
        {
            ActivateObject();
            _caption.SendFormat(value, _appearDuration);
            _comment.Send(_appearDuration);
            if (_isSeparator)
                _objects.SeparatorAppear(_appearDuration);

            return new WaitForSeconds(Mathf.Max(_timeMessage, _appearDuration));
        }

        public WaitForSeconds SendFormatComment_Wait(string value)
        {
            ActivateObject();
            _caption.Send(_appearDuration);
            _comment.SendFormat(value, _appearDuration);
            if (_isSeparator)
                _objects.SeparatorAppear(_appearDuration);

            return new WaitForSeconds(Mathf.Max(_timeMessage, _appearDuration));
        }

        public WaitForSeconds Fide()
        {
            _caption.Fade(_fadeDuration);
            _comment.Fade(_fadeDuration);
            if (_isSeparator)
                _objects.SeparatorFade(_fadeDuration);

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
            _objects.ActivateObject(_isSeparator, _isButton);
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
    //************************************
    [Serializable]
    private class ObjectsMessage
    {
        public TMP_Text textCaption;
        public TMP_Text textComment;
        public Image imageSeparator;
        public Button buttonStart;

        private GameObject _objButton;
        private GameObject _objSeparator;

        public void Initialize()
        {
            _objButton = buttonStart.gameObject;
            _objSeparator = imageSeparator.gameObject;
        }

        public void SeparatorAppear(float duration) => imageSeparator.Appear(imageSeparator.color, duration);
        public void SeparatorFade(float duration) => imageSeparator.Fade(imageSeparator.color, duration);

        public void ActivateObject(bool isSeparator, bool isButton)
        {
            _objSeparator.SetActive(isSeparator);
            _objButton.SetActive(isButton);
        }

        public void TextEmpty()
        {
            textCaption.text = string.Empty;
            textComment.text = string.Empty;
        }

        public void AddListener(UnityAction call) => buttonStart.onClick.AddListener(call);

        public void ButtonHide() => _objButton.SetActive(false);
    }
    #endregion
}
