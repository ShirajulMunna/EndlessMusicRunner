using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    const string EffectSound = "EffectSound_{0}";

    private AudioSource audioSource;
    int clap_1 = 0;
    int clap_2 = 1;
    int ouch_1 = 2;
    int longNoteClip = 3;
    [SerializeField] public AudioSource Audio_BackGround;
    [SerializeField] AudioClip[] BackSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SetBG();
    }

    private void Start()
    {
        PlayerManager.instance.Ac_Hit += PlayerHItSound;
    }

    void SetBG()
    {
        Audio_BackGround.clip = BackSound[0];
        Audio_BackGround.Pause();
    }

    public async void CreateSound_Shot(int id)
    {
        var str_audio = string.Format(EffectSound, id);
        var audio = await str_audio.LoadAsync<AudioClip>();
        audioSource.PlayOneShot(audio, 0.3f);
    }

    public void PlaySound(ScoreManager.E_ScoreState state)
    {
        var idx = state == ScoreManager.E_ScoreState.Perfect ? clap_1 : clap_2;
        CreateSound_Shot(idx);
    }

    public void PlayerHItSound()
    {
        CreateSound_Shot(ouch_1);
    }
    //롱노트사운드
    public void LongNoteSound()
    {
        CreateSound_Shot(longNoteClip);
    }

    public void StopMusic()
    {
        Audio_BackGround.Stop();
    }

    public void PlayMusic()
    {
        audioSource.Play();
    }

    public double GetAudioTime()
    {
        return audioSource.time;
    }

    //사운드 실행
    public async void PlayEffectSound(string key)
    {
        var result = await key.LoadAsync<AudioClip>();
        audioSource.PlayOneShot(result, 1);
    }
}
