using System;
namespace Hero_Project.NetCore5.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) {

        }
    }
}