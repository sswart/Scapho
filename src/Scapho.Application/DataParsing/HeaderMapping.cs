using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scapho.Application.DataParsing
{
    public record HeaderMapping(string Key, int Position);
    public record HeaderMapping<T>(string Key, int Position) : HeaderMapping(Key, Position);
    public record DateTimeMapping(string Key, int Position, string Format) : HeaderMapping<DateTime>(Key, Position);
    public record DecimalMapping(string Key, int Position, string DecimalSeparator) : HeaderMapping<decimal>(Key, Position);
}
