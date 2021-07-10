namespace Core.CrossCuttingConcerns.Logging
{
    /// <summary>
    /// Log Detail With Exception
    /// </summary>
    public class LogDetailWithException : LogDetail
    {
        public string ExceptionMessage { get; set; }
    }
}