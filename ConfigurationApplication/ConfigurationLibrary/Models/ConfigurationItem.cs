/*using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConfigurationLibrary.Models
{
    public class ConfigurationItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }   // Mongo'nun otomatik oluşturduğu id

        [BsonElement("Id")]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; } = string.Empty;
    }
}*/

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding; // [BindNever] için gerekli

namespace ConfigurationLibrary.Models
{
    public class ConfigurationItem
    {
        
         [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]      
        [BindNever]       
        public string? _id { get; set; }  // <-- KRİTİK DÜZELTME: string yerine string? 
                                          // Artık bu alan POST için zorunlu değil.
        
        [BsonElement("Id")]
        public int Id { get; set; } 

        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; } = string.Empty;
    }
}
