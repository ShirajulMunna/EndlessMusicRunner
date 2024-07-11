using System.Threading.Tasks;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    const string EffectSound = "EffectSound_{0}";
    const string StrPlayMusic = "PlayMusic_{0}";
    const string StrLobbyMusic = "LobbyMusic_0";
    int clap_1 = 0;
    int clap_2 = 1;
    int ouch_1 = 2;
    int longNoteClip = 3;
    AudioSource Audio_BackGround;
    AudioSource Audio_Effect;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        var audioscore = GetComponentsInChildren<AudioSource>();
        Audio_Effect = audioscore[0];
        Audio_BackGround = audioscore[1];
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PlayerManager.instance.Ac_Hit += PlayerHItSound;
        ActionManager.instance.AddAction((E_ActionScene.Lobby, E_ActionList.Start), GetLobbyMusic, true);
    }

    public float GetbackAodioMaxLength()
    {
        return Audio_BackGround.clip.length;
    }

    public async void CreateSound_Shot(int id)
    {
        var str_audio = string.Format(EffectSound, id);
        var audio = await str_audio.LoadAsync<AudioClip>();
        Audio_Effect.PlayOneShot(audio, 0.3f);
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

    public void PauseMusic()
    {
        Audio_BackGround.Pause();
    }

    public void PlayMusic()
    {
        Audio_BackGround.Play();
    }

    public double GetAudioTime()
    {
        return Audio_BackGround.time;
    }

    //사운드 실행
    public async void PlayEffectSound(string key)
    {
        var result = await key.LoadAsync<AudioClip>();
        Audio_Effect.PlayOneShot(result, 1);
    }

    //사운드 실행
    public async Task<bool> GetPlayMusic()
    {
        var idx = UI_Lobby.BitIdx > 1 ? 1 : UI_Lobby.BitIdx;

        var result = await string.Format(StrPlayMusic, idx).LoadAsync<AudioClip>();
        Audio_BackGround.clip = result;
        Audio_BackGround.Pause();
        return true;
    }

    //사운드 실행
    public async void GetLobbyMusic()
    {
        var result = await StrLobbyMusic.LoadAsync<AudioClip>();
        Audio_BackGround.clip = result;
        Audio_BackGround.Play();
    }
}
