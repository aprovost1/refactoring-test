using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class InvalidationAttribute : Attribute
    {
        public object Parameter { get; set; }

        public bool Negative { get; set; }

        public string Dependance { get; set; }

        public InvalidationAttribute(object parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentOutOfRangeException("The parameter for validation must be non-null");
            }
            Parameter = parameter;
        }

        string memberName;
        protected string MemberName
        {
            get
            {
                if (memberName == null)
                {
                    memberName = Parameter.ToString();
                }
                return memberName;
            }
            set
            {
                memberName = value;
            }
        }

        protected virtual object GetMethodInstance(object value)
        {
            return null;
        }

        protected virtual object GetMethodParameter(object value)
        {
            return value;
        }

        protected virtual Func<object, bool> GetDelegate(PropertyInfo property)
        {
            Type proprertyType = property.PropertyType;
            MethodInfo method = proprertyType.GetMethod(MemberName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public, null, new[] { proprertyType }, null);
            return value =>
            {
                if (method == null)
                {
                    return false;
                }
                object instance = GetMethodInstance(value);
                object parameter = GetMethodParameter(value);
                return Convert.ToBoolean(method.Invoke(instance, new[] { parameter }));
            };
        }

        public bool Invalidate(PropertyInfo property, object instance, bool verifyDependance = true)
        {
            object value = property.GetValue(instance);
            bool invalid = GetDelegate(property)(value) != Negative;
            if (Dependance != null && verifyDependance)
            {
                bool dependanceInvalid = !ValidationLogic.Validate(property.DeclaringType, Dependance, instance, false);
                return invalid && dependanceInvalid;
            }
            return invalid;
        }
    }
}
