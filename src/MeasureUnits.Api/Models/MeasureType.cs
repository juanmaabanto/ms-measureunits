using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sofisoft.MongoDb.Attributes;
using Sofisoft.MongoDb.Domain;

namespace Sofisoft.Erp.MeasureUnits.Api.Models
{
    [BsonCollection("measureType")]
    public class MeasureType : Document
    {

        [BsonElement("name")]
        public string Name { get; set; }
        
    }
}