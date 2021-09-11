using FluentAssertions;
using Scapho.Application.DataParsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Scapho.Api.Tests
{
    public class DataParsingTests
    {
        public class ResultRecord
        {
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
        }

        [Fact]
        public void HeaderMapper_DataRecordParser_Integration()
        {
            string csv =
                "description,amount,date\n" +
                "Pinnen,-50.00,2021-09-10:212200";

            var rawData = CsvReader.GetRawData(csv);
            var headers = rawData[0];
            var headerMapping = new HeaderMapper(GetFormat, GetTypeForHeader, GetInputForHeader).MapHeaders(headers);
            var records = new DataRecordParser().GetRecords(rawData.Skip(1), headerMapping);

            records.Should().HaveCount(1);
            var record = records.First();
            record.Where(kvp => kvp.Key.Key == "description").Should().HaveCount(1);
            record.First(kcp => kcp.Key.Key == "description").Value.Should().Be("Pinnen");

            record.Where(kvp => kvp.Key.Key == "amount").Should().HaveCount(1);
            record.First(kvp => kvp.Key.Key == "amount").Value.Should().BeEquivalentTo(-50m);

            record.Where(kvp => kvp.Key.Key == "date").Should().HaveCount(1);
            record.First(kvp => kvp.Key.Key == "date").Value.Should().BeEquivalentTo(new DateTime(2021, 9, 10, 21, 22, 0));
        }

        [Fact]
        public void DynamicStringDataParserTest()
        {
            string csv =
                "description,amount,date\n" +
                "Pinnen,-50.00,2021-09-10:212200";
            var rawData = CsvReader.GetRawData(csv);

            var records = new DynamicStringDataParser<ResultRecord>(new HeaderMapper(GetFormat, GetTypeForHeader, GetPropertyForHeader)).Parse(rawData);

            records.Should().HaveCount(1);
            var firstRecord = records.First();
            firstRecord.Description.Should().Be("Pinnen");
            firstRecord.Amount.Should().Be(-50m);
            firstRecord.Date.Should().Be(new DateTime(2021, 9, 10, 21, 22, 0));
        }

        private string GetPropertyForHeader(string header)
        {
            switch (header)
            {
                case "description":
                    return "Description";
                case "amount":
                    return "Amount";
                case "date":
                    return "Date";
                default:
                    throw new InvalidOperationException("Not implemented header string");
            }
        }

        [Fact]
        public void CsvReader_Should_Get_All_Records()
        {
            string csv =
                "one,two,three\n" +
                "1,2,3";

            var parsed = CsvReader.GetRawData(csv);

            parsed.Length.Should().Be(2);
        }

        [Fact]
        public void CsvReader_Should_Get_All_Fields()
        {
            string csv =
                "one,two,three\n" +
                "1,2,3";

            var parsed = CsvReader.GetRawData(csv);

            parsed.First().Length.Should().Be(parsed.Last().Length);
            parsed.Last().Length.Should().Be(3);
            parsed.Last()[0].Should().Be("1");
            parsed.Last()[1].Should().Be("2");
            parsed.Last()[2].Should().Be("3");
        }

        public static class CsvReader
        {
            public static string[] GetLines(string csv)
            {
                return csv.Split('\n');
            }

            public static string[][] GetRawData(string csv, string valueSeparator = ",")
            {
                var lines = GetLines(csv);
                return EnumerateRawData(lines, valueSeparator).ToArray();
            }

            private static IEnumerable<string[]> EnumerateRawData(string[] lines, string separator)
            {
                foreach(var line in lines)
                {
                    yield return line.Split(separator);
                }
            }
        }        

        [Fact]
        public void Lines_Are_Correctly_Split()
        {
            string csv =
                "description,amount,date\n" +
                "Pinnen,-50.00,2021-09-10:212200";

            var lines = CsvReader.GetLines(csv);
            lines.Should().HaveCount(2);
            lines[0].Should().Contain("amount");
        }

        [Fact]
        public void DateFormat_IsCorrect()
        {
            var dateStr = "2021-09-10:212200";
            var dateTime = new DateTime(2021, 9, 10, 21, 22, 0);

            var format = GetFormat(typeof(DateTime));
            var resultStr = dateTime.ToString(format);
            resultStr.Should().Be(dateStr);

            var resultDt = DateTime.ParseExact(dateStr, format, CultureInfo.InvariantCulture);
            resultDt.Should().Be(dateTime);
        }

        private string? GetFormat(Type type)
        {
            if (type == typeof(DateTime))
            {
                return "yyyy'-'MM'-'dd':'HHmmss";
            }
            if (type == typeof(decimal))
            {
                return ".";
            }
            return null;
        }

        private Type GetTypeForHeader(string header)
        {
            switch (header)
            {
                case "description":
                    return typeof(string);
                case "amount":
                    return typeof(decimal);
                case "date":
                    return typeof(DateTime);
                default:
                    return typeof(string);
            }
        }

        private string GetInputForHeader(string header)
        {
            return header;
        }
    }
}
