using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SynaptikonFramework.Util
{
	/// <summary>
	/// Tolerant Enum converter.
    /// 
    /// If the value found in the JSON matches the enum (either as a string or an integer), that value is used. (If the value is integer and there are multiple possible matches, the first of those is used.)
    /// Otherwise if the enum type is nullable, then the value is set to null.
    /// Otherwise if the enum has a value called "Unknown" or "unknown", then that value is used.
    /// Otherwise the first value of the enum is used.
    /// </summary>
    /// <seealso cref="https://gist.github.com/gubenkoved/999eb73e227b7063a67a50401578c3a7"/>
    /// <remarks>
	/// Based on code in the StackOverflow post below, but adds EnumMember attribute support.
	/// http://stackoverflow.com/questions/22752075/how-can-i-ignore-unknown-enum-values-during-json-deserialization    /// 
    /// </remarks>
	public class TolerantEnumConverter : JsonConverter
    {
        private readonly Dictionary<Type, Dictionary<string, object>> _fromValueMap = new Dictionary<Type, Dictionary<string, object>>(); // string representation to Enum value map

        private readonly Dictionary<Type, Dictionary<object, string>> _toValueMap = new Dictionary<Type, Dictionary<object, string>>(); // Enum value to string map

        public TolerantEnumConverter(IFormattable unknownValue) : this ()
        {
            UnknownValue = unknownValue.ToString();
        }

        public TolerantEnumConverter() : base()
        {
        }

        public string UnknownValue { get; set; } = "unknown";

        public override bool CanConvert(Type objectType)
        {
            Type type = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
#if UNITY_WEBGL
            return false;
#else
            return type.GetTypeInfo().IsEnum;
#endif
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isNullable = IsNullableType(objectType);
            Type enumType = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

            if (reader.TokenType == JsonToken.StartArray)
            {
                Type listElementType;
                if(objectType.IsArray){
                    listElementType = objectType.GetElementType();
                }else{
                    var temp = objectType.GetTypeInfo().IsGenericTypeDefinition ? objectType.GetTypeInfo().GenericTypeParameters : objectType.GetTypeInfo().GenericTypeArguments;
                    listElementType = temp.First();
                }
                InitMap(listElementType);

                JToken token = JToken.Load(reader);
                List<string> items = token.ToObject<List<string>>();
                var ret = Array.CreateInstance(listElementType, items.Count);
                for (var i = 0; i < items.Count; i++){
                    var enumVal = FromValue(listElementType, items[i]);
                    if(enumVal == null){
                        enumVal = HandleUnknown(reader, listElementType);
                    }
                    //ret[i] = enumVal;
                    ret.SetValue(enumVal, i);
                }
                if(objectType.IsArray){
                    return ret;
                }else{
                    return Activator.CreateInstance(objectType, ret);
                }
            }
            InitMap(enumType);
            if (reader.TokenType == JsonToken.String)
            {
                string enumText = reader.Value.ToString();

                object val = FromValue(enumType, enumText);

                if (val != null)
                    return val;
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                int enumVal = Convert.ToInt32(reader.Value);
                int[] values = (int[])Enum.GetValues(enumType);
                if (values.Contains(enumVal))
                {
                    return Enum.Parse(enumType, enumVal.ToString());
                }
            }

            if (!isNullable)
            {
                return HandleUnknown(reader, enumType);
            }

            return null;
        }     

        private object HandleUnknown(JsonReader reader, Type enumType){
            string[] names = Enum.GetNames(enumType);

            string unknownName = names.FirstOrDefault(n => string.Equals(n, UnknownValue, StringComparison.OrdinalIgnoreCase));

            if (unknownName == null)
            {
                throw new JsonSerializationException($"Unable to parse '{reader.Value}' to enum {enumType}. Consider adding Unknown as fail-back value.");
            }

            return Enum.Parse(enumType, unknownName);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type enumType;
            if(value.GetType().IsArray || value is ICollection){
                writer.WriteStartArray();

                if (value.GetType().IsArray)
                {
                    enumType = value.GetType().GetElementType();
                    InitMap(enumType);

                    var array = value as Array;
                    foreach (var v in array)
                    {
                        var val = ToValue(enumType, v);
                        writer.WriteValue(val);
                    }
                }
                else
                {
                    var temp = value.GetType().GetTypeInfo().IsGenericTypeDefinition ? value.GetType().GetTypeInfo().GenericTypeParameters : value.GetType().GetTypeInfo().GenericTypeArguments;
                    enumType = temp.First();
                    InitMap(enumType);

                    var list = value as ICollection;
                    foreach (var v in list)
                    {
                        var val = ToValue(enumType, v);
                        writer.WriteValue(val);
                    }
                }

                writer.WriteEndArray();
            }else{ 
                enumType = value.GetType();
                InitMap(enumType);
                var val = ToValue(enumType, value);
                writer.WriteValue(val);

            }


        }

        #region Private methods
        private bool IsNullableType(Type t)
        {
            return t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private void InitMap(Type enumType)
        {
            if (_fromValueMap.ContainsKey(enumType))
                return; // already initialized

            Dictionary<string, object> fromMap = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            Dictionary<object, string> toMap = new Dictionary<object, string>();

#if UNITY_WEBGL
            IEnumerable<FieldInfo> fields = enumType.GetFields(
                BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static);
#else
            IEnumerable<FieldInfo> fields = enumType.GetRuntimeFields().Where((FieldInfo arg) => arg.IsPublic && arg.IsStatic);
#endif

            foreach (FieldInfo field in fields)
            {
                string name = field.Name;
                object enumValue = Enum.Parse(enumType, name);

                // use EnumMember attribute if exists
                EnumMemberAttribute enumMemberAttrbiute = field.GetCustomAttribute<EnumMemberAttribute>();

                if (enumMemberAttrbiute != null)
                {
                    string enumMemberValue = enumMemberAttrbiute.Value;

                    fromMap[enumMemberValue] = enumValue;
                    toMap[enumValue] = enumMemberValue;
                }
                else
                {
                    toMap[enumValue] = name;
                }

                fromMap[name] = enumValue;  
            }

            _fromValueMap[enumType] = fromMap;
            _toValueMap[enumType] = toMap;
        }

        private object ToValue(Type enumType, object obj)
        {
            Dictionary<object, string> map = _toValueMap[enumType];

            int intValue;
            bool isNumeric = int.TryParse(map[obj], out intValue);


            if (isNumeric)
            {
                return intValue;
            }
            else
            {
                return map[obj];
            }
        }

        private object FromValue(Type enumType, string value)
        {
            Dictionary<string, object> map = _fromValueMap[enumType];

            if (!map.ContainsKey(value))
                return null;

            return map[value];
        }
        #endregion
    }
}