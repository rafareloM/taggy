using System;

namespace taggyManagement.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }

        protected Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Ok() => new Result(true, null);
        public static Result Fail(string error) => new Result(false, error);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(T value) : base(true, null)
        {
            Value = value;
        }

        private Result(string error) : base(false, error) { Value = default; }

        public static Result<T> Ok(T value) => new Result<T>(value);
        public static new Result<T> Fail(string error) => new Result<T>(error);
    }
}
