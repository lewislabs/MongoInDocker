using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Client
{
    [BsonIgnoreExtraElements]
    public class Person
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("age")]
        public int Age { get; set; }
    }
}