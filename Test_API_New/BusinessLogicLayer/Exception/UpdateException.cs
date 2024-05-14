using Microsoft.AspNetCore.Http;

namespace Test_API_New.BusinessLogicLayer.Exception
{
    public abstract class UpdateException : System.Exception
    {
        public UpdateException(string message) : base(message) { }
    }

    public class UpdateNotFoundException : UpdateException
    {
        public UpdateNotFoundException(string message) : base(message) { }
    }
}
