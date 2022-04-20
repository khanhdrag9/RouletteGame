namespace Game.Helper
{
    using System.Collections.Generic;
    using System;

    public static class ServiceLocator
    {
        private static readonly IDictionary<Type, object> serviceCache = new Dictionary<Type, object> ();

        public static void Register<T>(T service)
        {
            var key = typeof(T);
            if (!serviceCache.ContainsKey(key))
            {
                serviceCache.Add(key, service);
            }
            else  // overwrite the existing instance.
            {
                serviceCache[key] = service;
            }
        }
 
        public static T GetService<T>()
        {
            var key = typeof(T);
            if (!serviceCache.ContainsKey(key))
            {
                throw new ArgumentException(string.Format("Type '{0}' has not been registered.", key.Name));
            }
    
            return (T)serviceCache[key];
        }
    }
}