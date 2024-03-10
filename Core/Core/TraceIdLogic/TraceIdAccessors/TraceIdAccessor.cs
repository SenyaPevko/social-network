using Core.TraceLogic.TraceReaders;
using Core.TraceLogic.TraceWriters;
using Serilog.Context;

namespace Core.TraceIdLogic.TraceIdAccessors
{
    internal class TraceIdAccessor : ITraceWriter, ITraceReader, ITraceIdAccessor
    {
        public string Name => "TraceId";

        private string value;

        public string GetValue()
        {
            return value;
        }

        public void WriteValue(string value)
        {
            this.value = string.IsNullOrWhiteSpace(value) ? value = Guid.NewGuid().ToString() : value;
            LogContext.PushProperty("TraceId", value);
        }
    }
}

