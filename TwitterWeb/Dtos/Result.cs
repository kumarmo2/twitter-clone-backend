using System.Collections.Generic;

namespace TwitterWeb.Dtos
{
    public class Result<T>
    {
        public Result()
        {
            ErrorMessages = new List<string>();
        }
        public T SuccessResult { get; set; }
        public List<string> ErrorMessages { get; }

    }

    public class GenericResult<T, E>
    {
        public T SuccessResult { get; set; }
        public E Error { get; set; }
    }
}