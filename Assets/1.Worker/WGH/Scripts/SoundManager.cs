using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum E_BGM { BGM1, BGM2, BGM3 }
public enum E_SFX { Punch1, Punch2 }
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip[] _bgms; 
    [SerializeField] private AudioClip[] _sfxs;

    [SerializeField] private AudioSource _bgmSources;
    [SerializeField] private AudioSource _sfxSources;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _bgmSources = transform.GetChild(0).GetComponent<AudioSource>();
        _sfxSources = transform.GetChild(1).GetComponent<AudioSource>();
    }
    public void PlayBGM(E_BGM bgm, float volume)
    {
        _bgmSources?.Stop();
        _bgmSources.clip = _bgms[(int)bgm];
        _bgmSources.volume = volume;
        _bgmSources.Play();
    }
    public void StopBGM()
    {
        _bgmSources?.Stop();
    }
    public void PlaySFX(E_SFX sfx, float volume)
    {
        _sfxSources.PlayOneShot(_sfxs[(int)sfx]);
        _sfxSources.volume = volume;
    }
}
