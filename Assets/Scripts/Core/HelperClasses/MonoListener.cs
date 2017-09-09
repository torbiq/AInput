using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore {
    public class MonoListener : MonoBehaviour {
        #region Events
        public event Action OnStartEvent;
        public event Action OnFixedUpdateEvent;
        public event Action OnUpdateEvent;
        public event Action OnLateUpdateEvent;
        public event Action OnDestroyEvent;
        public event Action<bool> OnApplicationPauseEvent;
        public event Action<bool> OnApplicationFocusEvent;
        public event Action OnApplicationQuitEvent;
        public event Action<Scene, Scene> OnActiveSceneChanged;
        public event Action<Scene, LoadSceneMode> OnSceneLoaded;
        public event Action<Scene> OnSceneUnloaded;
        #endregion

        #region UnityEvents handlers
        private void OnSceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode) {
            if (OnSceneLoaded != null) {
                OnSceneLoaded(scene, loadSceneMode);
            }
        }
        private void OnActiveSceneChangedHandler(Scene previousScene, Scene nextScene) {
            if (OnActiveSceneChanged != null) {
                OnActiveSceneChanged(previousScene, nextScene);
            }
        }
        private void OnSceneUnloadedHandler(Scene scene) {
            if (OnSceneUnloaded != null) {
                OnSceneUnloaded(scene);
            }
        }
        #endregion

        #region Mono
        private bool _isDestroyable;
        public bool isDestroyable {
            get {
                return _isDestroyable;
            }
            set {
                if (_isDestroyable != value) {
                    _isDestroyable = value;
                    if (_isDestroyable) {
                        var go = new GameObject();
                        transform.SetParent(go.transform);
                        transform.SetParent(null);
                        Destroy(go);
                    }
                    else {
                        DontDestroyOnLoad(gameObject);
                    }
                }
            }
        }
        private void OnApplicationFocus(bool focus) {
            if (OnApplicationFocusEvent != null) {
                OnApplicationFocusEvent(focus);
            }
        }
        private void OnApplicationPause(bool pause) {
            if (OnApplicationPauseEvent != null) {
                OnApplicationPauseEvent(pause);
            }
        }
        private void OnApplicationQuit() {
            if (OnApplicationQuitEvent != null) {
                OnApplicationQuitEvent();
            }
        }
        private void Awake() {
            SceneManager.activeSceneChanged += OnActiveSceneChangedHandler;
            SceneManager.sceneLoaded += OnSceneLoadedHandler;
            SceneManager.sceneUnloaded += OnSceneUnloadedHandler;
            gameObject.name = "MonoListener";
        }
        private void Start() {
            if (OnStartEvent != null) {
                OnStartEvent();
            }
        }
        private void FixedUpdate() {
            if (OnFixedUpdateEvent != null) {
                OnFixedUpdateEvent();
            }
        }
        private void Update() {
            if (OnUpdateEvent != null) {
                OnUpdateEvent();
            }
        }
        private void LateUpdate() {
            if (OnLateUpdateEvent != null) {
                OnLateUpdateEvent();
            }
        }
        private void OnDestroy() {
            if (OnDestroyEvent != null) {
                OnDestroyEvent();
            }
            SceneManager.activeSceneChanged -= OnActiveSceneChangedHandler;
            SceneManager.sceneLoaded -= OnSceneLoadedHandler;
            SceneManager.sceneUnloaded -= OnSceneUnloadedHandler;
        }
        #endregion
    }
}