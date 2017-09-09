using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore {
    public class TimeController : ITimeController {
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

        public void Init() {
            var mainListener = new GameObject().AddComponent<MonoListener>();
            mainListener.isDestroyable = false;
            mainListener.OnStartEvent += OnStartHandler;
            mainListener.OnFixedUpdateEvent += OnFixedUpdateHandler;
            mainListener.OnUpdateEvent += OnUpdateHandler;
            mainListener.OnLateUpdateEvent += OnLateUpdateHandler;
            mainListener.OnDestroyEvent += OnDestroyHandler;
            mainListener.OnApplicationPauseEvent += OnApplicationPauseHandler;
            mainListener.OnApplicationFocusEvent += OnApplicationFocusHandler;
            mainListener.OnApplicationQuitEvent += OnApplicationQuitHandler;
            mainListener.OnActiveSceneChanged += OnActiveSceneChangedHandler;
            mainListener.OnSceneLoaded += OnSceneLoadedHandler;
            mainListener.OnSceneUnloaded += OnSceneUnloadedHandler;
        }

        #region Event handlers
        public void OnApplicationPauseHandler(bool pause) {
            if (OnApplicationPauseEvent != null) {
                OnApplicationPauseEvent(pause);
            }
        }
        public void OnApplicationFocusHandler(bool focus) {
            if (OnApplicationFocusEvent != null) {
                OnApplicationFocusEvent(focus);
            }
        }
        public void OnApplicationQuitHandler() {
            if (OnApplicationQuitEvent != null) {
                OnApplicationQuitEvent();
            }
        }
        public void OnStartHandler() {
            if (OnStartEvent != null) {
                OnStartEvent();
            }
        }
        public void OnFixedUpdateHandler() {
            if (OnFixedUpdateEvent != null) {
                OnFixedUpdateEvent();
            }
        }
        public void OnUpdateHandler() {
            if (OnUpdateEvent != null) {
                OnUpdateEvent();
            }
        }
        public void OnLateUpdateHandler() {
            if (OnLateUpdateEvent != null) {
                OnLateUpdateEvent();
            }
        }
        public void OnDestroyHandler() {
            if (OnDestroyEvent != null) {
                OnDestroyEvent();
            }
        }
        public void OnActiveSceneChangedHandler(Scene previousScene, Scene nextScene) {
            if (OnActiveSceneChanged != null) {
                OnActiveSceneChanged(previousScene, nextScene);
            }
        }
        public void OnSceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode) {
            if (OnSceneLoaded != null) {
                OnSceneLoaded(scene, loadSceneMode);
            }
        }
        public void OnSceneUnloadedHandler(Scene scene) {
            if (OnDestroyEvent != null) {
                OnSceneUnloaded(scene);
            }
        }
        #endregion
    }
}