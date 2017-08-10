using System.Collections.Generic;
using UnityEngine;

namespace AInput.Info {
    public sealed class FullNamesInfo : BaseInfo {
        public List<string> fullNamesNeeded { get; set; }
        public List<string> fullNamesCanBeUsed { get; set; }
        override public bool IsCorrect(Collider2D compairedCollider) {
            return fullNamesNeeded.Contains(compairedCollider.name);
        }
        override public bool IsUncorrect(Collider2D compairedCollider) {
            return fullNamesCanBeUsed.Contains(compairedCollider.name);
        }
        public FullNamesInfo(List<string> fullNamesNeeded,
            List<string> fullNamesCanBeUsed = null) {
            if (fullNamesNeeded == null) {
                fullNamesNeeded = new List<string>();
            }
            if (fullNamesCanBeUsed == null) {
                fullNamesCanBeUsed = new List<string>();
            }
            this.fullNamesNeeded = new List<string>(fullNamesNeeded);
            this.fullNamesCanBeUsed = new List<string>(fullNamesCanBeUsed);
        }
    }
}