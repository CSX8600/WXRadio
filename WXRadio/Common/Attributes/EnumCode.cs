using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WXRadio.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class EnumCode : Attribute
    {
        private string _code;
        private string _name;

        public EnumCode(string Code, string Name)
        {
            _code = Code;
            _name = Name;
        }

        public string Code
        {
            get { return _code; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
