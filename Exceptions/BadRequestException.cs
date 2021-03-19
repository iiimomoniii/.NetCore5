using System;
namespace Hero_Project.NetCore5.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message){
            
        }
    }
}