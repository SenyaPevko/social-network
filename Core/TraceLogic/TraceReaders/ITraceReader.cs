namespace Core.TraceLogic.TraceReaders
{
    /// <summary>
    /// Reading trace values when a new scoped is created
    ///
    ///  // для HTTP мы создаем middleware и  в нем это делаем 
    /// </summary>
    public interface ITraceReader
    {
        string Name { get; }

        void WriteValue(string value);
    }
}
