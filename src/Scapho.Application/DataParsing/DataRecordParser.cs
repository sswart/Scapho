using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scapho.Application.DataParsing
{
    public class DataRecordParser
    {
        public IEnumerable<Dictionary<HeaderMapping, object>> GetRecords(IEnumerable<string[]> data, IEnumerable<HeaderMapping> headerMapping)
        {
            foreach (var values in data)
            {
                var record = new Dictionary<HeaderMapping, object>();
                for (int i = 0; i < values.Length; i++)
                {
                    var header = headerMapping.First(hm => hm.Position == i);
                    record.Add(header, Parse(header, values[i]));
                }
                yield return record;
            }
        }

        protected virtual object Parse(HeaderMapping mapping, string value)
        {
            if (mapping is DateTimeMapping dtMap)
            {
                return DateTime.ParseExact(value, dtMap.Format, CultureInfo.InvariantCulture);
            }
            if (mapping is DecimalMapping decMap)
            {
                var formatInfo = new NumberFormatInfo();
                formatInfo.NumberDecimalSeparator = decMap.DecimalSeparator;
                formatInfo.CurrencyDecimalSeparator = decMap.DecimalSeparator;
                return decimal.Parse(value, NumberStyles.Currency, formatInfo);
            }
            return value;
        }
    }
}
