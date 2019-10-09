using System.Linq;
using System.Text;

namespace Cuemon.Data
{
    public class TokenBuilder
    {
        public TokenBuilder(char delimiter, char qualifier, int tokens)
        {
            Delimiter = delimiter;
            Qualifier = qualifier;
            Tokens = tokens;
        }

        public bool InsideQuotedField { get; private set; }

        public int Tokens { get; }

        public char Delimiter { get; }

        public char Qualifier { get; }

        public int CurrentTokenLength { get; private set; }

        private StringBuilder Result { get; } = new StringBuilder();

        public bool IsValid { get; private set; }

        public TokenBuilder Append(string value)
        {
            if (value != null)
            {
                Parse(value.ToCharArray());
            }
            return this;
        }

        private void Parse(char[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                var current = input[i];
                if ((current == Delimiter || i == (input.Length - 1)) && !InsideQuotedField)
                {
                    CurrentTokenLength++;
                }

                if (current == Delimiter && input.ElementAtOrDefault(i + 1) == Qualifier)
                {
                    InsideQuotedField = true;
                }

                if (current == Qualifier && input.ElementAtOrDefault(i + 1) == Delimiter)
                {
                    InsideQuotedField = false;
                }

                Result.Append(current);

                if (CurrentTokenLength == Tokens)
                {
                    IsValid = true;
                    break;
                }
            }
        }

        public override string ToString()
        {
            return Result.ToString();
        }
    }
}