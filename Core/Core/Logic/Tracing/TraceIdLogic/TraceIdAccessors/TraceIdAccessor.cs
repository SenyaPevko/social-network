using Core.Logic.Tracing.TraceLogic.TraceReaders;
using Core.Logic.Tracing.TraceLogic.TraceWriters;
using Serilog.Context;

namespace Core.Logic.Tracing.TraceIdLogic.TraceIdAccessors;

internal class TraceIdAccessor : ITraceWriter, ITraceReader, ITraceIdAccessor
{
    private string value;

    public string GetValue()
    {
        return value;
    }

    public string Name => "TraceId";

    public void WriteValue(string value)
    {
        this.value = string.IsNullOrWhiteSpace(value) ? value = Guid.NewGuid().ToString() : value;
        LogContext.PushProperty(Name, value);
    }
}