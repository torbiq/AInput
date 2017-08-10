using System;
using UnityEngine;

namespace AInput.Info {
    public sealed class NamePartInfo : BaseInfo {
        public string namePartNeeded { get; set; }
        public string namePartCanBeUsed { get; set; }
        override public bool IsCorrect(Collider2D collider) {
            return collider.name.Contains(namePartNeeded) && namePartNeeded != "";
        }
        override public bool IsUncorrect(Collider2D collider) {
            return collider.name.Contains(namePartCanBeUsed) && namePartCanBeUsed != "";
        }
        public NamePartInfo(string namePartNeeded,
            string namePartCanBeUsed = null) {
            this.namePartNeeded = namePartNeeded;
            this.namePartCanBeUsed = namePartCanBeUsed;
        }
    }
}
