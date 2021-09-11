using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scapho.Application.DataParsing
{
    public class DataRecordConverter<TRecord>
            where TRecord : new()
    {
        public TRecord ConvertRecord(Dictionary<HeaderMapping, object> dataRecord)
        {
            var result = new TRecord();
            foreach (var property in typeof(TRecord).GetProperties())
            {
                var value = dataRecord.FirstOrDefault(kvp => kvp.Key.Key == property.Name).Value;
                if (value != null)
                {
                    property.SetValue(result, value);
                }
            }
            return result;
        }
    }
}
