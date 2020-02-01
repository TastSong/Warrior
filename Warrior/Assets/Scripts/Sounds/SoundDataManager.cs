using UnityEngine;
using System.Collections;

public class SoundDataManager : MonoBehaviour
{
    public AudioClip[] ControlSounds = null;

    public AudioClip KnockDownSound = null;
    public AudioClip FatalitySound = null;

    public AudioClip KatanaOnSound = null;
    public AudioClip KatanaOffSound = null;

    public AudioClip ShopBuyHealth = null;
    public AudioClip ShopBuySword = null;
    public AudioClip ShopBuyCombo = null;

    public string ActionFilename = "action1";
    public float ActionMusicFadeOutTime = 0.4f;
    public float ActionMusicFadeInTime = 0.4f;
    public float ActionMusicVolume = 1;

    public string CalmFilename = "ambi_quiet1";
    public float CalmMusicFadeOutTime = 0.4f;
    public float CalmMusicFadeInTime = 0.4f;
    public float CalmMusicVolume = 1;

    [System.NonSerialized]
    public AudioClip ActionMusic;
    [System.NonSerialized]
    public AudioClip CalmMusic;

    const float MaxMusicVolume = 0.0f;

    public AudioClip ControlSound { get { return ControlSounds[Random.Range(0, ControlSounds.Length)]; } }

    public static SoundDataManager Instance;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ActionMusic = Resources.Load("Music/" + ActionFilename) as AudioClip;

        CalmMusic = Resources.Load("Music/" + CalmFilename) as AudioClip;
    }

    public bool IsSwitchingMusic()
    {
        return true;
    }
}
