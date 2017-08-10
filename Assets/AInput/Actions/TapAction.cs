using UnityEngine;
using System.Collections.Generic;
using System;
using AInput.Info;

namespace AInput {
    public class TapAction : BaseAction {
        public Action<Collider2D> OnTapWrong;
        public Action<Collider2D> OnTapCorrect;
        public BaseInfo tappableInfo { get; private set; }
        #region Constructors
        public TapAction(BaseInfo tappableInfo, Action<Collider2D> OnTapWrong, Action<Collider2D> OnTapCorrect) {
            this.tappableInfo = tappableInfo;
            this.OnTapWrong += OnTapWrong;
            this.OnTapCorrect += OnTapCorrect;
        }
        public static TapAction Colliders(List<Collider2D> correctColliders,
            List<Collider2D> wrongColliders = null,
            Action<Collider2D> OnTapWrong = null,
            Action<Collider2D> OnTapCorrect = null) {
            return new TapAction(new CollidersInfo(collidersNeeded: correctColliders,
                collidersCanBeUsed: wrongColliders),
                OnTapWrong: OnTapWrong,
                OnTapCorrect: OnTapCorrect);
        }
        public static TapAction NamePart(string correctNamePart,
            string wrongNamePart = "",
            Action<Collider2D> OnTapWrong = null,
            Action<Collider2D> OnTapCorrect = null) {
            return new TapAction(new NamePartInfo(namePartNeeded: correctNamePart,
                namePartCanBeUsed: wrongNamePart),
                OnTapWrong: OnTapWrong,
                OnTapCorrect: OnTapCorrect);
        }
        public static TapAction FullNames(List<string> correctNames,
            List<string> wrongNames = null,
            Action<Collider2D> OnTapWrong = null,
            Action<Collider2D> OnTapCorrect = null) {
            return new TapAction(new FullNamesInfo(fullNamesNeeded: correctNames,
                fullNamesCanBeUsed: wrongNames),
                OnTapWrong: OnTapWrong,
                OnTapCorrect: OnTapCorrect);
        }
        #endregion
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
