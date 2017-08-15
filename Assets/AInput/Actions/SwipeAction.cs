using UnityEngine;
using System.Collections.Generic;
using System;

namespace AInput {
    public class SwipeAction : BaseAction {
        /// <summary>
        /// Called when finger touches screen.
        /// </summary>
        public Action OnSwipeStarted;
        /// <summary>
        /// Called when finger moves through the screen.
        /// </summary>
        public Action OnSwipe;
        /// <summary>
        /// Called when you are touching correct collider.
        /// </summary>
        public Action<Collider2D> OnSwipeTouchedCorrect;
        /// <summary>
        /// Called when you are touching correct collider.
        /// </summary>
        public Action<Collider2D> OnSwipeTouchedUncorrect;
        /// <summary>
        /// Called when you release mouse on screen or touch uncorrect collider or finish swipe.
        /// </summary>
        public Action OnSwipeStopped;
        /// <summary>
        /// Called when resulting direction is contained in correct directions.
        /// </summary>
        public Action<Direction> OnSwipedCorrect;
        /// <summary>
        /// Called when resulting direction is not contained in correct directions.
        /// </summary>
        public Action<Direction> OnSwipedUncorrect;
        /// <summary>
        /// Called when resulting direction is not contained in correct directions.
        /// </summary>
        public Action<Direction> OnSwipeFinished;
        /// <summary>
        /// Distance needed to record swipe.
        /// </summary>
        public float swipeRecordableDistance { get; private set; }
        /// <summary>
        /// If null - swipe works everywhere, if set - it checks every collider touched for this one.
        /// </summary>
        public BaseInfo swipableInfo { get; private set; }
        /// <summary>
        /// List of correct directions for action.
        /// </summary>
        public List<Direction> correct { get; private set; }
        /// <summary>
        /// List of uncorrect directions for action.
        /// </summary>
        public List<Direction> uncorrect { get; private set; }
        /// <summary>
        /// Is swiping goes on.
        /// </summary>
        private bool _isSwiping;
        /// <summary>
        /// Swipable directions.
        /// </summary>
        public enum Direction {
            Right = 0,      // 0 * 45 = 0 deg
            UpRight = 1,    // 1 * 45 = 45 deg
            Up = 2,         // 2 * 45 = 90 deg
            UpLeft = 3,     // 3 * 45 = 135 deg
            Left = 4,       // 4 * 45 = 180 deg
            DownLeft = 5,   // 5 * 45 = 225 deg
            Down = 6,       // 6 * 45 = 270 deg
            DownRight = 7,  // 7 * 45 = 315 deg
        }
        #region Static
        /// <summary>
        /// All directions info.
        /// </summary>
        public static Dictionary<Direction, float> directionsInfo { get; private set; }
        /// <summary>
        /// Returns closest direction by angle.
        /// </summary>
        public static Direction GetDirection(float angle) {
            float minDistance = Mathf.Abs(Mathf.DeltaAngle(angle, 360f));
            Direction bestDirection = Direction.Right;
            foreach (var info in directionsInfo) {
                float distance = Mathf.Abs(Mathf.DeltaAngle(angle, info.Value));
                if (distance < minDistance) {
                    minDistance = distance;
                    bestDirection = info.Key;
                }
            }
            return bestDirection;
        }
        /// <summary>
        /// Returns closest direction by vector.
        /// </summary>
        public static Direction GetDirection(Vector2 delta) {
            return GetDirection(Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);
        }
        static SwipeAction() {
            directionsInfo = new Dictionary<Direction, float> {
                { Direction.Right, 0f },
                { Direction.UpRight, 45f },
                { Direction.Up, 90f },
                { Direction.UpLeft, 135f },
                { Direction.Left, 180f },
                { Direction.DownLeft, 225f },
                { Direction.Down, 270f },
                { Direction.DownRight, 315f },
            };
        }
        #endregion
        public SwipeAction(Action OnSwipeStarted = null,
            Action OnSwipe = null,
            Action<Collider2D> OnSwipeTouchedCorrect = null,
            Action<Collider2D> OnSwipeTouchedUncorrect = null,
            Action OnSwipeStopped = null,
            Action<Direction> OnSwipedCorrect = null,
            Action<Direction> OnSwipedUncorrect = null,
            Action<Direction> OnSwipeFinished = null,
            float swipeRecordableDistance = 4f,
            BaseInfo swipableInfo = null,
            List<Direction> correct = null,
            List<Direction> uncorrect = null) {
            this.OnSwipeStarted += OnSwipeStarted;
            this.OnSwipe += OnSwipe;
            this.OnSwipeTouchedCorrect += OnSwipeTouchedCorrect;
            this.OnSwipeTouchedUncorrect += OnSwipeTouchedUncorrect;
            this.OnSwipeStopped += OnSwipeStopped;
            this.OnSwipedCorrect += OnSwipedCorrect;
            this.OnSwipedUncorrect += OnSwipedUncorrect;
            this.OnSwipeFinished += OnSwipeFinished;
            this.swipeRecordableDistance = swipeRecordableDistance;
            this.swipableInfo = swipableInfo;
            this.correct = correct;
            this.uncorrect = uncorrect;
            if (correct == null) {
                correct = new List<Direction>();
            }
            if (uncorrect == null) {
                uncorrect = new List<Direction>();
            }
        }
        private void StartSwiping() {
            _isSwiping = true;
            if (OnSwipeStarted != null) {
                OnSwipeStarted();
            }
        }
        private void CheckTouchedColliders() {
            var touchedColliders = inputController.TouchedCollidersWorld();
            bool correctTouched = false;
            bool uncorrectTouched = false;
            int length = touchedColliders.Length;
            for(int i = 0;i < length; ++i) {
                var collider = touchedColliders[i];
                if (swipableInfo.IsCorrect(collider)) {
                    correctTouched = true;
                    if (OnSwipeTouchedCorrect != null) {
                        OnSwipeTouchedCorrect(collider);
                    }
                }
                if (swipableInfo.IsUncorrect(collider)) {
                    uncorrectTouched = true;
                    if (OnSwipeTouchedUncorrect != null) {
                        OnSwipeTouchedUncorrect(collider);
                    }
                }
            }
            if (!correctTouched || uncorrectTouched) {
                StopSwiping();
                return;
            }
        }
        private void SwipingFinished() {
            Direction dirFinal = GetDirection(inputController.dragDeltaVectorWorld);
            if (OnSwipeFinished != null) {
                OnSwipeFinished(dirFinal);
            }
            if (uncorrect.Contains(dirFinal)) {
                if (OnSwipedUncorrect != null) {
                    OnSwipedUncorrect(dirFinal);
                }
                return;
            }
            if (correct.Contains(dirFinal)) {
                if (OnSwipedCorrect != null) {
                    OnSwipedCorrect(dirFinal);
                }
                return;
            }
        }
        private void StopSwiping() {
            _isSwiping = false;
            if (OnSwipeStopped != null) {
                OnSwipeStopped();
            }
        }
        public override void Update() {
            if (inputController.mouseDown) {
                StartSwiping();
            }
            if (inputController.mouseDrag && _isSwiping) {
                if (OnSwipe != null) {
                    OnSwipe();
                }
                if (swipableInfo != null) {
                    CheckTouchedColliders();
                }
                if (_isSwiping) {
                    if (inputController.dragDeltaVectorWorld.magnitude >= swipeRecordableDistance) {
                        StopSwiping();
                        SwipingFinished();
                    }
                }
            }
            if (inputController.mouseUp && _isSwiping) {
                StopSwiping();
            }
        }
    }
}
