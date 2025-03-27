using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YSH.Framework
{
    public class AudioMgr : Singleton<AudioMgr>
    {
        public AudioSource bgSource;
        public AudioSource sfxSource;

        private Transform audioSourceParent;

        // ��¼��ǰ���ŵ���Ч
        private HashSet<string> playingSFX = new HashSet<string>();

        public AudioMgr()
        {
            sfxSource = new GameObject("SFXSource").AddComponent<AudioSource>();
            bgSource = new GameObject("BgSource").AddComponent<AudioSource>();
            bgSource.loop = true;

            audioSourceParent = new GameObject("AudioSources").transform;

            sfxSource.transform.SetParent(audioSourceParent);
            bgSource.transform.SetParent(audioSourceParent);
        }

        #region ��Ч
        /// <summary>
        /// ������Ч���ڲ������ʱ�����ص�
        /// </summary>
        public void PlaySFX(string audioClipName, UnityAction onComplete = null)
        {
            // �������Ч�Ѿ��ڲ��ţ����ظ�����
            if (playingSFX.Contains(audioClipName)) return;

            ResMgr.Instance.LoadAssetAsync<AudioClip>(audioClipName, ResMgr.Instance.resLoadType, result =>
            {
                sfxSource.PlayOneShot(result);

                playingSFX.Add(audioClipName);

                MonoMgr.Instance.StartCoroutine(WaitForSoundToFinish(audioClipName, result.length, onComplete));
            });
        }

        private IEnumerator WaitForSoundToFinish(string audioClipName, float duration, UnityAction onComplete)
        {
            yield return new WaitForSeconds(duration);
            playingSFX.Remove(audioClipName);
            onComplete?.Invoke();
        }

        /// <summary>
        /// ������Ч����
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// ����������Ч
        /// </summary>
        public void ClearAllSFX()
        {
            sfxSource.Stop();
            playingSFX.Clear();
        }

        #endregion

        #region ��������
        /// <summary>
        /// ���ű�������
        /// </summary>
        public void PlayBgMusic(string name)
        {
            ResMgr.Instance.LoadAssetAsync<AudioClip>(name, ResMgr.Instance.resLoadType, result =>
            {
                bgSource.clip = result;
                bgSource.Play();
            });
        }

        /// <summary>
        /// ֹͣ��ǰ���ŵı�������
        /// </summary>
        public void StopBgMusic()
        {
            bgSource.Stop();
        }

        /// <summary>
        /// ��ͣ��������
        /// </summary>
        public void PauseBgMusic()
        {
            bgSource.Pause();
        }

        /// <summary>
        /// �ָ����ű�������
        /// </summary>
        public void ResumeBgMusic()
        {
            bgSource.Play();
        }

        /// <summary>
        /// ���ñ�����������
        /// </summary>
        public void SetBgMusicVolume(float volume)
        {
            bgSource.volume = Mathf.Clamp01(volume);
        }

        #endregion
    }
}
