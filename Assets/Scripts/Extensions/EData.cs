using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Extensions {
    /// <summary>
    /// I/O Data extensions.
    /// </summary>
    public static class EData {
        /// <summary>
        /// Assets folder file saver.
        /// </summary>
        /// <param name="assetFolderPath">Path to file inside assets folder.</param>
        /// <param name="bytes">Saved bytes.</param>
        public static void Save(string assetFolderPath, byte[] bytes) {
            var path = Path.Combine(
#if UNITY_ANDROID
                Application.persistentDataPath
#else
                Application.dataPath
#endif
                , assetFolderPath);
            File.WriteAllBytes(path, bytes);
        }
        /// <summary>
        /// Assets folder file loader.
        /// </summary>
        /// <param name="assetFolderPath">Path to file inside assets folder.</param>
        /// <returns>File contents.</returns>
        public static byte[] Load(string assetFolderPath) {
            var path = Path.Combine(
#if UNITY_ANDROID
                Application.persistentDataPath
#else
                Application.dataPath
#endif
                , assetFolderPath);
            return File.ReadAllBytes(path);
        }
    }
}
