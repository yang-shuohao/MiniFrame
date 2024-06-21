using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������
/// </summary>
public class AudioMgr : Singleton<AudioMgr>
{
    //�������ֲ�����
    private AudioSource bgSource;
    //��Ч������
    private List<AudioSource> sfxSource = new List<AudioSource>();

    //���������Ƿ���
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

    //��Ч�Ƿ���
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
    /// ������Ч
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
    /// ���ű�������
    /// </summary>
    /// <param name="name">Ƭ������</param>
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
    /// ��ͣ���ű�������
    /// </summary>
    public void PauseBgMusic()
    {
        bgSource?.Pause();
    }

    /// <summary>
    /// ֹͣ���ֱ�������
    /// </summary>
    public void StopBgMusic()
    {
        bgSource?.Stop();
    }
}
