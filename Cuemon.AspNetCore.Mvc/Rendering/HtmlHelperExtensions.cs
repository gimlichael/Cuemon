using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cuemon.AspNetCore.Mvc.Rendering
{
    /// <summary>
    /// Extension methods for the <see cref="IHtmlHelper"/> interface.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Creates a value of <typeparamref name="T"/> when the specified <paramref name="action"/> name and <paramref name="controller"/> name is matched in <see cref="ActionContext.RouteData"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value for the matched action method.</typeparam>
        /// <param name="helper">The <see cref="IHtmlHelper"/> to extend.</param>
        /// <param name="action">The action name to match.</param>
        /// <param name="controller">The controller name to match.</param>
        /// <param name="result">The function delegate that, when both <paramref name="action"/> and <paramref name="controller"/> is matched in <see cref="ActionContext.RouteData"/>, returns <typeparamref name="T"/>.</param>
        /// <returns>Either the value of the function delegate <paramref name="result"/> or <b>default(T)</b>.</returns>
        public static T UseWhen<T>(this IHtmlHelper helper, string action, string controller, Func<T> result)
        {
            var viewAction = helper.ViewContext.RouteData.Values["action"] as string;
            var viewController = helper.ViewContext.RouteData.Values["controller"] as string;
            var actions = action.Split(',');
            var controllers = controller.Split(',');
            foreach (var a in actions)
            {
                foreach (var c in controllers)
                {
                    var match = viewAction?.Equals(a, StringComparison.OrdinalIgnoreCase) ?? false;
                    match &= viewController?.Equals(c, StringComparison.OrdinalIgnoreCase) ?? false;
                    if (match) { return result(); }
                }
            }
            return default(T);
        }

        /// <summary>
        /// Invokes the delegate <paramref name="body"/> when the specified <paramref name="action"/> name and <paramref name="controller"/> name is matched in <see cref="ActionContext.RouteData"/>.
        /// </summary>
        /// <param name="helper">The <see cref="IHtmlHelper"/> to extend.</param>
        /// <param name="action">The action name to match.</param>
        /// <param name="controller">The controller name to match.</param>
        /// <param name="body">The delegate that, when both <paramref name="action"/> and <paramref name="controller"/> is matched in <see cref="ActionContext.RouteData"/>, is invoked.</param>
        public static void UseWhen(this IHtmlHelper helper, string action, string controller, Action body)
        {
            var viewAction = helper.ViewContext.RouteData.Values["action"] as string;
            var viewController = helper.ViewContext.RouteData.Values["controller"] as string;
            var actions = action.Split(',');
            var controllers = controller.Split(',');
            foreach (var a in actions)
            {
                foreach (var c in controllers)
                {
                    var match = viewAction?.Equals(a, StringComparison.OrdinalIgnoreCase) ?? false;
                    match &= viewController?.Equals(c, StringComparison.OrdinalIgnoreCase) ?? false;
                    if (match)
                    {
                        body();
                        return;
                    }
                }
            }
        }
    }
}