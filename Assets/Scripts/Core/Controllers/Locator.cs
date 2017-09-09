using System.Collections.Generic;
using System;

namespace AppCore {
    static public class Locator {
        #region Members
        /// <summary>
        /// Controllers dictionary.
        /// </summary>
        static private Dictionary<Type, object> _controllers;
        #endregion

        #region Events
        /// <summary>
        /// Event called when controller is added.
        /// </summary>
        static public event Action<IController> OnAddedEvent;
        /// <summary>
        /// Event called when controller is removed (successfully).
        /// </summary>
        static public event Action<IController> OnRemovedEvent;
        #endregion

        /// <summary>
        /// Static constructor for initialization.
        /// </summary>
        static Locator() {
            _controllers = new Dictionary<Type, object>();
        }

        #region Methods
        /// <summary>
        /// Returns requested controller if it exists.
        /// </summary>
        /// <typeparam name="T">Base interface of requested controller (e.g. IDataController of DataController).</typeparam>
        /// <returns>Contained controller if it exists.</returns>
        static public T Get<T>() where T : IController {
            object value = null;
            _controllers.TryGetValue(typeof(T), out value);
            return (T)value;
        }
        /// <summary>
        /// Adds controller.
        /// </summary>
        /// <typeparam name="T">Base interface of added controller (e.g. IDataController of DataController).</typeparam>
        /// <param name="instance">Instance of added controller.</param>
        static public void Add<T>(IController instance) where T : IController {
            _controllers.Add(typeof(T), instance);
            if (OnAddedEvent != null) {
                OnAddedEvent(instance);
            }
        }
        /// <summary>
        /// Removes controller.
        /// </summary>
        /// <typeparam name="T">Base interface of removed controller (e.g. IDataController of DataController).</typeparam>
        /// <returns>True if it's found and removed</returns>
        static public bool Remove<T>() where T : IController {
            object value = null;
            var type = typeof(T);
            if (_controllers.TryGetValue(type, out value)) {
                if (OnRemovedEvent != null) {
                    OnRemovedEvent(value as IController);
                }
                _controllers.Remove(type);
                return true;
            }
            return false;
        }
        #endregion
    }
}
