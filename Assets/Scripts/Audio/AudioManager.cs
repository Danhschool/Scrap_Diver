using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private AudioSource wind;
    [SerializeField] private AudioSource[] sfx;

    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip countdown;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip gear;
    [SerializeField] private AudioClip slow_wind;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private AudioClip scroll;
    [SerializeField] private AudioClip transition;
 
    [SerializeField] private bool playBgm;
    [SerializeField] private int bgmIndex;

    private int poolIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGM(0);
    }

    //private void Update()
    //{
    //    if (playBgm == false && BgmIsPlaying())
    //        StopAllBGM();


    //    if (playBgm && bgm[bgmIndex].isPlaying == false)
    //        PlayRandomBGM();
    //}

    public void PlaySFX(AudioSource sfx, bool randomPitch = false, float minPitch = .85f, float maxPitch = 1.1f)
    {
        if (sfx == null)
            return;

        float pitch = Random.Range(minPitch, maxPitch);

        sfx.pitch = pitch;
        sfx.Play();
    }

    public void SFXDelayAndFade(AudioSource source, bool play, float taretVolume, float delay = 0, float fadeDuratuin = 1)
    {
        StartCoroutine(SFXDelayAndFadeCo(source, play, taretVolume, delay, fadeDuratuin));
    }

    public void PlayBGM(int index)
    {
        StopAllBGM();

        bgmIndex = index;
        bgm[index].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    [ContextMenu("Play random music")]
    public void PlayRandomBGM()
    {
        StopAllBGM();
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void PlayWindSFX()
    {
        PlaySFX(wind);
    }
    public void StopWindSFX()
    {
        wind.Stop();
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfx.Length == 0) return;

        sfx[poolIndex].clip = clip;
        sfx[poolIndex].Play();

        poolIndex = (poolIndex + 1) % sfx.Length;
    }
    public void StopSFX(AudioClip clip)
    {
        if (clip == null || sfx.Length == 0) return;

        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfx[i].clip == clip && sfx[i].isPlaying)
            {
                sfx[i].Stop();
                break;
            }
        }
    }
    public void PlayCoinSFX() =>  PlaySFX(coin);
    public void PlayClickSFX() => PlaySFX(click);
    public void PlayCountdownSFX() => PlaySFX(countdown);
    public void PlayExplosionSFX() => PlaySFX(explosion);
    public void PlayGearSFX() => PlaySFX(gear);
    public void StopGearSFX() => StopSFX(gear);
    public void PlaySlowWindSFX() => PlaySFX(slow_wind);
    public void PlayWrongSFX() => PlaySFX(wrong);
    public void PlayScrollSFX() => PlaySFX(scroll);
    public void StopScrollSFX() => StopSFX(scroll);
    public void PlayTransitionSFX() => PlaySFX(transition);
    public void StopTransitionSFX() => StopSFX(transition);

    private bool BgmIsPlaying()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (bgm[i].isPlaying)
                return true;
        }

        return false;
    }

    private IEnumerator SFXDelayAndFadeCo(AudioSource source, bool play, float targetVolume, float delay = 0, float fadeDuration = 1)
    {
        yield return new WaitForSeconds(delay);

        float startVolume = play ? 0 : source.volume;
        float endVolume = play ? targetVolume : 0;
        float elapsed = 0;

        if (play)
        {
            source.volume = 0;
            source.Play();
        }

        //Fade in/out over the duration
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, endVolume, elapsed / fadeDuration);
            yield return null;
        }

        source.volume = endVolume;

        if (play == false)
            source.Stop();
    }
}
