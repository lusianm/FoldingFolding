using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM_LIST
{
    ACTION_RETRO,
    GAME_LV1,
    GAME_LV2,
    WIN_1,
    WIN_2
}

public enum SFX_LIST
{
    PLAYER_MOVE,
    PLAYER_JUMP,
    PLAYER_DIE_1,
    PLAYER_DIE_2,
    BLOCK_ALERT,
    DESTROY_BLOCK,
    FOLD,
    MENU_CLICK_1,
    MENU_CLICK_2
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource _bgmSource;
    [SerializeField] AudioSource _sfxSource;

    [SerializeField] AudioClip[] _bgmClips;
    [SerializeField] AudioClip[] _sfxClips;

    [Range(0, 1)]
    public float _MainBGMVolume = 1f;
    [Range(0, 1)]
    public float _MainSFXVolume = 1f;

    #region 싱글톤 인스턴스화 : instance
    public static SoundManager s_instance;
    public static SoundManager instance
    {
        get
        {
            if (!s_instance)
            {
                s_instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
                if (!s_instance)
                {
                    Debug.LogError("SoundManager s_instance null");
                    return null;
                }
            }

            return s_instance;
        }
    }

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;

            DontDestroyOnLoad(this);

        }
        else if (this != s_instance)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    /// <summary>
    /// 특정 BGM을 재생합니다.
    /// </summary>
    /// <param name="target">재생할 BGM 용도</param>
    /// <param name="vol">볼륨. bgm이 너무 커서 0.2을 기본값으로 한다.</param>
    public void Play_BGM(BGM_LIST target, float vol = 0.2f)
    {
        if ((int)target >= _bgmClips.Length) return;

        if (_bgmSource.isPlaying) _bgmSource.Stop();
        _bgmSource.volume = vol * _MainBGMVolume;
        _bgmSource.clip = _bgmClips[(int)target];

        //우승 BGM에서 반복 제거
        if ((int)target > 2) _bgmSource.loop = false;
        else _bgmSource.loop = true;

        _bgmSource.Play();
    }

    /// <summary>
    /// 특정 효과음을 재생합니다.
    /// </summary>
    /// <param name="target">재생할 SFX 용도</param>
    /// <param name="vol">볼륨</param>
    public void Play_SFX(SFX_LIST target, float vol = 1f)
    {
        if ((int)target >= _sfxClips.Length) return;
        
        _sfxSource.PlayOneShot(_sfxClips[(int)target], vol);
    }

    //임시 사운드 퀵 플레이어
    KeyCode[] quick = { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
                        KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9};
    bool trigger = false;

    private void Start()
    {
        Play_BGM(BGM_LIST.ACTION_RETRO);
    }

    private void Update()
    {
        if (Application.platform != RuntimePlatform.WindowsEditor) return;

        if (Input.GetKeyDown(KeyCode.Space)) trigger = !trigger;

        for (int i = 0; i < quick.Length; i++)
        {
            if (Input.GetKeyDown(quick[i]))
            {
                if (trigger) Play_BGM((BGM_LIST)System.Enum.ToObject(typeof(BGM_LIST), i));
                else Play_SFX((SFX_LIST)System.Enum.ToObject(typeof(SFX_LIST), i));
            }
        }
    }
}
