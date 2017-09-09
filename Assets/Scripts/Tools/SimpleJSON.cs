using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Unity.Helpers {
    /// <summary>
    /// Converts any object into a JSON String.
    ///  
    /// Usage:
    ///
    /// string objJSON = JsonHelper.Json(myObject);
    /// string scrJSON = JsonHelper.Json(myScript);
    /// string arrayJSON = JsonHelper.Json(myArray);
    ///
    /// </summary>
    public static class JsonHelper {
        public static string Json(object obj) {
            if (obj == null)
                return "null";

            var type = obj.GetType();

            if (type.IsPrimitive)
                return obj.ToString();

            if (type == typeof(string)) {
                return "'" + obj.ToString() + "'";
            }

            if (type.IsArray)
                return JsonArray(obj as Array);

            if (type.IsClass)
                return JsonClass(obj);

            return obj.ToString();
        }

        private static string JsonClass(object obj) {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            int i = 0;
            foreach (var prop in GetProps(obj)) {
                object value = (prop is PropertyInfo) ?
                    (prop as PropertyInfo).GetValue(obj, null) :
                    (prop as FieldInfo).GetValue(obj);

                if (i++ > 0)
                    sb.Append(", ");
                sb.Append(prop.Name);
                sb.Append(": ");
                sb.Append(Json(value));
            }
            sb.Append("}");

            return sb.ToString();
        }

        private static IEnumerable<MemberInfo> GetProps(object obj) {
            Type t = obj.GetType();
            List<MemberInfo> result = new List<MemberInfo>();

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

#if UNITY
                if (t.BaseType == typeof(MonoBehaviour) || t.BaseType == typeof(ScriptableObject))
                {
                    flags = flags | BindingFlags.DeclaredOnly;
                }
#endif

            foreach (PropertyInfo prop in t
                    .GetProperties(flags)
                    .Where(m => m.CanRead && m.CanWrite)
                ) {
                //if (HasAttribute<Ignore>(prop, true))
                //    continue;
                result.Add(prop);
            }

            foreach (FieldInfo prop in t.GetFields(flags)) {
                //if (HasAttribute<Ignore>(prop, true))
                //    continue;

                result.Add(prop);
            }


            return result;
        }

        private static string JsonArray(Array array) {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            int i = 0;
            foreach (object val in array) {
                if (i++ > 0)
                    sb.Append(", ");

                sb.Append(Json(val));
            }
            sb.Append("]");
            return sb.ToString();
        }

    }
}
