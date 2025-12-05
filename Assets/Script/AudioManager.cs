using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayer;
    int channelIndex;

    public enum Sfx{ button, platform, platformEnemy}

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("bgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayer = new AudioSource[channels];

        for(int index=0; index < sfxPlayer.Length; index++)
        {
            sfxPlayer[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayer[index].playOnAwake = false;
            sfxPlayer[index].volume = sfxVolume;

        }

    }

    public void PLaySfx(Sfx sfx)
    {

        for(int index=0; index<sfxPlayer.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayer.Length;
            if(sfxPlayer[loopIndex].isPlaying)
            continue;

            channelIndex = loopIndex;

            AudioSource src = sfxPlayer[loopIndex];
            src.clip = sfxClip[(int)sfx];

            src.volume = (sfx == Sfx.platformEnemy) ? 1f : sfxVolume;
            src.Play();
            return;
        }
        sfxPlayer[0].clip = sfxClip[(int)sfx];
        sfxPlayer[0].Play();
    }
}
