using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebHttpBehaviorExtensions.Helpers
{
    internal static class StringTypeConverterHelper
    {
        public static bool TryConvertTo(this object value, Type type, out object result)
        {
            // keep the old value as default.
            result = value;

            if (string.IsNullOrWhiteSpace(Convert.ToString(value)))
            {
                // simply return. A default value is already provided to out parameter.
                return true;
            }
 
            try
            {
                var converter = TypeDescriptor.GetConverter(type);
                result = converter.ConvertFromString(Convert.ToString(value));
                return true;
            }
            catch
            {
                // Log this exception if required. 
                return false;
            }
        }
    }
}