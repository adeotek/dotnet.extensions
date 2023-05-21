using System.Diagnostics.Contracts;

namespace Adeotek.Extensions.Railways;

public struct Result<TSuccessValue, TFailureValue>
    where TFailureValue : notnull
{
    public readonly TSuccessValue Value;
    public readonly TFailureValue? Error; 
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
    public Result(TFailureValue? error)
    {
        Value = default!;
        Error = error;
        IsSuccess = true;
        IsFailure = false;
    }
    
    [Pure]
    public TSuccessValue IfFail(TSuccessValue defaultValue) => 
        IsSuccess ? 
            Value : 
            defaultValue;

    [Pure]
    public TSuccessValue  IfFail(Func<TFailureValue?, TSuccessValue> predicate) => 
        IsSuccess ? 
            Value : 
            predicate(Error);

    [Pure]
    public Result<TSuccessValue, TFailureValue> EnsureIs(
        Func<TSuccessValue, bool> predicate,
        TFailureValue? error) =>
        IsFailure || predicate(Value) ?
            this :
            new Result<TSuccessValue, TFailureValue>(error);
    
    [Pure]
    public async Task<Result<TSuccessValue, TFailureValue>> EnsureIsAsync(
        Func<TSuccessValue, Task<bool>> predicate,
        TFailureValue? error) =>
        IsFailure || await predicate(Value) ?
            this :
            new Result<TSuccessValue, TFailureValue>(error);

    [Pure]
    public TResult Match<TResult>(Func<TSuccessValue, TResult> successPredicate, Func<TFailureValue?, TResult> failurePredicate) => 
        IsSuccess ? 
            successPredicate(Value) :
            failurePredicate(Error);

    [Pure]
    public Result<TResult, TFailureValue> Map<TResult>(
        Func<TSuccessValue, TResult> predicate) =>
        IsSuccess 
            ? new Result<TResult, TFailureValue>(predicate(Value))        
            : new Result<TResult, TFailureValue>(Error);

    [Pure]
    public async Task<Result<TResult, TFailureValue>> MapAsync<TResult>(
        Func<TSuccessValue, Task<TResult>> predicate) =>
        IsSuccess
            ? new Result<TResult, TFailureValue>(await predicate(Value))
            : new Result<TResult, TFailureValue>(Error);
    
    [Pure]
    public Result<TSuccessResult, TFailureResult> Map<TSuccessResult, TFailureResult>(
        Func<TSuccessValue, TSuccessResult> successPredicate, 
        Func<TFailureValue?, TFailureResult> failurePredicate) 
        where TFailureResult : notnull =>
        IsSuccess 
            ? new Result<TSuccessResult, TFailureResult>(successPredicate(Value))        
            : new Result<TSuccessResult, TFailureResult>(failurePredicate(Error));
    
    [Pure]
    public async Task<Result<TSuccessResult, TFailureResult>> MapAsync<TSuccessResult, TFailureResult>(
        Func<TSuccessValue, Task<TSuccessResult>> successPredicate, 
        Func<TFailureValue?, Task<TFailureResult>> failurePredicate) 
        where TFailureResult : notnull =>
        IsSuccess 
            ? new Result<TSuccessResult, TFailureResult>(await successPredicate(Value))        
            : new Result<TSuccessResult, TFailureResult>(await failurePredicate(Error));

    [Pure]
    public override string ToString() => 
        IsSuccess ? 
            Value?.ToString() ?? "(null value)" :
            Error?.ToString() ?? "(null error)";
    
    [Pure]
    public override int GetHashCode() => 
        IsSuccess ? 
            Value?.GetHashCode() ?? 0 : 
            -1;
    
    [Pure]
    public bool Equals(Result<TSuccessValue, TFailureValue> other) => 
        ResultEquals(this, other);
        
    [Pure]
    public int CompareTo(Result<TSuccessValue, TFailureValue> other) => 
        ResultCompare(this, other);
    
    [Pure]
    public override bool Equals(object? obj) => 
        obj is Result<TSuccessValue, TFailureValue> other && Equals(other);

    [Pure]
    public static bool operator ==(Result<TSuccessValue, TFailureValue> x, 
        Result<TSuccessValue, TFailureValue> y) => 
        ResultEquals(x, y);

    [Pure]
    public static bool operator !=(Result<TSuccessValue, TFailureValue> x, 
        Result<TSuccessValue, TFailureValue> y) => 
        !(x==y);
    
    [Pure]
    public static bool operator <(Result<TSuccessValue, TFailureValue> x, 
        Result<TSuccessValue, TFailureValue> y) =>
        x.CompareTo(y) < 0;

    [Pure]
    public static bool operator <=(Result<TSuccessValue, TFailureValue> x, 
        Result<TSuccessValue, TFailureValue> y) =>
        x.CompareTo(y) <= 0;

    [Pure]
    public static bool operator >(Result<TSuccessValue, TFailureValue> x, 
        Result<TSuccessValue, TFailureValue> y) =>
        x.CompareTo(y) > 0;

    [Pure]
    public static bool operator >=(Result<TSuccessValue, TFailureValue> x, 
        Result<TSuccessValue, TFailureValue> y) =>
        x.CompareTo(y) >= 0;
    
    private static bool ResultEquals(
        Result<TSuccessValue, TFailureValue> x, 
        Result<TSuccessValue, TFailureValue> y) =>
        x.IsSuccess && y.IsSuccess ?
            EqualityComparer<TSuccessValue>.Default.Equals(x.Value, y.Value) :
            x.IsFailure && y.IsFailure && 
            EqualityComparer<TFailureValue>.Default.Equals(x.Error, y.Error);

    private static int ResultCompare(
        Result<TSuccessValue, TFailureValue> x,
        Result<TSuccessValue, TFailureValue> y) =>
        x.IsSuccess switch
        {
            false when !y.IsSuccess => 0,
            false when y.IsSuccess => -1,
            true when !y.IsSuccess => 1,
            _ => Comparer<TSuccessValue>.Default.Compare(x.Value, y.Value)
        };
}
