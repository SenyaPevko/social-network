namespace Core.TraceLogic.TraceWriters
{
    /// <summary>
    /// Recording trace values when sending a request
    /// </summary>
    public interface ITraceWriter
    {
        string Name { get; }

        string GetValue();
    }
}
