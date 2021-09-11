using FluentAssertions;
using Scapho.Application.DataParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Scapho.Application.Tests.DataParsing
{
    public class DataRecordConverterTests
    {
        [Fact]
        public void CanConvertToRecord()
        {
            var dataRecord = new Dictionary<HeaderMapping, object>();
            dataRecord.Add(new HeaderMapping("Description", 0), "Pinnen");
            dataRecord.Add(new DecimalMapping("Amount", 1, ","), -50m);
            dataRecord.Add(new DateTimeMapping("Date", 2, "something"), new DateTime(2021, 9, 10, 21, 22, 0));

            var result = new DataRecordConverter<ResultRecord>().ConvertRecord(dataRecord);
            result.Description.Should().Be("Pinnen");
            result.Amount.Should().Be(-50m);
            result.Date.Should().Be(new DateTime(2021, 9, 10, 21, 22, 0));
        }

        public class ResultRecord
        {
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
