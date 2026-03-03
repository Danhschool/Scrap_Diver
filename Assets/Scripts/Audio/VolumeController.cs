using System;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public static VolumeController instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;

    [Header("Volume Offsets (dB)")]
    [SerializeField] private float bgmOffsetDB = 0f;
    [SerializeField] private float sfxOffsetDB = 5f;

    private const string PrefKeyBGM = "BGMVolumeLevel";
    private const string PrefKeySFX = "SFXVolumeLevel";

    public enum VolumeLevel
    {
        Mute = 0,
        Half = 1,
        Full = 2
    }

    [Header("Default Levels")]
    [SerializeField] private VolumeLevel defaultBGM = VolumeLevel.Full;
    [SerializeField] private VolumeLevel defaultSFX = VolumeLevel.Full;

    public VolumeLevel CurrentBGMLevel { get; private set; }
    public VolumeLevel CurrentSFXLevel { get; private set; }

    public event Action<VolumeLevel, VolumeLevel> OnVolumeChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplyVolume("BGM_Volume", CurrentBGMLevel, bgmOffsetDB);
        ApplyVolume("SFX_Volume", CurrentSFXLevel, sfxOffsetDB);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.N))
    //    {
    //        ToggleBGM();
    //    }
    //    if (Input.GetKeyDown(KeyCode.M))
    //    {
    //        ToggleSFX();
    //    }
    //}
    private void LoadSettings()
    {
        int savedBGM = PlayerPrefs.GetInt(PrefKeyBGM, -1);
        CurrentBGMLevel = (savedBGM >= 0 && Enum.IsDefined(typeof(VolumeLevel), savedBGM))
            ? (VolumeLevel)savedBGM : defaultBGM;

        int savedSFX = PlayerPrefs.GetInt(PrefKeySFX, -1);
        CurrentSFXLevel = (savedSFX >= 0 && Enum.IsDefined(typeof(VolumeLevel), savedSFX))
            ? (VolumeLevel)savedSFX : defaultSFX;
    }

    public void ToggleBGM()
    {
        CurrentBGMLevel = GetNextLevel(CurrentBGMLevel);
        ApplyVolume("BGM_Volume", CurrentBGMLevel, bgmOffsetDB);
        PlayerPrefs.SetInt(PrefKeyBGM, (int)CurrentBGMLevel);
        PlayerPrefs.Save();
        OnVolumeChanged?.Invoke(CurrentBGMLevel, CurrentSFXLevel);

        //Debug.Log($"BGM Volume set to {CurrentBGMLevel} (Linear: {LevelToLinearValue(CurrentBGMLevel):F4})");
    }

    public void ToggleSFX()
    {
        CurrentSFXLevel = GetNextLevel(CurrentSFXLevel);
        ApplyVolume("SFX_Volume", CurrentSFXLevel, sfxOffsetDB);
        PlayerPrefs.SetInt(PrefKeySFX, (int)CurrentSFXLevel);
        PlayerPrefs.Save();
        OnVolumeChanged?.Invoke(CurrentBGMLevel, CurrentSFXLevel);
        
        //Debug.Log($"SFX Volume set to {CurrentSFXLevel} (Linear: {LevelToLinearValue(CurrentSFXLevel):F4})");
    }

    private VolumeLevel GetNextLevel(VolumeLevel current)
    {
        return (VolumeLevel)(((int)current + 1) % Enum.GetValues(typeof(VolumeLevel)).Length);
    }

    private void ApplyVolume(string exposedParameter, VolumeLevel level, float offsetDB)
    {
        if (audioMixer == null) return;

        float linearValue = LevelToLinearValue(level);
        float decibelValue = Mathf.Log10(linearValue) * 20f;

        audioMixer.SetFloat(exposedParameter, decibelValue + offsetDB);
    }

    private static float LevelToLinearValue(VolumeLevel level)
    {
        return level switch
        {
            VolumeLevel.Mute => 0.0001f,
            VolumeLevel.Half => 0.3162f,
            VolumeLevel.Full => 1f,
            _ => 1f
        };
    }
}