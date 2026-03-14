using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common.Results
{
    public class Result 
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; } = string.Empty;
        public List<string> Errors { get; protected set; } = new();

        public static Result Ok(string message = "Success")
            => new() { Success = true, Message = message };

        public static Result Fail(string message, params string[] errors)
            => new()
            {
                Success = false,
                Message = message,
                Errors = errors.ToList()
            };
    }
}
