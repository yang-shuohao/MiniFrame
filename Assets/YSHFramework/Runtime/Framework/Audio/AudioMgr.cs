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

        // 记录当前播放的音效
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

        #region 音效
        /// <summary>
        /// 播放音效并在播放完毕时触发回调
        /// </summary>
        public void PlaySFX(string audioClipName, UnityAction onComplete = null)
        {
            // 如果该音效已经在播放，则不重复播放
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
        /// 设置音效音量
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// 清理所有音效
        /// </summary>
        public void ClearAllSFX()
        {
            sfxSource.Stop();
            playingSFX.Clear();
        }

        #endregion

        #region 背景音乐
        /// <summary>
        /// 播放背景音乐
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
        /// 停止当前播放的背景音乐
        /// </summary>
        public void StopBgMusic()
        {
            bgSource.Stop();
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public void PauseBgMusic()
        {
            bgSource.Pause();
        }

        /// <summary>
        /// 恢复播放背景音乐
        /// </summary>
        public void ResumeBgMusic()
        {
            bgSource.Play();
        }

        /// <summary>
        /// 设置背景音乐音量
        /// </summary>
        public void SetBgMusicVolume(float volume)
        {
            bgSource.volume = Mathf.Clamp01(volume);
        }

        #endregion
    }
}
