namespace FonTech.Domain.Result;

public class BaseResult
{
    public bool IsSuccess=> ErrorMessage==null;

    public string ErrorMessage { get; set; }

    public int ErrorCode { get; set; }
}

public class BaseResult<T> : BaseResult
{
    public BaseResult (string errorMessege, int errorCode, T data)
    {
        ErrorMessage = errorMessege;
        ErrorCode = errorCode;
        Data = data;
    }

    public BaseResult() { }
    public T Data { get; set; }
}
