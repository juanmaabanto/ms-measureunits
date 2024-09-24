using System;
using System.Runtime.Serialization;
using Sofisoft.Abstractions.Exceptions;

namespace Sofisoft.Erp.MeasureUnits.Api.Infrastructure.Exceptions
{
    [Serializable]
    public sealed class DomainException : BadRequestException
    {
        public DomainException(string message)
            : base(message)
        { }

        private DomainException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}