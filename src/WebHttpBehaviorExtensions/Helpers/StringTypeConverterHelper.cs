using System;
using System.ComponentModel;

namespace WebHttpBehaviorExtensions.Helpers
{
    internal static class StringTypeConverterHelper
    {
        public static bool TryConvertTo(this object value, Type type, out object result)
        {
            // keep the old value as default.
            result = value;

            var stringVal = Convert.ToString(value);

            if (string.IsNullOrWhiteSpace(stringVal))
            {
                // simply return. A default value is already provided to out parameter.
                return true;
            }
 
            try
            {
                var converter = TypeDescriptor.GetConverter(type);
                result = converter.ConvertFromString(stringVal);
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