using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using DocoptNet;

namespace TagCloudGenerator
{
    static class OptionsFiller
    {
        // Docopt wrapper to populate static-typed options object

        static readonly Dictionary<Type, Func<ValueObject, object>> Converters =
            new Dictionary<Type, Func<ValueObject, object>>
        {
            { typeof(string), value => value.ToString() },
            { typeof(int), value => value.AsInt },
            { typeof(Color), value => ColorTranslator.FromHtml(value.ToString()) }
        };

        static string PropertyNameToArgument(string propertyName)
        {
            return new Regex("(?<=.)([A-Z])").Replace(propertyName, "-$1").ToLower();
        }

        public static TOptions Fill<TOptions>(string usage, string[] args)
        {
            var optionsObject = (TOptions) Activator.CreateInstance(typeof (TOptions));
            var optionsDict = new Docopt().Apply(usage, args, exit: true);

            var properties = optionsObject.GetType().GetProperties(
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            foreach (var propertyInfo in properties)
            {
                var name = propertyInfo.Name;
                var arg = PropertyNameToArgument(name);
                var type = propertyInfo.PropertyType;
                if (!Converters.ContainsKey(type))
                    throw new ArgumentException($"Can't populate property {name} with type {type.Name}");

                ValueObject value;
                if (!optionsDict.TryGetValue("--" + arg, out value) &&
                    !optionsDict.TryGetValue("<" + arg + ">", out value))
                    throw new ArgumentException($"Can't find arguments to populate property {name}");

                if (value == null)
                    continue;
                propertyInfo.SetValue(optionsObject, Converters[type](value));
            }
            return optionsObject;
        }
    }
}
