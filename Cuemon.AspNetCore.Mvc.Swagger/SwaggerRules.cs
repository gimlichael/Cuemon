using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Validation keywords in a schema impose requirements for successfully validating an instance.
    /// </summary>
    public class SwaggerRules
    {
        /// <summary>
        /// Gets or sets the value representing an upper limit for a numeric instance.
        /// </summary>
        /// <value>The maximum upper limit for a numeric instance.</value>
        /// <remarks>If the instance is a number, then this keyword validates if "exclusiveMaximum" is true and instance is less than the provided value, or else if the instance is less than or exactly equal to the provided value.</remarks>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets a value representing whether the limit in <see cref="Maximum"/> is exclusive or not.
        /// </summary>
        /// <value><c>true</c> if the limit in <see cref="Maximum"/> is exclusive; otherwise, <c>false</c>.</value>
        /// <remarks>If "exclusiveMaximum" is true, then a numeric instance SHOULD NOT be equal to the value specified in "maximum". If "exclusiveMaximum" is false (or not specified), then a numeric instance MAY be equal to the value of "maximum".</remarks>
        public bool ExclusiveMaximum { get; set; }

        /// <summary>
        /// Gets or sets the value representing a lower limit for a numeric instance.
        /// </summary>
        /// <value>The minimum lower limit for a numeric instance.</value>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets a value representing whether the limit in <see cref="Minimum"/> is exclusive or not. 
        /// </summary>
        /// <value><c>true</c> if the limit in <see cref="Minimum"/> is exclusive; otherwise, <c>false</c>.</value>
        /// <remarks>If "exclusiveMinimum" is true, then a numeric instance SHOULD NOT be equal to the value specified in "minimum". If "exclusiveMinimum" is false (or not specified), then a numeric instance MAY be equal to the value of "minimum".</remarks>
        public bool ExclusiveMinimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of a string instance that MUST be greater than, or equal to, 0.
        /// </summary>
        /// <value>The maximum length of a string instance.</value>
        /// <remarks>The length of a string instance is defined as the number of its characters as defined by RFC 7159 [RFC7159].</remarks>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of a string instance that MUST be greater than, or equal to, 0.
        /// </summary>
        /// <value>The minimum length of a string instance.</value>
        /// <remarks>The length of a string instance is defined as the number of its characters as defined by RFC 7159 [RFC7159].</remarks>
        public int MinLength { get; set; }

        /// <summary>
        /// Gets or sets the pattern that SHOULD be a valid regular expression, according to the ECMA 262 regular expression dialect.
        /// </summary>
        /// <value>The pattern that SHOULD be a valid regular expression.</value>
        /// <remarks>A string instance is considered valid if the regular expression matches the instance successfully. Recall: regular expressions are not implicitly anchored.</remarks>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets or sets the maximum items that is allowed in an array instance.
        /// </summary>
        /// <value>The maximum items that is allowed in an array instance.</value>
        /// <remarks>An array instance is valid against "maxItems" if its size is less than, or equal to, the value of this keyword.</remarks>
        public int MaxItems { get; set; }

        /// <summary>
        /// Gets or sets the minimum items that is allowed in an array instance.
        /// </summary>
        /// <value>The minimum items that is allowed in an array instance.</value>
        /// <remarks>An array instance is valid against "minItems" if its size is greater than, or equal to, the value of this keyword.</remarks>
        public int MinItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all elements of an array instance must be unique.
        /// </summary>
        /// <value><c>true</c> if all elements of an array instance must be unique; otherwise, <c>false</c>.</value>
        /// <remarks>If this keyword has boolean value false, the instance validates successfully. If it has boolean value true, the instance validates successfully if all of its elements are unique.</remarks>
        public bool UniqueItems { get; set; }

        /// <summary>
        /// Gets the enum values of an array instance.
        /// </summary>
        /// <value>The enum values of an array instance.</value>
        /// <remarks>An instance validates successfully against this keyword if its value is equal to one of the elements in this keyword's array value.</remarks>
        public IList<object> Enum { get; } = new List<object>();

        /// <summary>
        /// Gets or sets the value that when divided by a numeric instance must result in an integer.
        /// </summary>
        /// <value>The value that when divided by a numeric instance must result in an integer.</value>
        /// <remarks>A numeric instance is only valid if division by this keyword's value results in an integer.</remarks>
        public double MultipleOf { get; set; }

        /// <summary>
        /// Gets or sets the default value of the item that the server will use if none is provided.
        /// </summary>
        /// <value>The default value of the item that the server will use if none is provided.</value>
        public object Default { get; set; }
    }
}