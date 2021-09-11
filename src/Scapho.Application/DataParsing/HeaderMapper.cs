using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scapho.Application.DataParsing
{
    public class HeaderMapper
    {
        private readonly Func<Type, string> _getFormatFunc;
        private readonly Func<string, Type> _getTypeForHeaderFunc;
        private readonly Func<string, string> _getPropertyNameForHeaderFunc;

        public HeaderMapper(Func<Type, string> getFormatFunc, Func<string, Type> getTypeForHeaderFunc, Func<string, string> getPropertyNameForHeaderFunc)
        {
            this._getFormatFunc = getFormatFunc;
            this._getTypeForHeaderFunc = getTypeForHeaderFunc;
            this._getPropertyNameForHeaderFunc = getPropertyNameForHeaderFunc;
        }
        public IEnumerable<HeaderMapping> MapHeaders(string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                var header = headers[i];
                var key = _getPropertyNameForHeaderFunc(header);
                var type = _getTypeForHeaderFunc(header);
                var format = _getFormatFunc(type);
                yield return GetMapping(key, type, i, format);
            }
        }

        private HeaderMapping GetMapping(string key, Type type, int position, string? format)
        {
            if (CanMap(type))
            {
                return Map(key, type, position, format);
            }
            throw new InvalidOperationException($"Could not map {type}");
        }

        protected virtual bool CanMap(Type type) => type == typeof(DateTime) || type == typeof(decimal) || type == typeof(string);
        protected virtual HeaderMapping Map(string key, Type type, int position, string? format)
        {
            if (type == typeof(DateTime))
            {
                if (string.IsNullOrEmpty(format))
                    throw new InvalidOperationException("Cannot get mapping for DateTime without format");
                return new DateTimeMapping(key, position, format);
            }
            if (type == typeof(decimal))
            {
                if (string.IsNullOrEmpty(format))
                    throw new InvalidOperationException("Cannot get mapping for decimal without decimal separator format");
                return new DecimalMapping(key, position, format);
            }
            if (type == typeof(string))
            {
                return new HeaderMapping(key, position);
            }
            throw new ArgumentException("Cannot map type", nameof(type));
        }
    }
}
