using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace romaps_be.Models
{
    public class Camera
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string SnapshotUrl { get; set; } = null!;
    }
}
