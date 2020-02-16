using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.LinphoneDTO
{
    /// <summary>
    /// Policy to use to pass through NATs/firewalls.
    /// https://github.com/BelledonneCommunications/linphone/blob/master/coreapi/private.h
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class LinphoneNatPolicy
    {
        public IntPtr baseObject;
        public IntPtr user_data;
        public IntPtr lc;
        public IntPtr stun_resolver_context;
        public IntPtr stun_addrinfo;
        public IntPtr stun_server;
        public IntPtr stun_server_username;
        public IntPtr refObject;
        public IntPtr stun_enabled;
        public IntPtr turn_enabled;
        public IntPtr ice_enabled;
        public IntPtr upnp_enabled;
    }
}
