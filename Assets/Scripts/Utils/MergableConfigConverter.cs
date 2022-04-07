using System;
using Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Data
{
    public class MergeableConfigConverter : JsonConverter
    {
        // USAGE
        // [Button]
        // void Save()
        // {
        //     mergeables.SetCtx();
        //     var settings = new JsonSerializerSettings();
        //     settings.Converters.Add(new MergeableConfigConverter(mergeables) );
        //     // JsonSerializerSettings settings = new JsonSerializerSettings();
        //     // settings.ContractResolver = new ScriptableObjectCreator();
        //     
        //     var str = JsonConvert.SerializeObject(itemData, settings);
        //     Debug.Log(str);
        //     itemDataOut = JsonConvert.DeserializeObject<MergeableItemData>(str, settings);
        //     Debug.Log("Done");
        // }

        
        private MergeConfig _mergeConfig;

        public MergeableConfigConverter(MergeConfig mergeConfig)
        {
            this._mergeConfig = mergeConfig;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(MergeItemConfig);
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var name = (string)serializer.Deserialize(reader);
            var scriptable = _mergeConfig.Get(name);
            return (object) scriptable;
        }
        public MergeItemConfig Create(Type objectType)
        {
            return MergeItemConfig.CreateInstance<MergeItemConfig>();
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // if(value is MergeItemProfileData)
            //     return;
            ScriptableObject v = (ScriptableObject)value;
            writer.WriteValue( v.name );
        }
    }
}