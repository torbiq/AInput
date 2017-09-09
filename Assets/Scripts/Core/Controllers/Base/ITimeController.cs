using System;
using UnityEngine.SceneManagement;

namespace AppCore {
    public interface ITimeController : IController {
        event Action OnStartEvent;
        event Action OnFixedUpdateEvent;
        event Action OnUpdateEvent;
        event Action OnLateUpdateEvent;
        event Action OnDestroyEvent;
        event Action<bool> OnApplicationPauseEvent;
        event Action<bool> OnApplicationFocusEvent;
        event Action OnApplicationQuitEvent;
        event Action<Scene, Scene> OnActiveSceneChanged;
        event Action<Scene, LoadSceneMode> OnSceneLoaded;
        event Action<Scene> OnSceneUnloaded;
    }
}