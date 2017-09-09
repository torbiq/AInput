using UnityEngine;
using AJSON;

namespace AppCore {
    public class MainEntryPoint : MonoBehaviour {
        PlayerData pd = new PlayerData();
        private void Awake() {

            Debug.Log(new AJSON.Object(pd).Serialize());

            //new AppInitializer().Initialize();
            //Destroy(gameObject);
        }
    }
}
