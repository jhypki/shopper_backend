using System;

namespace ShopperBackend.Exceptions
{
    public class CustomNotFoundException : Exception
    {
        public CustomNotFoundException(string message) : base(message) { }
    }

    public class CustomBadRequestException : Exception
    {
        public CustomBadRequestException(string message) : base(message) { }
    }

    public class CustomUnauthorizedException : Exception
    {
        public CustomUnauthorizedException(string message) : base(message) { }
    }

    public class CustomForbiddenException : Exception
    {
        public CustomForbiddenException(string message) : base(message) { }
    }

    public class CustomConflictException : Exception
    {
        public CustomConflictException(string message) : base(message) { }
    }

    public class CustomInternalServerErrorException : Exception
    {
        public CustomInternalServerErrorException(string message) : base(message) { }
    }

    // Add more custom exceptions as needed
}
