using System.Collections;
using UnityEngine;

public class SoundSingleton : ASingleton<SoundSingleton>
{
    [Space]
    [SerializeField] private AudioClip _clipSpawn;
    [SerializeField] private AudioClip _clipSpawnBomb;
    [SerializeField] private AudioClip _clipFixed;
    [SerializeField] private AudioClip _clipToStart;
    [SerializeField] private AudioClip _clipRemove;
    [SerializeField] private AudioClip _clipExplode;
    [SerializeField] private AudioClip _clipTickingBomb;
    [SerializeField] private AudioClip _clipNewRecord;
    [SerializeField] private AudioClip _clipNewStage;
    [SerializeField] private AudioClip _clipReset;
    [SerializeField] private AudioClip _clipGameOver;
    [SerializeField] private AudioClip _SwitchWindows;
    [Space]
    [SerializeField] private AudioClip _clipMove;
    [SerializeField] private float _timePausePlayMove = 0.03f;

    private AudioSource _thisAudio;

    private WaitForSecondsRealtime _pausePlayMove;
    private bool _isPlayingMove = false;

    protected override void Awake()
    {
        base.Awake();

        _thisAudio = GetComponent<AudioSource>();
        _thisAudio.loop = true;
        _pausePlayMove = new(_timePausePlayMove);
    }

    public void PlaySpawn() => _thisAudio.PlayOneShot(_clipSpawn);
    public void PlaySpawnBomb() => _thisAudio.PlayOneShot(_clipSpawnBomb);
    public void PlayFixed() => _thisAudio.PlayOneShot(_clipFixed);
    public void PlayToStart() => _thisAudio.PlayOneShot(_clipToStart);
    public void PlayRemove() => _thisAudio.PlayOneShot(_clipRemove);
    public void PlayExplode() => _thisAudio.PlayOneShot(_clipExplode);
    public void PlayNewRecord() => _thisAudio.PlayOneShot(_clipNewRecord);
    public void PlayNewStage() => _thisAudio.PlayOneShot(_clipNewStage);
    public void PlayReset() => _thisAudio.PlayOneShot(_clipReset);
    public void PlayGameOver() => _thisAudio.PlayOneShot(_clipGameOver);
    public void PlaySwitchWindows() => _thisAudio.PlayOneShot(_SwitchWindows);

    public void PlayMove()
    {
        if (_isPlayingMove) return;

        _thisAudio.PlayOneShot(_clipMove);
        StartCoroutine(PlayMoveCoroutine());

        #region Local Function
        IEnumerator PlayMoveCoroutine()
        {
            _isPlayingMove = true;
            yield return _pausePlayMove;
            _isPlayingMove = false;
        }
        #endregion
    }
   
    public void StartTickingBomb()
    {
        _thisAudio.clip = _clipTickingBomb;
        _thisAudio.Play();
    }

    public void Stop() => _thisAudio.Stop();
}
