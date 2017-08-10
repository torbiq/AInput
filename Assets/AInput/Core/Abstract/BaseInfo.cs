using UnityEngine;

namespace AInput.Info {
    public abstract class BaseInfo {
        abstract public bool IsUncorrect(Collider2D collider);
        abstract public bool IsCorrect(Collider2D collider);
    }
}
