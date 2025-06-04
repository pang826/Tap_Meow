using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum E_BGM { }
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

    public void PlaySFX(E_SFX type, float volume)
    {
        _sfxSources.clip = _sfxs[(int)type];
        
        //_sfxSources.PlayOneShot(_sfxs[(int)type]);
        _sfxSources.volume = volume;
        _sfxSources.Play();
    }
}
