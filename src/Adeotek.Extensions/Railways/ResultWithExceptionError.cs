using System.Diagnostics.Contracts;

namespace Adeotek.Extensions.Railways;

public class Result<TSuccessValue>
{
    public readonly TSuccessValue Value;
    public readonly Exception? Error;
    public readonly bool IsSuccess;
    public readonly bool IsFailure;

    /// <summary>
    /// Constructor of success
    /// </summary>
    /// <param name="value"></param>
    public Result(TSuccessValue value)
    {
        Value = value;
        Error = default;
        IsSuccess = true;
        IsFailure = false;
    }

    /// <summary>
    /// Constructor of error
    /// </summary>
    /// <param name="error"></param>
    public Result(Exception? error)
    {
        Value = default!;
        Error = error;
        IsSuccess = true;
        IsFailure = false;
    }

    [Pure]
    public TSuccessValue IfFail(TSuccessValue defaultValue) =>
        IsSuccess ? Value : defaultValue;

    [Pure]
    public TSuccessValue IfFail(Func<Exception?, TSuccessValue> predicate) =>
        IsSuccess ? Value : predicate(Error);

    [Pure]
    public Result<TSuccessValue> EnsureIs(
        Func<TSuccessValue, bool> predicate,
        Exception? error) =>
        IsFailure || predicate(Value) ? this : new Result<TSuccessValue>(error);

    [Pure]
    public async Task<Result<TSuccessValue>> EnsureIsAsync(
        Func<TSuccessValue, Task<bool>> predicate,
        Exception? error) =>
        IsFailure || await predicate(Value) ? this : new Result<TSuccessValue>(error);

    [Pure]
    public TResult Match<TResult>(Func<TSuccessValue, TResult> successPredicate,
        Func<Exception?, TResult> failurePredicate) =>
        IsSuccess ? successPredicate(Value) : failurePredicate(Error);

    [Pure]
    public Result<TResult, Exception> Map<TResult>(
        Func<TSuccessValue, TResult> predicate) =>
        IsSuccess
            ? new Result<TResult, Exception>(predicate(Value))
            : new Result<TResult, Exception>(Error);

    [Pure]
    public async Task<Result<TResult, Exception>> MapAsync<TResult>(
        Func<TSuccessValue, Task<TResult>> predicate) =>
        IsSuccess
            ? new Result<TResult, Exception>(await predicate(Value))
            : new Result<TResult, Exception>(Error);

    [Pure]
    public Result<TSuccessResult, TFailureResult> Map<TSuccessResult, TFailureResult>(
        Func<TSuccessValue, TSuccessResult> successPredicate,
        Func<Exception?, TFailureResult> failurePredicate)
        where TFailureResult : notnull =>
        IsSuccess
            ? new Result<TSuccessResult, TFailureResult>(successPredicate(Value))
            : new Result<TSuccessResult, TFailureResult>(failurePredicate(Error));

    [Pure]
    public async Task<Result<TSuccessResult, TFailureResult>> MapAsync<TSuccessResult, TFailureResult>(
        Func<TSuccessValue, Task<TSuccessResult>> successPredicate,
        Func<Exception?, Task<TFailureResult>> failurePredicate)
        where TFailureResult : notnull =>
        IsSuccess
            ? new Result<TSuccessResult, TFailureResult>(await successPredicate(Value))
            : new Result<TSuccessResult, TFailureResult>(await failurePredicate(Error));

    [Pure]
    public override string ToString() =>
        IsSuccess ? Value?.ToString() ?? "(null value)" : Error?.ToString() ?? "(null error)";

    [Pure]
    public override int GetHashCode() =>
        IsSuccess ? Value?.GetHashCode() ?? 0 : -1;

    [Pure]
    public bool Equals(Result<TSuccessValue> other) =>
        ResultEquals(this, other);

    [Pure]
    public int CompareTo(Result<TSuccessValue> other) =>
        ResultCompare(this, other);

    [Pure]
    public override bool Equals(object? obj) =>
        obj is Result<TSuccessValue> other && Equals(other);

    [Pure]
    public static bool operator ==(Result<TSuccessValue> x,
        Result<TSuccessValue> y) =>
        ResultEquals(x, y);

    [Pure]
    public static bool operator !=(Result<TSuccessValue> x,
        Result<TSuccessValue> y) =>
        !(x == y);

    [Pure]
    public static bool operator <(Result<TSuccessValue> x,
        Result<TSuccessValue> y) =>
        x.CompareTo(y) < 0;

    [Pure]
    public static bool operator <=(Result<TSuccessValue> x,
        Result<TSuccessValue> y) =>
        x.CompareTo(y) <= 0;

    [Pure]
    public static bool operator >(Result<TSuccessValue> x,
        Result<TSuccessValue> y) =>
        x.CompareTo(y) > 0;

    [Pure]
    public static bool operator >=(Result<TSuccessValue> x,
        Result<TSuccessValue> y) =>
        x.CompareTo(y) >= 0;

    private static bool ResultEquals(
        Result<TSuccessValue> x,
        Result<TSuccessValue> y) =>
        x.IsSuccess && y.IsSuccess
            ? EqualityComparer<TSuccessValue>.Default.Equals(x.Value, y.Value)
            : x.IsFailure && y.IsFailure &&
              x.Error?.GetType() == y.Error?.GetType() &&
              x.Error?.Message == y.Error?.Message;
    
    private static int ResultCompare(
        Result<TSuccessValue> x, 
        Result<TSuccessValue> y) =>
        x.IsSuccess switch
        {
            false when !y.IsSuccess => 0,
            false when y.IsSuccess => -1,
            true when !y.IsSuccess => 1,
            _ => Comparer<TSuccessValue>.Default.Compare(x.Value, y.Value)
        };
}