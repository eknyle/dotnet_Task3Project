using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Project.Classes
{
    public class ErrorObject
    {
        public string Message { get; set; }
        public string Parameter { get; set; }

        public ErrorObject(string message, string parameter)
        {
            Message = message;
            Parameter = parameter;
        }
    }
}
