using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp.Validation
{
    public class ModelValidation<M> where M : new()
    {
        M Object { get; set; }

        bool Valid { get; set; }
        public ModelValidation(M obj)
        {
            Object = obj;
            Validate();
        }

        void Validate()
        {
            Valid = true;
            foreach (var pair in GetAttributeDictionnary())
            {
                if (!pair.Value.Any())
                {
                    continue;
                }
                Valid = Valid ? pair.Value.Validate(pair.Key, Object) : false;
            }
        }

        public static implicit operator bool(ModelValidation<M> modelValidation)
        {
            return modelValidation.Valid;
        }

        static Dictionary<PropertyInfo, InvalidationAttribute[]> GetAttributeDictionnary()
        {
            return ValidationLogic.GetAttributeDictionnary(typeof(M));
        }
    }
}
