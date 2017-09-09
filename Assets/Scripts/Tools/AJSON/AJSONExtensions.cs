using System;
using System.Collections.Generic;

namespace AJSON {
    public static class AJSONExtensions {
        /// <summary>
        /// Returns true if value string representation contains only digits or one seperator per string.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="seperatorFound">Seperator is found.</param>
        /// <param name="seperatorChar">Seperator symbol.</param>
        /// <returns>True if value string representation contains only digits or one seperator per string.</returns>
        public static bool IsNumeric(string value, out bool seperatorFound, char seperatorChar = '.') {
            seperatorFound = false;
            for (int i = 0; i < value.Length; ++i) {
                if (!Char.IsDigit(value[i])) {
                    if (value[i] == seperatorChar && !seperatorFound) {
                        seperatorFound = true;
                    }
                    else {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Returns true if value string representation contains only digits or one seperator per string.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="seperatorChar">Seperator symbol.</param>
        /// <returns>True if value string representation contains only digits or one seperator per string.</returns>
        public static bool IsNumeric(string value, char seperatorChar = '.') {
            bool dotFound = false;
            for (int i = 0; i < value.Length; ++i) {
                if (!Char.IsDigit(value[i])) {
                    if (value[i] == seperatorChar && !dotFound) {
                        dotFound = true;
                    }
                    else {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Returns true if value string representation contains only digits.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if value string representation contains only digits.</returns>
        public static bool IsInteger(string value) {
            for (int i = 0; i < value.Length; ++i) {
                if (!Char.IsDigit(value[i])) {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Returns true if value string representation is equal to true or false string example.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="trueString">True boolean string example.</param>
        /// <param name="falseString">False boolean string example.</param>
        /// <returns>True if value string representation is equal to true or false string example</returns>
        public static bool IsBoolean(string value, string trueString = "true", string falseString = "false") {
            if (value == falseString) {
                return true;
            }
            if (value == trueString) {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true if value string representation isn't empty and starts with "[" and ends with "]".
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if value string representation isn't empty and starts with "[" and ends with "]".</returns>
        public static bool IsArray(string value, char arrayOpenChar = '[', char arrayCloseChar = ']') {
            if (value.Length < 1) {
                return false;
            }
            return value[0] == arrayOpenChar && value[value.Length - 1] == arrayCloseChar;
        }
        /// <summary>
        /// Returns true if value string representation isn't empty and starts with "{" and ends with "}".
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param objectOpenChar="value">Object opening symbol.</param>
        /// <param objectCloseChar="value">Object closing symbol.</param>
        /// <returns>True if value string representation isn't empty and starts with "{" and ends with "}".</returns>
        public static bool IsObject(string value, char objectOpenChar = '{', char objectCloseChar = '}') {
            if (value.Length < 1) {
                return false;
            }
            return value[0] == objectOpenChar && value[value.Length - 1] == objectCloseChar;
        }
        /// <summary>
        /// Returns true if value string representation isn't empty and starts with '"' and ends with '"'.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param stringOpenChar="value">String opening symbol.</param>
        /// <param stringCloseChar="value">String closing symbol.</param>
        /// <returns>True if value string representation isn't empty and starts with '"' and ends with '"'.returns>
        public static bool IsString(string value, char stringOpenChar = '"', char stringCloseChar = '"') {
            if (value.Length < 1) {
                return false;
            }
            return value[0] == stringOpenChar && value[value.Length - 1] == stringCloseChar;
        }
        /// <summary>
        /// Returns true if value string representation equals to "null".
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="nullString">Null string example.</param>
        /// <returns>True if value string representation is a null.</returns>
        public static bool IsNull(string value, string nullString = "null") {
            return value == nullString;
        }
    }

    public static class JSONSerializer {
        //public static string Serialize<T>() where T : IJSONSerializable, new() {
        //    return new T().Serialize();
        //}
    }
}