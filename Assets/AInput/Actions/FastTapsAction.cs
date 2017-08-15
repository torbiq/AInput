using UnityEngine;
using System.Collections.Generic;
using System;

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
        public FastTapsAction(BaseInfo tappableInfo,
            Action<Collider2D> OnTap = null,
            Action OnProgressComplete = null,
            Action OnProgressFallenToMin = null,
            float progressPerTap = 0.3f,
            float progressRemovedPerSecond = 0.35f,
            float minProgress = 0.0f,
            float maxProgress = 1.0f,
            bool isProgressTimeScalable = true,
            bool isProgressDecreasable = true) {
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
