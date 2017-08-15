using UnityEngine;
using System.Collections.Generic;

namespace AInput {
    public sealed class CollidersInfo : BaseInfo {
        /// <summary>
        /// List of correct colliders.
        /// </summary>
        public List<Collider2D> correct { get; set; }
        /// <summary>
        /// List of uncorrect colliders.
        /// </summary>
        public List<Collider2D> uncorrect { get; set; }
        /// <summary>
        /// Check is collider correct.
        /// </summary>
        /// <param name="collider">Collider to check.</param>
        /// <returns>True if collider contained in correct colliders list.</returns>
        override public bool IsCorrect(Collider2D collider) {
            return correct.Contains(collider);
        }
        /// <summary>
        /// Check is collider uncorrect.
        /// </summary>
        /// <param name="collider">Collider to check.</param>
        /// <returns>True if collider contained in uncorrect colliders list.</returns>
        override public bool IsUncorrect(Collider2D collider) {
            return uncorrect.Contains(collider);
        }
        /// <summary>
        /// You can keep anything as null.
        /// </summary>
        /// <param name="correct">List of correct colliders.</param>
        /// <param name="uncorrect">List of uncorrect colliders.</param>
        public CollidersInfo(List<Collider2D> correct = null,
            List<Collider2D> uncorrect = null) {
            if (correct == null) {
                correct = new List<Collider2D>();
            }
            if (uncorrect == null) {
                uncorrect = new List<Collider2D>();
            }
            this.correct = new List<Collider2D>(correct);
            this.uncorrect = new List<Collider2D>(uncorrect);
        }
    }
}