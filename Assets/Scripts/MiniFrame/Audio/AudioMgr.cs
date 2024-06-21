using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// …˘“Ùπ‹¿Ì∆˜
/// </summary>
public class AudioMgr : Singleton<AudioMgr>
{
    //±≥æ∞“Ù¿÷≤•∑≈∆˜
    private AudioSource bgSource;
    //“Ù–ß≤•∑≈∆˜
    private List<AudioSource> sfxSource = new List<AudioSource>();

    //±≥æ∞“Ù¿÷ «∑Òæ≤“Ù
    private bool isMuteBgMusic;
    public bool IsMuteBgMusic
    {
        get
        {
            return isMuteBgMusic;
        }
        set
        {
            isMuteBgMusic = value;

            if (bgSource != null)
            {
                bgSource.mute = isMuteBgMusic;
            }
        }
    }

    //“Ù–ß «∑Òæ≤“Ù
    public bool IsMuteSFX { get; set; }

    public AudioMgr()
    {
        MonoMgr.Instance.AddUpdateListener(Update);
    }

    private void Update()
    {
        for (int i = sfxSource.Count - 1; i >= 0; i--)
        {
            if (!sfxSource[i].isPlaying)
            {
                PoolMgr.Instance.ReleaseToPool(sfxSource[i].gameObject);
                sfxSource.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// ≤•∑≈“Ù–ß
    /// </summary>
    /// <param name="name"></param>
    public void PlaySFX(string name)
    {
        if (IsMuteSFX)
        {
            return;
        }

        PoolMgr.Instance.GetFromPool<AudioSource>("SFXSource", (audioSource) =>
        {
            ResMgr.Instance.LoadAssetAsync<AudioClip>(name, ResMgr.Instance.resLoadType, (res) =>
            {
                audioSource.clip = res;
                audioSource.volume = 1;
                audioSource.Play();
                sfxSource.Add(audioSource);
            });
        });
    }

    /// <summary>
    /// ≤•∑≈±≥æ∞“Ù¿÷
    /// </summary>
    /// <param name="name">∆¨∂Œ√˚◊÷</param>
    public void PlayBgMusic(string name)
    {
        if (bgSource == null)
        {
            GameObject go = new GameObject("BgSource");
            bgSource = go.AddComponent<AudioSource>();
        }

        ResMgr.Instance.LoadAssetAsync<AudioClip>(name, ResMgr.Instance.resLoadType, (res) =>
        {
            bgSource.clip = res;
            bgSource.loop = true;
            bgSource.volume = 1;
            bgSource.Play();
        });
    }

    /// <summary>
    /// ‘›Õ£≤•∑≈±≥æ∞“Ù¿÷
    /// </summary>
    public void PauseBgMusic()
    {
        bgSource?.Pause();
    }

    /// <summary>
    /// Õ£÷π≤ø∑÷±≥æ∞“Ù¿÷
    /// </summary>
    public void StopBgMusic()
    {
        bgSource?.Stop();
    }
}
