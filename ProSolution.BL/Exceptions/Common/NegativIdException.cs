using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Exceptions.Common
{
    public class NegativIdException : Exception,IBaseException
    {
        public int StatusCode => StatusCodes.Status404NotFound;

        public string ErrorMessage { get;}
        public NegativIdException() : base() { ErrorMessage = "id uygun deyil"; }
        public NegativIdException(string? message) : base(message)
        {
            ErrorMessage = message;
        }

    }
}
