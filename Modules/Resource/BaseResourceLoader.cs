﻿using System;
using System.Collections.Generic;

namespace TEDCore.Resource
{
    public abstract class BaseResourceLoader
    {
        public abstract string AssetBundleName { get; }

        public void LoadAsync<T>(string assetName, Action<T> callback) where T : UnityEngine.Object
        {
            ResourceManager.Instance.LoadAsync<T>(AssetBundleName, assetName, callback);
        }

        public void LoadAllAsync<T>(string assetName, Action<List<T>> callback) where T : UnityEngine.Object
        {
            ResourceManager.Instance.LoadAllAsync<T>(AssetBundleName, assetName, callback);
        }

        public void Unload<T>(string assetName) where T : UnityEngine.Object
        {
            ResourceManager.Instance.Unload<T>(AssetBundleName, assetName);
        }

        public void Clear()
        {
            ResourceManager.Instance.Clear();
        }
    }
}
