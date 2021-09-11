using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scapho.Application.DataParsing
{
    public class DynamicStringDataParser<TRecord>
            where TRecord : new()
    {
        private readonly DataRecordParser _recordParser;
        private readonly DataRecordConverter<TRecord> _recordConverter;
        private readonly HeaderMapper _headerMapper;
        public DynamicStringDataParser(HeaderMapper headerMapper, DataRecordParser recordParser = null, DataRecordConverter<TRecord> recordConverter = null)
        {
            _recordParser = recordParser ?? new();
            _recordConverter = recordConverter ?? new();
            _headerMapper = headerMapper ?? throw new ArgumentNullException(nameof(headerMapper));
        }

        public IEnumerable<TRecord> Parse(string[][] rawDataWithHeaderRow)
        {
            var headers = rawDataWithHeaderRow[0];
            var headerMapping = _headerMapper.MapHeaders(headers);
            var records = _recordParser.GetRecords(rawDataWithHeaderRow.Skip(1), headerMapping);
            foreach (var record in records)
            {
                yield return _recordConverter.ConvertRecord(record);
            }
        }
    }
}
