using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;
    public AudioClip clap_1;
    public AudioClip clap_2;
    public AudioClip ouch_1;
    public AudioClip failGame;
    public AudioClip bossAttackClip;
    public AudioClip longNoteClip;
    [SerializeField] public AudioSource Audio_BackGround;
    [SerializeField] AudioClip[] BackSound;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetBG();
    }

    void SetBG()
    {
        Audio_BackGround.clip = BackSound[0];
        Audio_BackGround.Pause();
    }

    public void PlaySound(ScoreManager.E_ScoreState state)
    {
        var audio = state == ScoreManager.E_ScoreState.Perfect ? clap_1 : clap_2;

        audioSource.PlayOneShot(audio, 0.3f);
    }

    public void PlayerHItSound()
    {
        audioSource.PlayOneShot(ouch_1, 0.3f);
    }
    //롱노트사운드
    public void LongNoteSound()
    {
        audioSource.PlayOneShot(longNoteClip, 0.5f);
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
        var result = await AddressLoad.LoadAsync<AudioClip>(key);
        audioSource.PlayOneShot(result, 1);
    }
}
