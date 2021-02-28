using System;
using System.Collections.Generic;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Represents different representations of a version scheme in a consistent way.
    /// </summary>
    public class VersionResult
    {
        private readonly Version _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionResult"/> class.
        /// </summary>
        /// <param name="alphanumericVersion">The <see cref="string"/> that represents a potential alphanumeric version.</param>
        public VersionResult(string alphanumericVersion)
        {
            AlphanumericVersion = alphanumericVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionResult"/> class.
        /// </summary>
        /// <param name="version">The <see cref="Version"/> that represents a numerical version {major.minor.build.revision}.</param>
        public VersionResult(Version version)
        {
            AlphanumericVersion = version?.ToString();
            _version = version;
        }

        /// <summary>
        /// Gets the alphanumeric version assigned to this instance.
        /// </summary>
        /// <value>The alphanumeric version assigned to this instance.</value>
        public string AlphanumericVersion { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has alphanumeric version assigned.
        /// </summary>
        /// <value><c>true</c> if this instance has alphanumeric version assigned; otherwise, <c>false</c>.</value>
        public bool HasAlphanumericVersion => !string.IsNullOrEmpty(AlphanumericVersion);

        /// <summary>
        /// Determines whether this instance represents a semantic version.
        /// </summary>
        /// <returns><c>true</c> if this instance represents a semantic version; otherwise, <c>false</c>.</returns>
        public bool IsSemanticVersion()
        {
            if (HasAlphanumericVersion)
            {
                var isSemantic = true;
                var versions = AlphanumericVersion.Split('.');
                foreach (var version in versions)
                {
                    isSemantic &= int.TryParse(version, out _);
                }
                return !isSemantic;
            }
            return false;
        }

        /// <summary>
        /// Converts this instance to an equivalent <see cref="Version"/> object.
        /// </summary>
        /// <returns>An equivalent <see cref="Version"/> object of this instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// Only a non-semantic version can be converted to a <see cref="Version"/> object.
        /// </exception>
        public Version ToVersion()
        {
            if (HasAlphanumericVersion && _version != null) { return _version; }
            if (HasAlphanumericVersion)
            {
                var versionComponents = new List<int>();
                var versions = AlphanumericVersion.Split('.');
                foreach (var version in versions)
                {
                    if (int.TryParse(version, out var v)) { versionComponents.Add(v); }
                }
                return new Version(DelimitedString.Create(versionComponents, o => o.Delimiter = "."));
            }
            throw new InvalidOperationException("Only a non-semantic version can be converted to a Version object.");
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return AlphanumericVersion;
        }
    }
}