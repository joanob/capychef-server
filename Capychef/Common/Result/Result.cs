using YourOwnBoss.Common.Errors;

namespace YourOwnBoss.Common.Result;

public class Result<T>
{
    private readonly AppError _error;
    private readonly T _result;

    public Result(T result)
    {
        _result = result;
    }

    public Result(AppError error)
    {
        _error = error;
    }

    public Result(T result, AppError error)
    {
        _result = result;
        _error = error;
    }

    public bool failed()
    {
        return _error != null;
    }

    public T get()
    {
        return _result;
    }

    public AppError error()
    {
        return _error;
    }
}