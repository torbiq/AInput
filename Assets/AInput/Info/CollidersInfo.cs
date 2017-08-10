using UnityEngine;
using System.Collections.Generic;

namespace AInput.Info {
    public sealed class CollidersInfo : BaseInfo {
        public List<Collider2D> collidersNeeded { get; set; }
        public List<Collider2D> collidersCanBeUsed { get; set; }
        override public bool IsCorrect(Collider2D collider2d) {
            return collidersNeeded.Contains(collider2d);
        }
        override public bool IsUncorrect(Collider2D collider2d) {
            return collidersCanBeUsed.Contains(collider2d);
        }
        public CollidersInfo(List<Collider2D> collidersNeeded = null,
            List<Collider2D> collidersCanBeUsed = null) {
            if (collidersNeeded == null) {
                collidersNeeded = new List<Collider2D>();
            }
            if (collidersCanBeUsed == null) {
                collidersCanBeUsed = new List<Collider2D>();
            }
            this.collidersNeeded = new List<Collider2D>(collidersNeeded);
            this.collidersCanBeUsed = new List<Collider2D>(collidersCanBeUsed);
        }
    }
}