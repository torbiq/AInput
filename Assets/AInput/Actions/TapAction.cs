using UnityEngine;
using System.Collections.Generic;
using System;

namespace AInput {
    public class TapAction : BaseAction {
        public Action<Collider2D> OnTapWrong;
        public Action<Collider2D> OnTapCorrect;
        public BaseInfo tappableInfo { get; private set; }
        public TapAction(BaseInfo tappableInfo,
            Action<Collider2D> OnTapWrong = null,
            Action<Collider2D> OnTapCorrect = null) {
            this.tappableInfo = tappableInfo;
            this.OnTapWrong += OnTapWrong;
            this.OnTapCorrect += OnTapCorrect;
        }
        public override void Update() {
            if (inputController.mouseDown) {
                foreach (Collider2D collider in inputController.TouchedCollidersWorld()) {
                    if (tappableInfo.IsCorrect(collider)) {
                        if (OnTapCorrect != null) {
                            OnTapCorrect(collider);
                        }
                        break;
                    }
                    if (tappableInfo.IsUncorrect(collider)) {
                        if (OnTapWrong != null) {
                            OnTapWrong(collider);
                        }
                        break;
                    }
                }
            }
        }
    }
}
