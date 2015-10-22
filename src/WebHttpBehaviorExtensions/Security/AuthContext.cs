using System;

namespace WebHttpBehaviorExtensions.Security
{
    public static class AuthContext
    {
        private static Func<IAuthenticationProvider> _currentProvider;

        /// <summary>
        /// The current ambient container.
        /// </summary>
        public static IAuthenticationProvider Current
        {
            get
            {
                if (!IsCustomAuthenticationProviderSet) throw new InvalidOperationException("No authentication strategy found!");

                return _currentProvider();
            }
        }

        /// <summary>
        /// Set the delegate that is used to retrieve the current provider.
        /// </summary>
        /// <param name="newProvider">Delegate that, when called, will return
        /// the current ambient container.</param>
        public static void SetAuthenticationProvider(Func<IAuthenticationProvider> newProvider)
        {
            _currentProvider = newProvider;
        }

        public static bool IsCustomAuthenticationProviderSet
        {
            get
            {
                return _currentProvider != null;
            }
        }
    }
}