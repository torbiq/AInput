using UnityEngine.SceneManagement;
namespace AppCore {
    public interface ISceneUnloadable {
        void OnSceneUnloaded(Scene scene);
    }
}
