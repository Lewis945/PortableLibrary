using System;

namespace PortableLibrary.Core.Http
{
    public class HttpPostException : Exception
    {
        public HttpPostException(string message) : base(message)
        {
        }
    }
}