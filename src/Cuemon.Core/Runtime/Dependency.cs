using System;
using Cuemon.Reflection;

namespace Cuemon.Runtime
{
    /// <summary>
    /// An abstract class for establishing various methods of a dependency relationship to an object. 
    /// The implementing class of the <see cref="Dependency"/> class must monitor the dependency relationships so that when any of them changes, action will automatically be taken.
    /// </summary>
    public abstract class Dependency : IDependency
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Dependency"/> class.
        /// </summary>
        protected Dependency()
        {
            UtcLastModified = DateTime.UtcNow;
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a <see cref="Dependency"/> has changed.
        /// </summary>
        public event EventHandler<DependencyEventArgs> DependencyChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Gets time when the dependency was last changed.
        /// </summary>
        /// <value>The time when the dependency was last changed.</value>
        /// <remarks>This property is measured in Coordinated Universal Time (UTC) (also known as Greenwich Mean Time).</remarks>
        public DateTime UtcLastModified { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Dependency"/> object has changed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the <see cref="Dependency"/> object has changed; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasChanged { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> that implements the <see cref="IDependency"/> interface using a constructor of five parameters.
        /// </summary>
        /// <typeparam name="TResult">The type to create that implements the <see cref="IDependency"/> interface.</typeparam>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult Create<TResult>() where TResult : IDependency
        {
            return ActivatorFactory.CreateInstance<TResult>();
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> that implements the <see cref="IDependency"/> interface using a constructor of five parameters.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create that implements the <see cref="IDependency"/> interface.</typeparam>
        /// <param name="arg">The parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult Create<T, TResult>(T arg) where TResult : IDependency
        {
            return ActivatorFactory.CreateInstance<T, TResult>(arg);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> that implements the <see cref="IDependency"/> interface using a constructor of five parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create that implements the <see cref="IDependency"/> interface.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult Create<T1, T2, TResult>(T1 arg1, T2 arg2) where TResult : IDependency
        {
            return ActivatorFactory.CreateInstance<T1, T2, TResult>(arg1, arg2);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> that implements the <see cref="IDependency"/> interface using a constructor of five parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create that implements the <see cref="IDependency"/> interface.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult Create<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3) where TResult : IDependency
        {
            return ActivatorFactory.CreateInstance<T1, T2, T3, TResult>(arg1, arg2, arg3);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> that implements the <see cref="IDependency"/> interface using a constructor of five parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create that implements the <see cref="IDependency"/> interface.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <param name="arg4">The fourth parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult Create<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TResult : IDependency
        {
            return ActivatorFactory.CreateInstance<T1, T2, T3, T4, TResult>(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> that implements the <see cref="IDependency"/> interface using a constructor of five parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create that implements the <see cref="IDependency"/> interface.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <param name="arg4">The fourth parameter of the constructor.</param>
        /// <param name="arg5">The fifth parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult Create<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TResult : IDependency
        {
            return ActivatorFactory.CreateInstance<T1, T2, T3, T4, T5, TResult>(arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Marks the time when a dependency last changed.
        /// </summary>
        /// <param name="utcLastModified">The time when the dependency last changed.</param>
        protected void SetUtcLastModified(DateTime utcLastModified)
        {
            if (utcLastModified.Kind != DateTimeKind.Utc) { throw new ArgumentException("The time from when the dependency was last changed, must be specified in  Coordinated Universal Time (UTC).", nameof(utcLastModified)); }
            UtcLastModified = utcLastModified;
        }

        /// <summary>
        /// Raises the <see cref="DependencyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DependencyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDependencyChangedRaised(DependencyEventArgs e)
        {
            var handler = DependencyChanged;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Starts and performs the necessary dependency tasks of this instance.
        /// </summary>
        public abstract void Start();
        #endregion
    }
}