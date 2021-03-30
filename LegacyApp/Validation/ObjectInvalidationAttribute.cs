using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp.Validation
{
    public class ObjectInvalidationAttribute : InvalidationAttribute
    {
        public object MethodParameter
        {
            get;
            set;
        }

        public ObjectInvalidationAttribute(string methodName, object methodParameter) : base(methodName)
        {
            MethodParameter = methodParameter;
        }

        protected override object GetMethodInstance(object value)
        {
            return value;
        }

        protected override object GetMethodParameter(object value)
        {
            return MethodParameter;
        }
    }
}
