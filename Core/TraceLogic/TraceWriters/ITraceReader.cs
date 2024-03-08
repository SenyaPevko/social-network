namespace Core.TraceLogic.TraceWriters
{
    /// <summary>
    /// Recording trace values when sending a request
    /// </summary>
    public interface ITraceReader
    {
        string Name { get; }

        string GetValue();
    }
}
