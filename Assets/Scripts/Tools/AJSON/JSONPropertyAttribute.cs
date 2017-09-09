using System;

namespace AJSON {
    public enum SerializedType {
        Float = 0,
        Integer = 1,
        Boolean = 2,
        String = 3,
        Array = 4,
        Object = 5,
        Number = Float | Integer,
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class JSONPropertyAttribute : Attribute {
        public string propertyName = "";
        public SerializedType serializedType = SerializedType.String;
        public JSONPropertyAttribute(string propertyName = "", SerializedType serializedType = SerializedType.Number) {
            this.propertyName = propertyName;
            this.serializedType = serializedType;
        }
    }
}