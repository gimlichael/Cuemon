using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cuemon.Security.Cryptography;

namespace Cuemon.Security
{
    /// <summary>
    /// Provides methods for obfuscation of documents for reduced readability and size.
    /// </summary>
    public abstract class Obfuscator
    {
        private readonly List<string> _obfuscatedValues;
        private readonly List<string> _exclusions;
        private IList<char> _permutationCharacters;
        private static readonly object PadLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Obfuscator"/> class using <see cref="System.Text.Encoding.Unicode"/> for the character encoding.
        /// </summary>
        protected Obfuscator() : this(Encoding.Unicode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obfuscator"/> class using <see cref="System.Text.Encoding.Unicode"/> for the character encoding.
        /// </summary>
        /// <param name="exclusions">A sequence of <see cref="string"/> values used for excluding matching original values in the obfuscation process.</param>
        protected Obfuscator(IEnumerable<string> exclusions) : this(Encoding.Unicode, exclusions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obfuscator"/> class.
        /// </summary>
        /// <param name="encoding">The character encoding to use.</param>
        protected Obfuscator(Encoding encoding) : this(encoding, null)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Obfuscator"/> class.
        /// </summary>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="exclusions">A sequence of <see cref="string"/> values used for excluding matching original values in the obfuscation process.</param>
        protected Obfuscator(Encoding encoding, IEnumerable<string> exclusions)
        {
            Validator.ThrowIfNull(encoding, nameof(encoding));
            Encoding = encoding;
            _exclusions = exclusions == null ? new List<string>() : new List<string>(exclusions);
            Mappings = new Dictionary<string, ObfuscatorMapping>();
            _obfuscatedValues = new List<string>();
        }

        /// <summary>
        /// Gets the length of the current combination used in conjuction with <see cref="CurrentPermutationSize"/>.
        /// </summary>
        /// <value>The length of the current combination used in conjuction with <see cref="CurrentPermutationSize"/>.</value>
        protected byte CurrentCombinationLength { get; private set; }

        /// <summary>
        /// Gets the calculated size of the current permutation used in conjuction with <see cref="CurrentCombinationLength"/>.
        /// </summary>
        /// <value>The calculated size of the current permutation used in conjuction with <see cref="CurrentCombinationLength"/>.</value>
        protected int CurrentPermutationSize { get; private set; }

        /// <summary>
        /// Gets or sets the character encoding to use.
        /// </summary>
        /// <value>The character encoding to use. The default is <see cref="System.Text.Encoding.Unicode"/>.</value>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets an <see cref="IList{T}"/> compatible object holding the characters initialized by <see cref="InitializePermutationCharacters()"/>.
        /// </summary>
        /// <value>An <see cref="IList{T}"/> compatible object holding the characters initialized by <see cref="InitializePermutationCharacters()"/>.</value>
        protected IList<char> PermutationCharacters => _permutationCharacters ?? (_permutationCharacters = InitializePermutationCharacters());

        /// <summary>
        /// Gets a <see cref="ICollection{T}"/> of the generated obfuscated values.
        /// </summary>
        /// <value>A <see cref="ICollection{T}"/> of the generated obfuscated values.</value>
        private ICollection<string> ObfuscatedValues => _obfuscatedValues;

        /// <summary>
        /// Gets a list of exclusions for the obfuscation process.
        /// </summary>
        /// <value>A list of exclusions for the obfuscation process.</value>
        protected ICollection<string> Exclusions => _exclusions;

        /// <summary>
        /// Gets the generated mapping values associated with the obfuscated content.
        /// </summary>
        /// <value>The original values gathered in a mappable structure.</value>
        protected Dictionary<string, ObfuscatorMapping> Mappings { get; }

        /// <summary>
        /// Initializes the permutation characters used in the obfuscation process.
        /// </summary>
        /// <returns>An <see cref="IList{T}"/> compatible object holding the permuation characters used in the obfuscation process.</returns>
        /// <remarks>
        /// Override this method to implement your own initialization of permuation characters.
        /// Default implementation is initialized as a read-only <see cref="IList{T}"/> compatible object with the values found in <see cref="StringUtility.AlphanumericCharactersCaseSensitive"/>.
        /// </remarks>
        protected virtual IList<char> InitializePermutationCharacters()
        {
            var builder = new StringBuilder(StringUtility.AlphanumericCharactersCaseSensitive);
            var characters = new List<char>(builder.ToString().ToCharArray());
            return characters.AsReadOnly();
        }

        /// <summary>
        /// Computes a SHA-1 hash value of the specified <see cref="string"/> value.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <returns>A <see cref="string"/> containing the computed SHA-1 hash value of <paramref name="value"/>.</returns>
        /// <remarks>Do override this method if you prefer another hashing than SHA-1.</remarks>
        protected virtual string ComputeHash(string value)
        {
            return HashUtility.ComputeHash(value, o =>
            {
                o.AlgorithmType = HashAlgorithmType.SHA1;
                o.Encoding = Encoding;
            }).ToHexadecimalString();
        }
        
        private string GeneratePermutationValue()
        {
            var values = new StringBuilder(CurrentCombinationLength);
            for (byte i = 0; i < CurrentCombinationLength; i++)
            {
                values.Append(PermutationCharacters[NumberUtility.GetRandomNumber(0, PermutationCharacters.Count, RandomSeverity.Strong)].ToString());
            }
            return values.ToString();
        }

        /// <summary>
        /// Creates and returns a random generated <see cref="string"/> with a factorial size of (x - <see cref="CurrentCombinationLength"/>) ideal for obfuscated usage.
        /// </summary>
        /// <returns>A random generated <see cref="string"/> with a factorial size of (x - <see cref="CurrentCombinationLength"/>) ideal for obfuscated usage, where x in the default implementation equals 62.</returns>
        /// <remarks>
        /// This implementation uses the values from <see cref="PermutationCharacters"/> to compute the permutation.
        /// Using the standard implementation gives a close to limitless permutation in regards to obfuscation, as the value automatically grows when needed.
        /// </remarks>
        protected string GenerateObfuscatedValue()
        {
            HandlePermutationCalculation();
            var obfuscatedValue = GenerateObfuscatedValue(GeneratePermutationValue());
            lock (ObfuscatedValues)
            {
                ObfuscatedValues.Add(obfuscatedValue);
            }
            return obfuscatedValue;
        }

        private string GenerateObfuscatedValue(string value)
        {
            lock (ObfuscatedValues)
            {
                if (!ObfuscatedValues.Contains(value))
                {
                    return value;
                }
            }
            return GenerateObfuscatedValue(GeneratePermutationValue());
        }

        private void HandlePermutationCalculation()
        {
            if (CurrentPermutationSize == ObfuscatedValues.Count)
            {
                lock (PadLock)
                {
                    if (CurrentPermutationSize == ObfuscatedValues.Count)
                    {
                        IncrementCurrentCombinationLength();
                        IncreaseCurrentPermutationSize();
                    }
                }
            }
        }

        private void IncrementCurrentCombinationLength()
        {
            CurrentCombinationLength++;
        }

        private void IncreaseCurrentPermutationSize()
        {
            CurrentPermutationSize = (int)Math.Round((NumberUtility.Factorial(PermutationCharacters.Count) / NumberUtility.Factorial(PermutationCharacters.Count - CurrentCombinationLength)));
        }

        /// <summary>
        /// Creates and returns a mappaple format of the original values and the obfuscated values.
        /// </summary>
        /// <returns>A mappaple format of the original values and the obfuscated values.</returns>
        public abstract Stream CreateMapping();

        /// <summary>
        /// Obfuscates the values of the specified <see cref="Stream"/> object.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to obfuscate.</param>
        /// <returns>A <see cref="Stream"/> object where the values has been obfuscated.</returns>
        public abstract Stream Obfuscate(Stream value);

        /// <summary>
        /// Revert the obfuscated value of <paramref name="obfuscated"/> to its original state by applying the mappaple values of <paramref name="mapping"/>.
        /// </summary>
        /// <param name="obfuscated">The obfuscated <see cref="Stream"/> to revert.</param>
        /// <param name="mapping">A <see cref="Stream"/> containing mappaple values necessary to revert <paramref name="obfuscated"/> to its original state.</param>
        /// <returns>A <see cref="Stream"/> object where the obfuscated value has been reverted to its original value.</returns>
        public abstract Stream Revert(Stream obfuscated, Stream mapping);
    }
}