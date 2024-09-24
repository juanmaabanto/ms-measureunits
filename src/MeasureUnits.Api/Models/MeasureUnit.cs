using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sofisoft.MongoDb.Attributes;
using Sofisoft.MongoDb.Domain;

namespace Sofisoft.Erp.MeasureUnits.Api.Models
{
    [BsonCollection("measureUnit")]
    public class MeasureUnit : Document
    {
        /// <summary>
        /// Get or set the company Id.
        /// </summary>
        [BsonElement("companyId")]
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or set the Id of the measure type.
        /// </summary>
        [BsonElement("measureTypeId")]
        public string MeasureTypeId { get; set; }

        /// <summary>
        /// Get or set the name of measure unit.
        /// </summary>
        [BsonElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Get or set the symbol of measure unit.
        /// </summary>
        [BsonElement("symbol")]
        public string Symbol { get; set; }

        /// <summary>
        /// True if the unit of measurement is the reference of the type of measurement,
        /// otherwise false.
        /// </summary>
        [BsonElement("reference")]
        public bool Reference { get; set; }

        /// <summary>
        /// Get or set the key figure for the conversion of units of measure.
        /// </summary>
        [BsonElement("ratio")]
        public decimal Ratio { get; set; }

        /// <summary>
        /// Get or set the unit of measure code.
        /// </summary>
        [BsonElement("code")]
        public string Code { get; set; }

        /// <summary>
        /// True if the unit of measurement is active, otherwise false.
        /// </summary>
        [BsonElement("active")]
        public bool active { get; set; }

        /// <summary>
        /// True if the unit of measurement is canceled, otherwise false.
        /// </summary>
        [BsonElement("canceled")]
        public bool Canceled { get; set; }

    }
}