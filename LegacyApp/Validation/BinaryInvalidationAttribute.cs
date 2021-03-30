using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp.Validation
{
    public class BinaryInvalidationAttribute : InvalidationAttribute
    {
        public ExpressionType ExpressionType { get; set; }

        protected virtual object GetConstantParameterObject()
        {
            return Parameter;
        }

        public BinaryInvalidationAttribute(object parameter, ExpressionType expressionType) : base(parameter)
        {
            ExpressionType = expressionType;
        }

        protected virtual Expression GetBinaryExpressionRight(PropertyInfo property)
        {
            var parameter = GetConstantParameterObject();
            Expression constant = Expression.Constant(parameter);
            if (parameter.GetType() == property.PropertyType)
            {
                return constant;
            }
            return Expression.Convert(constant, property.PropertyType);
        }
        
        protected virtual Expression GetBinaryExpressionLeft(ParameterExpression parameter, PropertyInfo propertyInfo)
        {
            return Expression.Convert(parameter, propertyInfo.PropertyType);
        }

        protected override Func<object, bool> GetDelegate(PropertyInfo property)
        {
            var parameter = Expression.Parameter(typeof(object));
            var expression = Expression.MakeBinary(ExpressionType, GetBinaryExpressionLeft(parameter, property), GetBinaryExpressionRight(property));
            return Expression.Lambda<Func<object, bool>>(expression, parameter).Compile();
        }
    }
}
