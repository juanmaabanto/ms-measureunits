using System;
using System.Runtime.Serialization;
using Sofisoft.Abstractions.Exceptions;

namespace Sofisoft.Erp.MeasureUnits.Api.Infrastructure.Exceptions
{
    [Serializable]
    public sealed class MeasureTypeNotFoundException : NotFoundException
    {
        public MeasureTypeNotFoundException(string measureTypeId)
            : base($"The measure type with the identifier {measureTypeId} was not found.")
        { }

        private MeasureTypeNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}