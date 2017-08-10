using UnityEngine;
using System.Collections.Generic;
using System;
using AInput.Info;

namespace AInput {
    public class FastTapsAction : BaseAction {
        public Action OnProgressFallenToMin;
        public Action OnProgressCompleteToMax;
        public Action<Collider2D> OnTap;
        public BaseInfo tappableInfo { get; private set; }
        public float minProgress { get; private set; }
        public float maxProgress { get; private set; }
        public float progress { get; private set; }
        public float progressPerTap { get; private set; }
        public float progressRemovedPerSecond { get; private set; }
        public bool isProgressDecreasing { get; private set; }
        public bool isProgressTimeScalable { get; private set; }
        public bool isProgressDecreasable { get; private set; }
        #region Constructors
        public FastTapsAction(BaseInfo tappableInfo,
            Action<Collider2D> OnTap,
            Action OnProgressCompleteToMax,
            Action OnProgressFallenToMin,
            float progressPerTap,
            float progressRemovedPerSecond,
            float minProgress,
            float maxProgress,
            bool isProgressTimeScalable,
            bool isProgressDecreasable) {
            this.tappableInfo = tappableInfo;
            this.OnTap += OnTap;
            this.OnProgressCompleteToMax += OnProgressCompleteToMax;
            this.OnProgressFallenToMin += OnProgressFallenToMin;
            this.progressPerTap = progressPerTap;
            this.progressRemovedPerSecond = progressRemovedPerSecond;
            this.minProgress = minProgress;
            this.maxProgress = maxProgress;
            this.isProgressTimeScalable = isProgressTimeScalable;
            this.isProgressDecreasable = isProgressDecreasable;
            this.isProgressDecreasing = false;
            this.progress = minProgress;
            if (minProgress >= maxProgress) {
                throw new Exception("minProgress >= maxProgress");
            }
            if (progress >= maxProgress) {
                throw new Exception("progress >= maxProgress in constructor");
            }
            if (isProgressDecreasable && OnProgressFallenToMin != null) {
                Log.Warning("OnProgressFallenToMin action is being listened, but progress isn't decreasable.");
            }
        }
        public static FastTapsAction Colliders(List<Collider2D> correctColliders = null,
            Action<Collider2D> OnTap = null,
            Action OnProgressComplete = null,
            Action OnProgressFallenToMin = null,
            float progressPerTap = 0.3f,
            float progressRemovedPerSecond = 0.35f,
            float minProgress = 0.0f,
            float maxProgress = 1.0f,
            bool isProgressTimeScalable = true,
            bool isProgressDecreasable = true) {
            return new FastTapsAction(new CollidersInfo(collidersNeeded: correctColliders),
                OnTap: OnTap,
                OnProgressCompleteToMax: OnProgressComplete,
                OnProgressFallenToMin: OnProgressFallenToMin,
                progressPerTap: progressPerTap,
                progressRemovedPerSecond: progressRemovedPerSecond,
                minProgress: minProgress,
                maxProgress: maxProgress,
                isProgressTimeScalable: isProgressTimeScalable,
                isProgressDecreasable: isProgressDecreasable);
        }
        public static FastTapsAction NamePart(string correctNamePart = "",
            Action<Collider2D> OnTap = null,
            Action OnProgressComplete = null,
            Action OnProgressFallenToMin = null,
            float progressPerTap = 0.3f,
            float progressRemovedPerSecond = 0.35f,
            float minProgress = 0.0f,
            float maxProgress = 1.0f,
            bool isProgressTimeScalable = true,
            bool isProgressDecreasable = true) {
            return new FastTapsAction(new NamePartInfo(namePartNeeded: correctNamePart),
                OnTap: OnTap,
                OnProgressCompleteToMax: OnProgressComplete,
                OnProgressFallenToMin: OnProgressFallenToMin,
                progressPerTap: progressPerTap,
                progressRemovedPerSecond: progressRemovedPerSecond,
                minProgress: minProgress,
                maxProgress: maxProgress,
                isProgressTimeScalable: isProgressTimeScalable,
                isProgressDecreasable: isProgressDecreasable);
        }
        public static FastTapsAction FullNames(List<string> correctNames = null,
            Action<Collider2D> OnTap = null,
            Action OnProgressComplete = null,
            Action OnProgressFallenToMin = null,
            float progressPerTap = 0.3f,
            float progressRemovedPerSecond = 0.35f,
            float minProgress = 0.0f,
            float maxProgress = 1.0f,
            bool isProgressTimeScalable = true,
            bool isProgressDecreasable = true) {
            return new FastTapsAction(new FullNamesInfo(fullNamesNeeded: correctNames),
                OnTap: OnTap,
                OnProgressCompleteToMax: OnProgressComplete,
                OnProgressFallenToMin: OnProgressFallenToMin,
                progressPerTap: progressPerTap,
                progressRemovedPerSecond: progressRemovedPerSecond,
                minProgress: minProgress,
                maxProgress: maxProgress,
                isProgressTimeScalable: isProgressTimeScalable,
                isProgressDecreasable: isProgressDecreasable);
        }
        #endregion
        public override void Update() {
            if (inputController.mouseDown) {
                if (progress < maxProgress) {
                    foreach (Collider2D collider in inputController.TouchedCollidersWorld()) {
                        if (tappableInfo.IsCorrect(collider)) {
                            if (OnTap != null) {
                                OnTap(collider);
                            }
                            if (isProgressDecreasable) {
                                isProgressDecreasing = true;
                            }
                            progress += progressPerTap;
                            break;
                        }
                    }
                }
                if (progress > maxProgress) {
                    progress = maxProgress;
                    isProgressDecreasing = false;
                    if (OnProgressCompleteToMax != null) {
                        OnProgressCompleteToMax();
                    }
                }
            }
            if (isProgressDecreasing) {
                progress -= progressRemovedPerSecond * (isProgressTimeScalable ? Time.deltaTime : Time.unscaledDeltaTime);

                if (progress < minProgress) {
                    progress = minProgress;
                    isProgressDecreasing = false;
                    if (OnProgressFallenToMin != null) {
                        OnProgressFallenToMin();
                    }
                }
            }
        }
    }
}
