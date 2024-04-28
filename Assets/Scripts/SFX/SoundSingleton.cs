using UnityEngine;

public class SoundSingleton : ASingleton<SoundSingleton>, IVolume
{
    [Space]
    [SerializeField] private AudioClip _clipNewLevel;
    [SerializeField] private AudioClip _clipLevelComplete;
    [SerializeField] private AudioClip _clipGameOver;
    [SerializeField] private AudioClip _clipStart;
    [SerializeField] private AudioClip _clipSelect;
    [SerializeField] private AudioClip _clipFixed;
    [SerializeField] private AudioClip _clipError;
    [SerializeField] private AudioClip _clipTurn;
    [SerializeField] private AudioClip _clipShuffle;

    private AudioSource _thisAudio;

    public float Volume 
    { 
        get => _thisAudio.volume;
        set
        {
            _thisAudio.volume = value;
            _notPlay = value < 0.001f;
        }
    }

    private bool _notPlay = false;

    protected override void Awake()
    {
        base.Awake();

        _thisAudio = GetComponent<AudioSource>();
    }

    public void PlayNewLevel() => Play(_clipNewLevel);
    public void PlayLevelComplete() => Play(_clipLevelComplete);
    public void PlayGameOver() => Play(_clipGameOver);
    public void PlayStart() => Play(_clipStart);
    public void PlaySelect() => Play(_clipSelect);
    public void PlayFixed() => Play(_clipFixed);
    public void PlayError() => Play(_clipError);
    public void PlayTurn() => Play(_clipTurn);
    public void PlayShuffle() => Play(_clipShuffle);

    private void Play(AudioClip clip)
    {
        if (_notPlay) return;

        _thisAudio.PlayOneShot(clip);
    }
}
