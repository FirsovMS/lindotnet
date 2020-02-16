using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.LinphoneDTO
{
    /// <summary>
    /// Linphone core SIP transport ports
    /// http://www.linphone.org/docs/liblinphone/struct__LinphoneSipTransports.html
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class LCSipTransports
    {
        /// <summary>
        /// UDP port to listening on, negative value if not set
        /// </summary>
        public int udp_port;

        /// <summary>
        /// TCP port to listening on, negative value if not set
        /// </summary>
        public int tcp_port;

        /// <summary>
        /// DTLS port to listening on, negative value if not set
        /// </summary>
        public int dtls_port;

        /// <summary>
        /// TLS port to listening on, negative value if not set
        /// </summary>
        public int tls_port;
    }
}