using System.Reflection;
using System.Linq;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace AJSON {
    public interface IJSONDeserializable<T> {
        IJSONObject<T> Deserialize(string jsonValue);
    }
    public interface IJSONSerializable {
        string Serialize();
    }
    public interface IJSONObject<T> : IJSONSerializable, IJSONDeserializable<T> {
        T value { get; set; }
    }
    public abstract class JSONObject<T> : IJSONObject<T> {
        public T value { get; set; }
        public virtual IJSONObject<T> Deserialize(string jsonValue) {
            return default(IJSONObject<T>);
        }
        public virtual string Serialize() {
            return value.ToString();
        }
    }
    public class Float : JSONObject<float> {
        public override IJSONObject<float> Deserialize(string jsonValue) {
            if (AJSONExtensions.IsNumeric(jsonValue)) {
                value = float.Parse(jsonValue);
                return this;
            }
            Debug.Log("Can't Deserialize float " + jsonValue + ". Returning 0f by default.");
            value = 0f;
            return this;
        }
        public override string Serialize() {
            return value.ToString();
        }
        public Float() {
            value = 0f;
        }
        public Float(float value) {
            this.value = value;
        }
    }
    public class Integer : JSONObject<int> {
        public override IJSONObject<int> Deserialize(string jsonValue) {
            if (AJSONExtensions.IsInteger(jsonValue)) {
                value = int.Parse(jsonValue);
                return this;
            }
            Debug.Log("Can't Deserialize integer " + jsonValue + ". Returning 0 by default.");
            value = 0;
            return this;
        }
        public override string Serialize() {
            return value.ToString();
        }
        public Integer() {
            value = 0;
        }
        public Integer(int value) {
            this.value = value;
        }
    }
    public class Boolean : JSONObject<bool> {
        public override IJSONObject<bool> Deserialize(string jsonValue) {
            if (AJSONExtensions.IsBoolean(jsonValue)) {
                value = jsonValue == "true";
                return this;
            }
            Debug.Log("Can't Deserialize boolean " + jsonValue + ". Returning false by default.");
            value = false;
            return this;
        }
        public override string Serialize() {
            return value.ToString();
        }
        public Boolean() {
            value = false;
        }
        public Boolean(bool value) {
            this.value = value;
        }
    }
    public class String : JSONObject<string> {
        public override IJSONObject<string> Deserialize(string jsonValue) {
            if (AJSONExtensions.IsString(jsonValue)) {
                value = jsonValue;
                return this;
            }
            Debug.Log("Can't Deserialize string " + jsonValue + ". Returning \"\" by default.");
            value = "";
            return this;
        }
        public override string Serialize() {
            return "\"" + value.ToString() + "\"";
        }
        public String() {
            value = "";
        }
        public String(string value) {
            this.value = value;
        }
    }
    public class Array<T> : JSONObject<T[]> {
        public override IJSONObject<T[]> Deserialize(string jsonValue) {
            if (AJSONExtensions.IsArray(jsonValue)) {
                throw new System.NotImplementedException();
            }
            Debug.Log("Can't Deserialize array " + jsonValue + ". Returning empty " + typeof(T).ToString() + "[]  by default.");
            value = new T[0];
            return this;
        }
        public override string Serialize() {
            string returned = "[ ";
            if (value.Length > 1) {
                returned += new Object(value[0]).Serialize();
                for (int i = 1; i < value.Length; ++i) {
                    returned += ", " + new Object(value[i]).Serialize();
                }
            }
            returned += "]";
            return returned;
        }
        public Array() {
            this.value = new T[0];
        }
        public Array(T[] value) {
            this.value = value;
        }
    }
    public class Object : JSONObject<object> {
        public override IJSONObject<object> Deserialize(string jsonValue) {
            if (AJSONExtensions.IsObject(jsonValue)) {
                throw new System.NotImplementedException();
            }
            Debug.Log("Can't Deserialize object " + jsonValue + ". Returning null by default.");
            value = null;
            return this;
        }
        public override string Serialize() {
            string returned = "{\r";
            //var fields = value.GetType().GetFields();
            //for (int i = 0; i < fields.Length; i++) {
            //}
            //.ToList().ForEach(field => {
            //    var attributes = (JSONPropertyAttribute[])field.GetCustomAttributes(typeof(JSONPropertyAttribute), true);
            //    if (attributes.Length > 0) {
            //        JSONPropertyAttribute attribute = attributes[0];

            //        returned += "\"" + attribute.propertyName == "" ? field.Name : attribute.propertyName + "\":";

            //        switch (attribute.serializedType) {
            //            case SerializedType.Float:
            //                returned += new Float((float)field.GetValue(value)).Serialize();
            //                break;
            //            case SerializedType.Integer:
            //                returned += new Integer((int)field.GetValue(value)).Serialize();
            //                break;
            //            case SerializedType.Boolean:
            //                returned += new Boolean((bool)field.GetValue(value)).Serialize();
            //                break;
            //            case SerializedType.String:
            //                returned += new String((string)field.GetValue(value).ToString()).Serialize();
            //                break;
            //            case SerializedType.Array:
            //                //returned += new Array<T>((field.GetType())field.GetValue(value)).Serialize();
            //                break;
            //            case SerializedType.Object:
            //                returned += new Object(field.GetValue(value)).Serialize();
            //                break;
            //            default:
            //                throw new System.NotImplementedException();
            //        }
            //    }
            //});
            //value.GetType().GetProperties().ToList().ForEach(property => {
            //    var attributes = (JSONPropertyAttribute[])property.GetCustomAttributes(typeof(JSONPropertyAttribute), true);
            //    if (attributes.Length > 1) {
            //        JSONPropertyAttribute attribute = attributes[0];

            //        returned += "\"" + attribute.propertyName == "" ? property.Name : attribute.propertyName + "\":";

            //        switch (attribute.serializedType) {
            //            case SerializedType.Float:
            //                returned += new Float((float)property.GetValue(value, null)).Serialize();
            //                break;
            //            case SerializedType.Integer:
            //                returned += new Integer((int)property.GetValue(value, null)).Serialize();
            //                break;
            //            case SerializedType.Boolean:
            //                returned += new Boolean((bool)property.GetValue(value, null)).Serialize();
            //                break;
            //            case SerializedType.String:
            //                returned += new String((string)property.GetValue(value, null)).Serialize();
            //                break;
            //            case SerializedType.Array:
            //                //returned += new Array<T>((field.GetType())field.GetValue(value)).Serialize();
            //                break;
            //            case SerializedType.Object:
            //                returned += new Object(property.GetValue(value, null)).Serialize();
            //                break;
            //            default:
            //                throw new System.NotImplementedException();
            //        }
            //    }
            //});
            return returned + "}";
        }
        public Object() {
            value = null;
        }
        public Object(object value) {
            this.value = value;
        }
    }

    //public static class JSON {

    //    public static string Serialize(object serialized) {
    //        var type = serialized.GetType();
    //        string returned = "{";
    //        foreach (var field in type.GetFields()) {
    //            var attributes = field.GetCustomAttributes(typeof(JSONPropertyAttribute), true);
    //            if (attributes.Length > 0) {
    //                var attribute = (JSONPropertyAttribute)attributes[0];
    //                returned += SerializeMember(serialized, field, attribute);
    //            }
    //        }
    //        foreach (var field in type.GetProperties().Where(m => m.CanRead && m.CanWrite)) {
    //            var attributes = field.GetCustomAttributes(typeof(JSONPropertyAttribute), true);
    //            if (attributes.Length > 0) {
    //                var attribute = (JSONPropertyAttribute)attributes[0];
    //                returned += SerializeMember(serialized, field, attribute);
    //            }
    //        }
    //        returned += "}";
    //        return returned;
    //    }

    //    private static string SerializeMember(object serialized, FieldInfo field, JSONPropertyAttribute attribute) {
    //        var value = field.GetValue(serialized);
    //        string fieldToString = "\"" + (attribute.propertyName == "" ? field.Name : attribute.propertyName) + "\": ";
    //        string valueToString = SerializeValue(value, attribute);
    //        return fieldToString + valueToString + ",";
    //    }

    //    private static string SerializeMember(object serialized, PropertyInfo property, JSONPropertyAttribute attribute) {
    //        var value = property.GetValue(serialized, null);
    //        string propertyToString = "\"" + (attribute.propertyName == "" ? property.Name : attribute.propertyName) + "\": ";
    //        string valueToString = SerializeValue(value, attribute);
    //        return propertyToString + valueToString + ",";
    //    }

    //    private static string SerializeValue(object value, JSONPropertyAttribute attribute) {
    //        switch (attribute.serializedType) {
    //            case SerializedType.String:
    //                return '\"' + value.ToString() + '\"';
    //            case SerializedType.Number:
    //                return value.ToString();
    //            case SerializedType.Boolean:
    //                return value.ToString();
    //            case SerializedType.Array:
    //                return "DATA_TYPE_UNRECOGNIZED";
    //            case SerializedType.Dictionary:
    //                return "DATA_TYPE_UNRECOGNIZED";
    //            case SerializedType.Object:
    //                return Serialize(value);
    //            default:
    //                return "DATA_TYPE_UNRECOGNIZED";
    //        }
    //    }

    //    public static T Deserialize<T>(string json) {
    //        return default(T);
    //    }
    //}
}
