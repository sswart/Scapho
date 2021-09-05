namespace Scapho.Application.POCO
{
    public struct Percentage
    {
        public Percentage(decimal percAsDecimal)
        {
            Value = percAsDecimal;
        }
        public Percentage(int percAsNumber)
        {
            Value = percAsNumber / 100m;
        }

        public static implicit operator decimal(Percentage p) => p.Value;
        public static implicit operator Percentage(decimal p) => new(p);

        public decimal Value { get; init; }
    }
}
