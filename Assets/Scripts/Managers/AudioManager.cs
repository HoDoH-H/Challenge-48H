using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioData> sfxList;

    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource sfxPlayerPrefab;

    [SerializeField] float fadeDuration = .75f;

    float originalMusicVolume;
    Dictionary<AudioId, AudioData> sfxLookup;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        originalMusicVolume = musicPlayer.volume;

        sfxLookup = sfxList.ToDictionary(x => x.id);
    }

    public IEnumerator PlaySFX(AudioClip clip, bool pauseMusic = false)
    {
        if (clip == null) yield break;

        if (pauseMusic)
        {
            musicPlayer.Pause();
            StartCoroutine(UnPauseMusic(clip.length));
        }

        var sfxPlayer = Instantiate(sfxPlayerPrefab, PlayerMovement.instance.playerCharacter.transform.position, new Quaternion(), PlayerMovement.instance.playerCharacter.transform);
        sfxPlayer.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length + .5f);
        Destroy(sfxPlayer.gameObject);
    }

    public void PlaySFX(AudioId audioId, bool pauseMusic = false)
    {
        if (!sfxLookup.ContainsKey(audioId)) return;

        var audioData = sfxLookup[audioId];
        StartCoroutine(PlaySFX(audioData.clip, pauseMusic));
    }

    public void PlayMusic(AudioClip clip, bool loop = true, bool fade = false)
    {
        if (clip == null || clip == musicPlayer.clip) return;

        StartCoroutine(PlayMusicAsync(clip, loop, fade));
    }

    IEnumerator PlayMusicAsync(AudioClip clip, bool loop, bool fade)
    {
        if (fade)
            yield return musicPlayer.DOFade(0, fadeDuration).WaitForCompletion();

        musicPlayer.clip = clip;
        musicPlayer.loop = loop;
        musicPlayer.Play();

        if (fade)
            yield return musicPlayer.DOFade(originalMusicVolume, fadeDuration).WaitForCompletion();
    }

    IEnumerator UnPauseMusic(float delay)
    {
        yield return new WaitForSeconds(delay);

        musicPlayer.volume = 0;
        musicPlayer.UnPause();
        musicPlayer.DOFade(originalMusicVolume, fadeDuration);
    }

    public IEnumerator LimitMusicVolume(bool value)
    {
        if (value)
            yield return musicPlayer.DOFade(originalMusicVolume / 4, fadeDuration);
        else
            yield return musicPlayer.DOFade(originalMusicVolume, fadeDuration);
    }
}

public enum AudioId { UIHover, UISelect, Take, Store }

[Serializable]
public class AudioData
{
    public AudioId id;
    public AudioClip clip;
}
