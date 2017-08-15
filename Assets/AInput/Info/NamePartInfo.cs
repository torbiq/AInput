using System;
using UnityEngine;

namespace AInput {
    public sealed class NamePartInfo : BaseInfo {
        /// <summary>
        /// Correct name part.
        /// </summary>
        public string correct { get; set; }
        /// <summary>
        /// Uncorrect name part.
        /// </summary>
        public string uncorrect { get; set; }
        /// <summary>
        /// Check is collider correct.
        /// </summary>
        /// <param name="collider2d">Collider to check.</param>
        /// <returns>True if collider name contains correct name part.</returns>
        override public bool IsCorrect(Collider2D collider) {
            return collider.name.Contains(correct) && correct != "";
        }
        /// <summary>
        /// Check is collider uncorrect.
        /// </summary>
        /// <param name="collider2d">Collider to check.</param>
        /// <returns>True if collider name contains uncorrect name part.</returns>
        override public bool IsUncorrect(Collider2D collider) {
            return collider.name.Contains(uncorrect) && uncorrect != "";
        }
        /// <summary>
        /// You can keep anything as null.
        /// </summary>
        /// <param name="correct">Correct collider name part.</param>
        /// <param name="uncorrect">Uncorrect collider name part.</param>
        public NamePartInfo(string correct = null,
            string uncorrect = null) {
            this.correct = correct;
            this.uncorrect = uncorrect;
            if (this.correct == null) {
                this.correct = "";
            }
            if (this.uncorrect == null) {
                this.uncorrect = "";
            }
        }
    }
}
