namespace E_Commerce.Helpers
{
    public class ApiResponses<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }

        public ApiResponses(int statuscode, string message, T? data = default, string? error = null)
        {
            StatusCode = statuscode;
            Message = message;
            Data = data;
            Error = error;
        }

    }
}
