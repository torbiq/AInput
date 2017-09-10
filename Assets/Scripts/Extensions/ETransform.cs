using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Extensions {
    public static class ETransform {
        public static void LookAt2D(this Transform transform, Vector2 direction) {
            direction.Normalize();
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }
}
