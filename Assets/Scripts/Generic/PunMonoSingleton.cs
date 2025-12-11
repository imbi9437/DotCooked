using System;
using Interface;
using Photon.Pun;
using UnityEngine;

namespace Generic
{
    public class PunMonoSingleton<T> : MonoBehaviourPunCallbacks, IInitializable where T : MonoBehaviourPunCallbacks
    {
        private static T _instance;
        private static object _lock = new object();
        private static bool _isApplicationQuitting;
        public bool isDontDestroyOnLoad;
        public bool IsCompleteInitialize { get; set; }

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_isApplicationQuitting) return null;
                    if (_instance == false) _instance = FindAnyObjectByType<T>();
                    if (_instance) return _instance;
                    
                    GameObject gameObject = new GameObject(nameof(T));
                    _instance = gameObject.AddComponent<T>();
                    return _instance;
                }
            }
        }
        
        protected virtual void Awake()
        {
            if (_instance == null) _instance = this as T;
            else DestroyImmediate(this);
            
            if (isDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnApplicationQuit()
        {
            _isApplicationQuitting = true;
        }

        public virtual void Initialize()
        {
            IsCompleteInitialize = true;
        }
    }
}
