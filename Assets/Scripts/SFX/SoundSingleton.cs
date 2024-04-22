using UnityEngine;

public class SoundSingleton : ASingleton<SoundSingleton>
{
    [Space]
    [SerializeField] private AudioClip _clipSpawn;
    [SerializeField] private AudioClip _clipSpawnBomb;
    [SerializeField] private AudioClip _clipFixed;
    [SerializeField] private AudioClip _clipToStart;
    [SerializeField] private AudioClip _clipRemove;
    [SerializeField] private AudioClip _clipTickingBomb;
    [SerializeField] private AudioClip _clipNewRecord;
    [SerializeField] private AudioClip _clipNewLevel;
    [SerializeField] private AudioClip _clipReset;
    [SerializeField] private AudioClip _clipGameOver;
    [SerializeField] private AudioClip _SwitchWindows;
    [SerializeField] private AudioClip _clipMove;

    private AudioSource _thisAudio;


    protected override void Awake()
    {
        base.Awake();

        _thisAudio = GetComponent<AudioSource>();
    }

    public void PlaySpawn() => _thisAudio.PlayOneShot(_clipSpawn);
    public void PlaySpawnBomb() => _thisAudio.PlayOneShot(_clipSpawnBomb);
    public void PlayFixed() => _thisAudio.PlayOneShot(_clipFixed);
    public void PlayToStart() => _thisAudio.PlayOneShot(_clipToStart);
    public void PlayRemove() => _thisAudio.PlayOneShot(_clipRemove);
    public void PlayNewRecord() => _thisAudio.PlayOneShot(_clipNewRecord);
    public void PlayNewStage() => _thisAudio.PlayOneShot(_clipNewLevel);
    public void PlayReset() => _thisAudio.PlayOneShot(_clipReset);
    public void PlayGameOver() => _thisAudio.PlayOneShot(_clipGameOver);
    public void PlaySwitchWindows() => _thisAudio.PlayOneShot(_SwitchWindows);


}
