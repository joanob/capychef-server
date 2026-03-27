using Capychef.Common.Errors;

namespace Capychef.Common.Result;

public class Result<T>
{
    private readonly AppError? _error;
    private readonly T? _result;

    public Result(T result)
    {
        _result = result;
    }

    public Result(AppError error)
    {
        _error = error;
    }

    public bool Failed()
    {
        return _error != null;
    }

    public T Get()
    {
        return _result ?? throw new Exception("Result object is null");
    }

    public AppError Error()
    {
        return _error ?? throw new Exception("Result error is null");
    }
}