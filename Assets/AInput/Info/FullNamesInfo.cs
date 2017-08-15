using System.Collections.Generic;
using UnityEngine;

namespace AInput {
    public sealed class FullNamesInfo : BaseInfo {
        /// <summary>
        /// Correct colliders full names list.
        /// </summary>
        public List<string> correct { get; set; }
        /// <summary>
        /// Uncorrect colliders full names list.
        /// </summary>
        public List<string> uncorrect { get; set; }
        /// <summary>
        /// Check is collider correct.
        /// </summary>
        /// <param name="collider2d">Collider to check.</param>
        /// <returns>True if collider name is contained in correct names list.</returns>
        override public bool IsCorrect(Collider2D collider) {
            return correct.Contains(collider.name);
        }
        /// <summary>
        /// Check is collider uncorrect.
        /// </summary>
        /// <param name="collider2d">Collider to check.</param>
        /// <returns>True if collider name is contained in uncorrect names list.</returns>
        override public bool IsUncorrect(Collider2D collider) {
            return uncorrect.Contains(collider.name);
        }
        /// <summary>
        /// You can keep anything as null.
        /// </summary>
        /// <param name="correct">Correct colliders full names list.</param>
        /// <param name="uncorrect">Uncorrect colliders full names list.</param>
        public FullNamesInfo(List<string> correct = null,
            List<string> uncorrect = null) {
            if (correct == null) {
                correct = new List<string>();
            }
            if (uncorrect == null) {
                uncorrect = new List<string>();
            }
            this.correct = new List<string>(correct);
            this.uncorrect = new List<string>(uncorrect);
        }
    }
}