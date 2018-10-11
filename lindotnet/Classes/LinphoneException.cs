using System;

namespace lindotnet.Classes
{
    public class LinphoneException : Exception
    {
        public LinphoneException(string message) : base(message)
        {
        }

        public LinphoneException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
