using UnityEngine;
using System.Collections;

namespace AInput {
    public class AInputMain : MonoBehaviour {
        static AInputMain() {
            _instance = new GameObject().AddComponent<AInputMain>();
            DontDestroyOnLoad(_instance.gameObject);
            _instance._baseInputControllerInstance = _instance.gameObject.AddComponent<BaseInputController>();
        }
        private static AInputMain _instance;
        private BaseInputController _baseInputControllerInstance;
        private void Awake() {
            if (_instance) {
                if (_instance != this) {
                    Destroy(gameObject);
                }
            }
        }
        public static void SetAction(BaseAction action) {
            _instance._baseInputControllerInstance.currentAction = action;
        }
    }
}
