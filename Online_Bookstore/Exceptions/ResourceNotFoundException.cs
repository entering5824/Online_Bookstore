using System;

namespace Online_Bookstore.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException()
            : base("Tài nguyên không được tìm thấy.") { }

        public ResourceNotFoundException(string message)
            : base(message) { }

        public ResourceNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
