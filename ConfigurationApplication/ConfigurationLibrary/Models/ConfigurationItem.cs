using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ConfigurationLibrary.Models
{
    public class ConfigurationItem
    {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]      
        [BindNever]       
        public string? _id { get; set; }  
        
        [BsonElement("Id")]
        public int Id { get; set; } 

        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; } = string.Empty;
    }
}
