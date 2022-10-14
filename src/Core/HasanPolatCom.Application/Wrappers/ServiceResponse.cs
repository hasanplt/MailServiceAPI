namespace HasanPolatCom.Application.Wrappers
{

    public class ServiceResponse<T> : BaseResponse
    {
        public T Value { get; set; }

        public ServiceResponse(T value)
        {
            Value = value;
        }
        public ServiceResponse(T value, bool isSuccess, string? message)
        {
            Value = value;
            IsSuccess = isSuccess;
            Message = message;
        }
        public ServiceResponse()
        {

        }
    }
}