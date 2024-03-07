using Core.TraceLogic.TraceWriters;
using Serilog.Context;

namespace Core.TraceIdLogic.TraceIdAccessors
{
    internal class TraceIdAccessor : ITraceReader, ITraceWriter, ITraceIdAccessor
    {
        public string Name => "TraceId";

        private string value;

        public string GetValue()
        {
            return value;
        }

        public void WriteValue(string value)
        {
            // на случай если это первый в цепочке сервис и до этого не было traceId
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Guid.NewGuid().ToString();
            }

            this.value = value;
            LogContext.PushProperty("TraceId", value);
        }
    }
}
