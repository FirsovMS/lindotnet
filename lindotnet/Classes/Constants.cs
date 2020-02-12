namespace lindotnet.Classes
{
    internal static class Constants
    {
        internal static readonly string ClientVersion = "1.0.1";

        internal static readonly string DefaultUserAgent = "liblinphone";

#if (WINDOWS)
		internal const string LIBNAME = "linphone.dll";
#else
        internal const string LIBNAME = "liblinphone";
#endif

        /// <summary>
        /// Disable a sip transport
        /// </summary>
        internal static readonly int LC_SIP_TRANSPORT_DISABLED = 0;

        /// <summary>
        /// Randomly chose a sip port for this transport
        /// </summary>
        internal static readonly int LC_SIP_TRANSPORT_RANDOM = -1;

        /// <summary>
        /// Don't create any server socket for this transport, ie don't bind on any port
        /// </summary>
        internal static readonly int LC_SIP_TRANSPORT_DONTBIND = -2;

        internal static readonly int LC_SLEEP_TIMEOUT = 100;

        internal static readonly int LC_CORE_PROXY_DISABLE_TIMEOUT = 2000;

        internal static readonly int BOOL_T_FAILED_CODE = -1;

        /// <summary>
        /// All necessary relocations shall be performed when the object is first loaded.
        /// This may waste some processing if relocations are performed for functions that are never referenced.
        /// This behavior may be useful for applications that need to know as soon as an object is loaded that all
        /// symbols referenced during execution are available.
        /// </summary>
        internal static readonly int RTLD_NOW = 2;
    }
}