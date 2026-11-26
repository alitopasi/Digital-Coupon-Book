namespace CouponBook.Business.Auth;

public class ServiceResult<T>
{
    public bool Succeeded { get; private set; }
    public string? ErrorCode { get; private set; }
    public List<string> Errors { get; private set; } = new();
    public T? Value { get; private set; }

    public static ServiceResult<T> Ok(T value) =>
        new() { Succeeded = true, Value = value };

    public static ServiceResult<T> Fail(string code, IEnumerable<string>? errors = null) =>
        new() { ErrorCode = code, Errors = errors?.ToList() ?? new List<string>() };
}