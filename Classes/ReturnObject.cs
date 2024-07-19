using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Project.Classes
{
    public class ReturnObject
    {
        public string? Message;
        public string? Value;

        public ReturnObject() { }
        public ReturnObject(string? message, string? value)
        {
            Message = message;
            Value = value;

        }

        public void SetAllParameters(string? message, string? value)
        {
            Message = message;
            Value = value;

        }
        public bool HasError() { return Message != null; }
        public bool HasValue() { return Value != null; }
    }
}
