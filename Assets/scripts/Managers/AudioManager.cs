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
    [SerializeField] public  AudioSource Audio_BackGround;
    [SerializeField] AudioClip[] BackSound;

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
        //Audio_BackGround.clip = BackSound[DiskRotator.ChoseBG];
        Audio_BackGround.clip = BackSound[0];
    }

    public void PlaySound()
    {
        AudioClip clipToPlay = Random.value > 0.5 ? clap_1 : clap_2;

        audioSource.PlayOneShot(clipToPlay, 0.1f);
    }

    public void PlayerHItSound()
    {

        audioSource.PlayOneShot(ouch_1, 0.1f);
    }
    private void Update()
    {
       
    }
    private void DisplayBackgroundMusicTime()
    {
        float currentTime = Audio_BackGround.time;
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        Debug.Log($"Background music playing for {minutes}:{seconds:00}");
    }

    public void StopMusic()
    {
       Audio_BackGround.Stop();
    }
}
