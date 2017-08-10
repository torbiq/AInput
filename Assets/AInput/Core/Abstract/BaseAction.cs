namespace AInput {
    public abstract class BaseAction {
        /// <summary>
        /// Scene controller that uses action.
        /// </summary>
        public BaseInputController inputController { get; set; }
        virtual public void Update() {
        }
    }
}