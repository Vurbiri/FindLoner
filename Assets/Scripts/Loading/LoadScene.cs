using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene 
{
    private AsyncOperation _asyncOperation = null;
    private readonly int _nextScene;
    private float _addProgress = 0f;
    private readonly Slider _slider = null;
    private readonly bool _isAddProgress = false;

    private float Progress
    {
        get
        {
            if(_isAddProgress)
                return _asyncOperation.progress * 0.555f + _addProgress;

            return _asyncOperation.progress * 1.11f;
        }
    }

    public LoadScene(int nextScene) => _nextScene = nextScene;
    public LoadScene(int nextScene, Slider slider, bool isAddProgress = false) : this(nextScene)
    {
        _slider = slider;
        _isAddProgress = isAddProgress;
    }

    public IEnumerator StartCoroutine()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(_nextScene);
        _asyncOperation.allowSceneActivation = false;
        if (_slider != null)
            return ProgressCoroutine();
        
        return null;

        IEnumerator ProgressCoroutine()
        {
            while (!_asyncOperation.isDone)
            {
                _slider.value = Progress;
                yield return null;
            }
        }
    }

    public void End()
    {
        if (_asyncOperation == null)
            return;

        SetProgress(0.5f);
        _asyncOperation.allowSceneActivation = true;
    }

    public void SetProgress(float progress)
    {
        if (!_isAddProgress || _asyncOperation == null || _slider == null)
            return;

        _addProgress = progress;
        _slider.value = Progress;
    }
}
