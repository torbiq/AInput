using UnityEngine;
using System.Collections;

namespace AInput {
    public class AInputMain : MonoBehaviour {
        /// <summary>
        /// Base input controller instance of singletone.
        /// </summary>
        private BaseInputController _baseInputControllerInstance;
        #region Singletone implementation
        private static AInputMain _instance;
        static AInputMain() {
            _instance = new GameObject().AddComponent<AInputMain>();
            _instance.name = "AInputCore";
            _instance._baseInputControllerInstance = _instance.gameObject.AddComponent<BaseInputController>();
            DontDestroyOnLoad(_instance.gameObject);
        }
        #endregion
        private void Awake() {
            if (_instance) {
                if (_instance != this) {
                    Destroy(gameObject);
                }
            }
        }
        /// <summary>
        /// Adds action to currect actions list.
        /// </summary>
        /// <param name="action"></param>
        public static void AddAction(BaseAction action) {
            action.inputController = _instance._baseInputControllerInstance;
            _instance._baseInputControllerInstance.currentActions.Add(action);
        }
        /// <summary>
        /// Removes action from current actions list.
        /// </summary>
        /// <param name="action">Action to remove.</param>
        /// <returns>True if it's found and removed.</returns>
        public static bool RemoveAction(BaseAction action) {
            return _instance._baseInputControllerInstance.currentActions.Remove(action);
        }
        /// <summary>
        /// Sets action and removes others.
        /// </summary>
        /// <param name="action">Action to set.</param>
        public static void SetAction(BaseAction action) {
            _instance._baseInputControllerInstance.currentActions.Clear();
            AddAction(action);
        }
    }
}
