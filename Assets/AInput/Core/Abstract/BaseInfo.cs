using UnityEngine;

namespace AInput {
    /// <summary>
    /// Base info to realise in inherited class.
    /// </summary>
    public abstract class BaseInfo {
        /// <summary>
        /// Checking is collider uncorrect.
        /// </summary>
        /// <param name="collider">Collider to check</param>
        /// <returns>True if it's uncorrect.</returns>
        abstract public bool IsUncorrect(Collider2D collider);
        /// <summary>
        /// Checking is collider correct.
        /// </summary>
        /// <param name="collider">Collider to check</param>
        /// <returns>True if it's correct.</returns>
        abstract public bool IsCorrect(Collider2D collider);
    }
}
