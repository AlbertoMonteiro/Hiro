﻿using System;
using System.Collections.Generic;
using System.Text;
using Hiro.Containers;

namespace Hiro
{
    /// <summary>
    /// A helper class that adds syntactic sugar to the <see cref="IMicroContainer"/> interface.
    /// </summary>
    public static class MicroContainerExtensions
    {
        /// <summary>
        /// Obtains an object instance that matches the given service type
        /// and <paramref name="key">service name</paramref>.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <param name="container">The target container.</param>
        /// <returns>An object instance that matches the given service description.</returns>
        public static T GetInstance<T>(this IMicroContainer container)
        {
            return container.GetInstance<T>(null);
        }

        /// <summary>
        /// Obtains an object instance that matches the given service type
        /// and <paramref name="key">service name</paramref>.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// /// <param name="container">The target container.</param>
        /// <param name="key">The name of the service itself.</param>
        /// <returns>An object instance that matches the given service description.</returns>
        public static T GetInstance<T>(this IMicroContainer container, string key)
        {
            return (T)container.GetInstance(typeof(T), key);
        }

        /// <summary>
        /// Adds a service instance to the container.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <param name="container">The container instance itself.</param>
        /// <param name="serviceInstance">The service instance.</param>
        public static void AddService<T>(this IMicroContainer container, T serviceInstance)
        {
            container.AddService<T>(null, serviceInstance);
        }

        /// <summary>
        /// Adds a service instance to the container.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <param name="container">The container instance itself.</param>
        /// <param name="serviceName">The service name.</param>
        /// <param name="serviceInstance">The service instance.</param>
        public static void AddService<T>(this IMicroContainer container, string serviceName, T serviceInstance)
        {
            // Find the next available container slot
            var targetContainer = container;
            while (targetContainer.NextContainer != null)
            {
                targetContainer = targetContainer.NextContainer;
            }

            targetContainer.NextContainer = new InstanceContainer(typeof(T), serviceName, serviceInstance);
        }
    }
}