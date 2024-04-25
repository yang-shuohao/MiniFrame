using MiniFrame.Base;
using MiniFrame.Mono;
using MiniFrame.Pool;
using MiniFrame.Res;
using System.Collections.Generic;
using UnityEngine;

namespace MiniFrame.Audio
{
    /// <summary>
    /// ����������
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
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

        public AudioManager()
        {
            MonoManager.Instance.AddUpdateListener(Update);
        }

        private void Update()
        {
            for (int i = sfxSource.Count - 1; i >= 0; i--)
            {
                if (!sfxSource[i].isPlaying)
                {
                    PoolManager.Instance.ReleaseToPool(sfxSource[i].gameObject);
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

            PoolManager.Instance.GetFromPool<AudioSource>("SFXSource", (audioSource) =>
            {
                ResourcesManager.Instance.LoadAssetAsync<AudioClip>(name, (handle) =>
                {
                    audioSource.clip = handle.Result;
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

            ResourcesManager.Instance.LoadAssetAsync<AudioClip>(name, (handle) =>
            {
                bgSource.clip = handle.Result;
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
}
