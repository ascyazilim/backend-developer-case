using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common.Results
{
    public class Result<T> : Result
    {
        public T? Data { get; private set; } 

        public static Result<T> Ok(T data, string message = "Success")
            => new()
            {
                Success = true,
                Message = message,
                Data = data
            };

        public new static Result<T> Fail(string message, params string[] errors)
            => new()
            {
                Success = false,
                Message = message,
                Errors = errors.ToList()
            };
    }
}
