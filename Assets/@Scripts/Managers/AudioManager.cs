using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;
    public AudioClip clap_1;
    public AudioClip clap_2;
    public AudioClip ouch_1;
    public AudioClip failGame;
    public AudioClip bossAttackClip;
    [SerializeField] public AudioSource Audio_BackGround;
    [SerializeField] AudioClip[] BackSound;
    bool CheckMusic;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        SetBG();
    }

    void SetBG()
    {
        Audio_BackGround.clip = BackSound[0];
        Audio_BackGround.Pause();
    }

    public void PlaySound()
    {
        AudioClip clipToPlay = Random.value > 0.5 ? clap_1 : clap_2;

        audioSource.PlayOneShot(clipToPlay, 0.3f);
    }

    public void PlayerHItSound()
    {
        audioSource.PlayOneShot(ouch_1, 0.3f);
    }

    public void StopMusic()
    {
        Audio_BackGround.Stop();
    }

    public void PlayMusic()
    {
        audioSource.Play();
    }

    //사운드 실행
    public async void PlayEffectSound(string key)
    {
        var result = await AddressLoad.LoadAsync<AudioClip>(key);
        audioSource.PlayOneShot(result, 1);
    }
}
