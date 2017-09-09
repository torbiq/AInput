using UnityEngine.SceneManagement;
namespace AppCore {
    public interface ISceneLoadable {
        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode);
    }
}
