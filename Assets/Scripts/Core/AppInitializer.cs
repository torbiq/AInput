namespace AppCore {
    public class AppInitializer {
        /// <summary>
        /// Application intialization entry point.
        /// </summary>
        public void Initialize() {
            Locator.OnAddedEvent += OnAddedEventHandler;
            Locator.OnRemovedEvent += OnRemovedEventHandler;
            InitControllers();
        }
        /// <summary>
        /// Intialization of core controllers.
        /// </summary>
        private void InitControllers() {
            Locator.Add<ITimeController>(new TimeController());
            Locator.Add<IDataController>(new DataController());
        }
        /// <summary>
        /// Event handler for adding controller to Locator.
        /// </summary>
        /// <param name="controller">Added controller.</param>
        private void OnAddedEventHandler(IController controller) {
            controller.Init();
            var timeController = Locator.Get<ITimeController>();
            var asApplicationFocusable = controller as IOnApplicationFocusable;
            if (asApplicationFocusable != null) {
                timeController.OnApplicationFocusEvent += asApplicationFocusable.OnApplicationFocus;
            }
            var asApplicationPausable = controller as IOnApplicationPausable;
            if (asApplicationPausable != null) {
                timeController.OnApplicationPauseEvent += asApplicationPausable.OnApplicationPause;
            }
            var asApplicationQuitable = controller as IOnApplicationQuitable;
            if (asApplicationQuitable != null) {
                timeController.OnApplicationQuitEvent += asApplicationQuitable.OnApplicationQuit;
            }
            var asStartable = controller as IStartable;
            if (asStartable != null) {
                timeController.OnStartEvent += asStartable.OnStart;
            }
            var asFixedUpdatable = controller as IFixedUpdatable;
            if (asFixedUpdatable != null) {
                timeController.OnFixedUpdateEvent += asFixedUpdatable.OnFixedUpdate;
            }
            var asUpdatable = controller as IUpdatable;
            if (asUpdatable != null) {
                timeController.OnUpdateEvent += asUpdatable.OnUpdate;
            }
            var asLateUpdatable = controller as ILateUpdatable;
            if (asLateUpdatable != null) {
                timeController.OnLateUpdateEvent += asLateUpdatable.OnLateUpdate;
            }
            var asDestroyable = controller as IOnDestroyable;
            if (asDestroyable != null) {
                timeController.OnDestroyEvent += asDestroyable.OnDestroy;
            }
            var asActiveSceneChangable = controller as IActiveSceneChangable;
            if (asActiveSceneChangable != null) {
                timeController.OnActiveSceneChanged += asActiveSceneChangable.OnActiveSceneChanged;
            }
            var asSceneLoadable = controller as ISceneLoadable;
            if (asSceneLoadable != null) {
                timeController.OnSceneLoaded += asSceneLoadable.OnSceneLoaded;
            }
            var asSceneUnloadable = controller as ISceneUnloadable;
            if (asSceneUnloadable != null) {
                timeController.OnSceneUnloaded += asSceneUnloadable.OnSceneUnloaded;
            }
        }
        /// <summary>
        /// Event handler for removing controller from Locator.
        /// </summary>
        /// <param name="controller">Removed controller.</param>
        private void OnRemovedEventHandler(IController controller) {
            var timeController = Locator.Get<ITimeController>();
            var asApplicationFocusable = controller as IOnApplicationFocusable;
            if (asApplicationFocusable != null) {
                timeController.OnApplicationFocusEvent -= asApplicationFocusable.OnApplicationFocus;
            }
            var asApplicationPausable = controller as IOnApplicationPausable;
            if (asApplicationPausable != null) {
                timeController.OnApplicationPauseEvent -= asApplicationPausable.OnApplicationPause;
            }
            var asApplicationQuitable = controller as IOnApplicationQuitable;
            if (asApplicationQuitable != null) {
                timeController.OnApplicationQuitEvent -= asApplicationQuitable.OnApplicationQuit;
            }
            var asStartable = controller as IStartable;
            if (asStartable != null) {
                timeController.OnStartEvent -= asStartable.OnStart;
            }
            var asFixedUpdatable = controller as IFixedUpdatable;
            if (asFixedUpdatable != null) {
                timeController.OnFixedUpdateEvent -= asFixedUpdatable.OnFixedUpdate;
            }
            var asUpdatable = controller as IUpdatable;
            if (asUpdatable != null) {
                timeController.OnUpdateEvent -= asUpdatable.OnUpdate;
            }
            var asLateUpdatable = controller as ILateUpdatable;
            if (asLateUpdatable != null) {
                timeController.OnLateUpdateEvent -= asLateUpdatable.OnLateUpdate;
            }
            var asDestroyable = controller as IOnDestroyable;
            if (asDestroyable != null) {
                timeController.OnDestroyEvent -= asDestroyable.OnDestroy;
            }
            var asActiveSceneChangable = controller as IActiveSceneChangable;
            if (asActiveSceneChangable != null) {
                timeController.OnActiveSceneChanged -= asActiveSceneChangable.OnActiveSceneChanged;
            }
            var asSceneLoadable = controller as ISceneLoadable;
            if (asSceneLoadable != null) {
                timeController.OnSceneLoaded -= asSceneLoadable.OnSceneLoaded;
            }
            var asSceneUnloadable = controller as ISceneUnloadable;
            if (asSceneUnloadable != null) {
                timeController.OnSceneUnloaded -= asSceneUnloadable.OnSceneUnloaded;
            }
        }
    }
}