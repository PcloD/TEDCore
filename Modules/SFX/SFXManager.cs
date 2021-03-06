﻿using UnityEngine;
using TEDCore.ObjectPool;
using TEDCore.Resource;
using TEDCore.Coroutine;

namespace TEDCore.Audio
{
    public class SFXManager : Singleton<SFXManager>
    {
        private const string OBJECT_POOL_KEY = "SFXManager";
        private float m_volume = 1f;

        public SFXManager()
        {
            GameObject referenceAsset = new GameObject();
            referenceAsset.AddComponent<AudioSource>();

            ObjectPoolManager.Instance.AddPool(OBJECT_POOL_KEY, referenceAsset, 10);
        }

        public void SetVolume(float volume)
        {
            m_volume = volume;
        }

        public void Play(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                TEDDebug.LogError("[SFXManager] - The asset name is null or empty.");
                return;
            }

            ResourceManager.Instance.LoadAsync<AudioClip>(assetName, OnAssetLoaded);
        }

        public void Play(string bundleName, string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                TEDDebug.LogError("[SFXManager] - The asset name is null or empty.");
                return;
            }

            ResourceManager.Instance.LoadAsync<AudioClip>(bundleName, assetName, OnAssetLoaded);
        }

        private void OnAssetLoaded(AudioClip audioClip)
        {
            if(audioClip == null)
            {
                TEDDebug.LogError("[SFXManager] - The AudioClip is null.");
                return;
            }

            AudioSource audioSource = ObjectPoolManager.Instance.Get(OBJECT_POOL_KEY).GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.volume = m_volume;
            audioSource.Play();

            CoroutineManager.Instance.Create()
                                 .Enqueue(CoroutineUtils.WaitForSeconds(audioClip.length))
                                 .Enqueue(OnPlayFinished, audioSource)
                                 .StartCoroutine();
        }

        private void OnPlayFinished(AudioSource audioSource)
        {
            audioSource.clip = null;
            ObjectPoolManager.Instance.Recycle(OBJECT_POOL_KEY, audioSource.gameObject);

            ResourceManager.Instance.Release();
        }
    }
}
