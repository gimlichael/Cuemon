namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// Represents a breadcrumb that can be used for navigation purposes on a website.
    /// </summary>
    public class Breadcrumb
    {
        /// <summary>
        /// Gets or sets the label of the breadcrumb.
        /// </summary>
        /// <value>The label of the breadcrumb.</value>
        public string Label { get; set; }

        /// <summary>
        /// Gets the name of the action that this breadcrumb represents.
        /// </summary>
        /// <value>The name of the action that this breadcrumb represents.</value>
        public string ActionName { get; internal set; }

        /// <summary>
        /// Gets the name of the controller this breadcrumb is associated with.
        /// </summary>
        /// <value>The name of the controller this breadcrumb is associated with.</value>
        public string ControllerName { get; internal set; }
    }
}