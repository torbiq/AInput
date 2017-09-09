using UnityEngine;
//using AInput.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace AInput {
    public class BaseInputController : MonoBehaviour {
        /// <summary>
        /// Previous frame's mouse position (same as Input.mousePosition).
        /// </summary>
        public Vector2? prevMouseScreenPos { get; private set; }
        /// <summary>
        /// Current mouse position (same as Input.mousePosition).
        /// </summary>
        public Vector2? currMouseScreenPos { get; private set; }

        /// <summary>
        /// Previous frame's mouse world position.
        /// </summary>
        public Vector2? prevMouseWorldPos { get; private set; }
        /// <summary>
        /// Current mouse world position.
        /// </summary>
        public Vector2? currMouseWorldPos { get; private set; }
        
        /// <summary>
        /// Current drag delta (between frames) in world coords. 
        /// </summary>
        public Vector2 currentDragDeltaVectorWorld { get; private set; }
        /// <summary>
        /// Current drag delta (between frames) in screen coords. 
        /// </summary>
        public Vector2 currentDragDeltaVectorScreen { get; private set; }

        /// <summary>
        /// Mouse delta in world coordinates since pressing mouse.
        /// </summary>
        public Vector2 dragDeltaVectorWorld { get; private set; }
        /// <summary>
        /// Mouse delta in screen coordinates since pressing mouse.
        /// </summary>
        public Vector2 dragDeltaVectorScreen { get; private set; }

        /// <summary>
        /// Overall mouse delta length in world coords since pressing mouse.
        /// </summary>
        public float dragDeltaLengthWorld { get; private set; }
        /// <summary>
        /// Overall mouse delta length in screen coords since pressing mouse.
        /// </summary>
        public float dragDeltaLengthScreen { get; private set; }

        /// <summary>
        /// Input.GetMouseButtonDown(0) shorthand.
        /// </summary>
        public bool mouseDown { get; private set; }
        /// <summary>
        /// Input.GetMouseButton(0) shorthand.
        /// </summary>
        public bool mouseDrag { get; private set; }
        /// <summary>
        /// Input.GetMouseButtonUp(0) shorthand.
        /// </summary>
        public bool mouseUp { get; private set; }

        /// <summary>
        /// Mouse delta in world coords.
        /// </summary>
        public Vector2 GetMouseDeltaWorld() {
            return prevMouseWorldPos == null || currMouseWorldPos == null ? Vector2.zero : currMouseWorldPos.Value - prevMouseWorldPos.Value;
        }
        /// <summary>
        /// Mouse delta in screen coords.
        /// </summary>
        public Vector2 GetMouseDeltaScreen() {
            return prevMouseScreenPos == null || currMouseScreenPos == null ? Vector2.zero : currMouseScreenPos.Value - prevMouseScreenPos.Value;
        }

        /// <summary>
        /// Touched collider by mouse position in world coords.
        /// </summary>
        public Collider2D TouchedColliderWorld() {
            return currMouseScreenPos == null ? null : Physics2D.OverlapPoint(currMouseWorldPos.Value);
        }
        /// <summary>
        /// Touched collider by mouse position in screen coords.
        /// </summary>
        public Collider2D TouchedColliderScreen() {
            return currMouseScreenPos == null ? null : Physics2D.OverlapPoint(currMouseScreenPos.Value);
        }

        /// <summary>
        /// Touched colliders by mouse position in world coords.
        /// </summary>
        public Collider2D[] TouchedCollidersWorld() {
            return Physics2D.OverlapPointAll(currMouseWorldPos.Value);
        }
        /// <summary>
        /// Touched colliders by mouse position in world coords.
        /// </summary>
        public Collider2D[] TouchedCollidersScreen() {
            return Physics2D.OverlapPointAll(currMouseScreenPos.Value);
        }

        /// <summary>
        /// Is collider touched by mouse position in world coords?
        /// </summary>
        public bool IsTouchingColliderWorld(Collider2D collider2d) {
            Collider2D[] touchedColliders = TouchedCollidersWorld();
            return touchedColliders.Contains(collider2d);
        }
        /// <summary>
        /// Is collider touched by mouse position in screen coords?
        /// </summary>
        public bool IsTouchingColliderScreen(Collider2D collider2d) {
            Collider2D[] touchedColliders = TouchedCollidersScreen();
            return touchedColliders.Contains(collider2d);
        }

        /// <summary>
        /// Current actions list.
        /// </summary>
        private List<BaseAction> _currentActions = new List<BaseAction>();
        /// <summary>
        /// Current actions list.
        /// </summary>
        public List<BaseAction> currentActions {
            get {
                return _currentActions;
            }
        }

        #region Mono update inherited
        private void Awake() {
            IAwake();
        }
        private void Start() {
            IStart();
        }
        private void Update() {
            mouseDown = UnityEngine.Input.GetMouseButtonDown(0);
            mouseDrag = UnityEngine.Input.GetMouseButton(0);
            mouseUp = UnityEngine.Input.GetMouseButtonUp(0);
            if (mouseDown) {
                currMouseScreenPos = Input.mousePosition;
                currMouseWorldPos = Camera.main.ScreenToWorldPoint((Vector3)Input.mousePosition);
            }
            if (mouseDrag) {
                prevMouseScreenPos = currMouseScreenPos;
                currMouseScreenPos = Input.mousePosition;

                prevMouseWorldPos = currMouseWorldPos;
                currMouseWorldPos = Camera.main.ScreenToWorldPoint((Vector3)Input.mousePosition);

                currentDragDeltaVectorScreen = GetMouseDeltaScreen();
                currentDragDeltaVectorWorld = GetMouseDeltaWorld();

                var deltaWorld = GetMouseDeltaWorld();
                var deltaScreen = GetMouseDeltaScreen();

                dragDeltaVectorWorld += deltaWorld;
                dragDeltaVectorScreen += deltaScreen;

                dragDeltaLengthScreen += deltaScreen.magnitude;
                dragDeltaLengthWorld += deltaWorld.magnitude;
            }
            IUpdate();
            foreach (var action in currentActions) {
                action.Update();
            }
            if (mouseUp) {
                currMouseScreenPos = null;
                prevMouseScreenPos = null;

                currMouseWorldPos = null;
                prevMouseWorldPos = null;

                dragDeltaVectorWorld = Vector2.zero;
                dragDeltaVectorScreen = Vector2.zero;

                dragDeltaLengthScreen = 0;
                dragDeltaLengthWorld = 0;
            }
        }
        private void OnDestroy() {
            IOnDestroy();
        }
        #endregion

        /// <summary>
        /// Default Awake() to override in inherited class.
        /// </summary>
        virtual protected void IAwake() {
        }
        /// <summary>
        /// Default Start() to override in inherited class.
        /// </summary>
        virtual protected void IStart() {
        }
        /// <summary>
        /// Default Update() to override in inherited class.
        /// </summary>
        virtual protected void IUpdate() {
        }
        /// <summary>
        /// Default OnDestroy() to override in inherited class.
        /// </summary>
        virtual protected void IOnDestroy() {
        }
    }
}