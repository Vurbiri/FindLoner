using UnityEngine;

public class SoundSingleton : ASingleton<SoundSingleton>
{
    [Space]
    [SerializeField] private AudioClip _clipNewLevel;
    [SerializeField] private AudioClip _clipLevelComplete;
    [SerializeField] private AudioClip _clipGameOver;
    [SerializeField] private AudioClip _clipSelect;
    [SerializeField] private AudioClip _clipError;
    [SerializeField] private AudioClip _clipTurn;
    [SerializeField] private AudioClip _clipShuffle;

    private AudioSource _thisAudio;

    protected override void Awake()
    {
        base.Awake();

        _thisAudio = GetComponent<AudioSource>();
    }

    public void PlayNewLevel() => _thisAudio.PlayOneShot(_clipNewLevel);
    public void PlayLevelComplete() => _thisAudio.PlayOneShot(_clipLevelComplete);
    public void PlayGameOver() => _thisAudio.PlayOneShot(_clipGameOver);
    public void PlaySelect() => _thisAudio.PlayOneShot(_clipSelect);
    public void PlayError() => _thisAudio.PlayOneShot(_clipError);
    public void PlayTurn() => _thisAudio.PlayOneShot(_clipTurn);
    public void PlayShuffle() => _thisAudio.PlayOneShot(_clipShuffle);
}
