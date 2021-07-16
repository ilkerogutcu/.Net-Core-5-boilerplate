#nullable enable
namespace Core.CrossCuttingConcerns.Logging
{
    /// <summary>
    /// Log parameter
    /// </summary>
    public class LogParameter
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public string Type { get; set; }

        public object? ReturnValue { get; set; }
    }
}