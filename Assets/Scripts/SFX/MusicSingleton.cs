using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSingleton : ASingleton<MusicSingleton>
{
    [SerializeField] private float _timeStart = 6f;

    private AudioSource _thisAudio;

    protected override void Awake()
    {
        base.Awake();

        _thisAudio = GetComponent<AudioSource>();
        _thisAudio.volume = 0;
    }

    private void Start()
    {
        StartCoroutine(StartMenuMusicCoroutine());

        IEnumerator StartMenuMusicCoroutine()
        {
            float volume = 0, speed = 1f / _timeStart;
            _thisAudio.volume = volume;
            _thisAudio.Play();

            while (volume < 1f)
            {
                yield return null;
                volume += speed * Time.unscaledDeltaTime;
                _thisAudio.volume = volume;
            }

            _thisAudio.volume = 1f;
        }
    }
}
