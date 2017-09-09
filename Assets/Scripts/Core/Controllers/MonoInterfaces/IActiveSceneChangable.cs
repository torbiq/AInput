using UnityEngine.SceneManagement;
namespace AppCore {
    public interface IActiveSceneChangable {
        void OnActiveSceneChanged(Scene previousScene, Scene nextScene);
    }
}
