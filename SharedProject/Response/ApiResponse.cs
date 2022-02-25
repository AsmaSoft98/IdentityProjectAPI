namespace SharedProject.Response
{
    public class ApiResponse<T> : ApiBaseResponse
    {
        public ApiResponse()
        {
            IsSuccess = true;
        }
        public ApiResponse(T value)
        {
            IsSuccess = true;
            Value = value;
        }
        public ApiResponse(T value, string message)
        {
            IsSuccess = true;
            Value = value;
            Message = message;
        }
        public T Value { get; set; }
    }
    public class ApiResponse : ApiBaseResponse
    {
        public ApiResponse(string message)
        {
            IsSuccess = true;
            Message = message;
        }
        public ApiResponse()
        {
            IsSuccess = true;
        }
    }
}
