namespace Cuemon
{
    internal class StringReplaceCoordinate
    {
        internal StringReplaceCoordinate(int startIndex, int length, string value)
        {
            this.StartIndex = startIndex;
            this.Length = length;
            this.Value = value;
        }

        internal int StartIndex { get; set; }
        internal int Length { get; set; }
        internal string Value { get; set; }
    }
}