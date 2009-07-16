﻿using System;
using System.Collections.Generic;
using System.Text;
using Hiro.Interfaces;
using System.Reflection;
using NGenerics.DataStructures.General;

namespace Hiro.Loaders
{
    /// <summary>
    /// Represents a class that can load a dependency map from a given set o fassemblies
    /// </summary>
    public class DependencyMapLoader
    {
        private IServiceLoader _serviceLoader;
        private IDefaultServiceResolver _defaultServiceResolver;

        /// <summary>
        /// Initializes a new instance of the DependencyMapLoader class.
        /// </summary>
        public DependencyMapLoader()
            : this(new ServiceLoader(), new DefaultServiceResolver())
        {
        }

        /// <summary>
        /// Initializes a new instance of the DependencyMapLoader class.
        /// </summary>
        /// <param name="serviceLoader">The service loader that will load services from a given assembly.</param>
        /// <param name="defaultServiceResolver">The resolver that will determine the default anonymous implementation for a particular service type.</param>
        public DependencyMapLoader(IServiceLoader serviceLoader, IDefaultServiceResolver defaultServiceResolver)
        {
            _serviceLoader = serviceLoader;
            _defaultServiceResolver = defaultServiceResolver;
        }

        /// <summary>
        /// Loads a dependency map using the types in the given <paramref name="assemblies"/>.
        /// </summary>
        /// <param name="assemblies">The list of assemblies that will be used to construct the dependency map.</param>
        /// <returns>A dependency map.</returns>
        public DependencyMap LoadFrom(IEnumerable<Assembly> assemblies)
        {
            var map = new DependencyMap();
            var serviceList = GetServiceList(assemblies);

            var defaultServices = GetDefaultServices(serviceList);

            map.RegisterNamedServices(serviceList);
            map.RegisterDefaultServices(defaultServices);

            return map;
        }

        /// <summary>
        /// Gets the list of default services from a given service list.
        /// </summary>
        /// <param name="serviceList">The list of service implementations that will be used to determine the default service for each service type.</param>
        /// <returns></returns>
        private List<IServiceInfo> GetDefaultServices(HashList<Type, IServiceInfo> serviceList)
        {
            // Get the default services for each service type
            var defaultServices = new List<IServiceInfo>();
            foreach (var serviceType in serviceList.Keys)
            {
                var services = serviceList[serviceType];
                var defaultService = _defaultServiceResolver.GetDefaultService(serviceType, services);

                // Use the default service as the anonymous service
                var anonymousService = new ServiceInfo(defaultService.ServiceType, defaultService.ImplementingType, null);
                defaultServices.Add(anonymousService);
            }

            return defaultServices;
        }


        /// <summary>
        /// Obtains a list of services (grouped by type) from the list of assemblies.
        /// </summary>
        /// <param name="assemblies">The list of assemblies that contain the types that will be injected into the dependency map.</param>
        /// <returns>A list of services grouped by type.</returns>
        private HashList<Type, IServiceInfo> GetServiceList(IEnumerable<Assembly> assemblies)
        {
            var serviceList = new HashList<Type, IServiceInfo>();
            foreach (var assembly in assemblies)
            {
                var services = _serviceLoader.Load(assembly);
                foreach (var service in services)
                {
                    var serviceName = service.ServiceName;
                    var serviceType = service.ServiceType;

                    // Group the services by service type
                    serviceList.Add(serviceType, service);
                }
            }

            return serviceList;
        }
    }
}
