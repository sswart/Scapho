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
    public class HeaderMapperTests
    {
        [Fact]
        public void HeaderMapper_Should_MakeMappingPerHeader()
        {
            var headers = new string[]
            {
                "description",
                "amount",
                "date"
            };
            var headerMapping = new HeaderMapper(GetFormat, GetTypeForHeader, GetInputForHeader).MapHeaders(headers);

            headerMapping.Should().HaveCount(3);
        }

        [Fact]
        public void HeaderMapper_Should_UseProvidedType()
        {
            var headers = new string[]
            {
                "description",
                "amount",
                "date"
            };
            var headerMapping = new HeaderMapper(GetFormat, GetTypeForHeader, GetInputForHeader).MapHeaders(headers);
            headerMapping.First(hm => hm.Key == "amount").Position.Should().Be(1);
            headerMapping.First(hm => hm.Key == "amount").Should().BeAssignableTo<DecimalMapping>();
        }

        [Fact]
        public void HeaderMapper_Should_ThrowException_On_UnknownType()
        {
            var headers = new string[]
            {
                "description",
                "amount",
                "date"
            };

            Type getTypeForHeader(string header) => typeof(double);
            var headerMapper = new HeaderMapper(GetFormat, getTypeForHeader, GetInputForHeader);
            Action act = () => headerMapper.MapHeaders(headers).ToArray();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void HeaderMapper_Should_ThrowException_On_MissingFormat()
        {
            var headers = new string[]
            {
                "description",
                "amount",
                "date"
            };
            string getFormat(Type type)
            {
                if (type == typeof(DateTime))
                {
                    return null;
                }
                else
                {
                    return GetFormat(type);
                }
            }
            var headerMapper = new HeaderMapper(getFormat, GetTypeForHeader, GetInputForHeader);
            Action act = () => headerMapper.MapHeaders(headers).ToArray();
            act.Should().Throw<InvalidOperationException>();
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
