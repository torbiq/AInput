//#define FORCE_LOG_MOBILE
//#define USE_CONTEXT

using UnityEngine;

namespace AInput {
    /// <summary>
    /// Class that handles all debug logs in the game.
    /// </summary>
    public static class Log {
        /// <summary>
        /// Debug.Log() handle
        /// </summary>
        static public void Msg(object msg
#if USE_CONTEXT
	, Object context = null
#endif
    ) {
#if UNITY_EDITOR || (FORCE_LOG_MOBILE && (UNITY_ANDROID || UNITY_IOS))
#if USE_CONTEXT
        if (context) {
            Debug.Log("[ Logger ] : " + msg, context);
            return;
        }
#endif
            Debug.Log("[ Logger ] : " + msg);
#endif
        }
        /// <summary>
        /// Debug.LogWarning() handle
        /// </summary>
        static public void Warning(object msg
#if USE_CONTEXT
	, Object context = null
#endif
) {
#if UNITY_EDITOR || (FORCE_LOG_MOBILE && (UNITY_ANDROID || UNITY_IOS))
#if USE_CONTEXT
        if (context) {
            Debug.LogWarning("[ Logger ] : " + msg, context);
            return;
        }
#endif
            Debug.LogWarning("[ Logger ] : " + msg);
#endif
        }
        /// <summary>
        /// Debug.LogError() handle
        /// </summary>
        static public void Error(object msg
#if USE_CONTEXT
	, Object context = null
#endif
) {
#if UNITY_EDITOR || (FORCE_LOG_MOBILE && (UNITY_ANDROID || UNITY_IOS))
#if USE_CONTEXT
        if (context) {
            Debug.LogError("[ Logger ] : " + msg, context);
            return;
        }
#endif
            Debug.LogError("[ Logger ] : " + msg);
#endif
        }
    }
}