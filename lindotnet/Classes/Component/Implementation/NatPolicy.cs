namespace lindotnet.Classes.Component.Implementation
{
    public class NatPolicy
    {
        public bool UseSTUN { get; private set; }

        public bool UseTURN { get; private set; }

        public bool UseICE { get; private set; }

        public bool UseUPNP { get; private set; }

        public string Server { get; private set; }

        public NatPolicy(bool use_stun, bool use_turn, bool use_ice, bool use_upnp, string server)
        {
            UseSTUN = use_stun;
            UseTURN = use_turn;
            UseICE = use_ice;
            UseUPNP = use_upnp;
            Server = server;
        }

        public static NatPolicy GetDefaultNatPolicy()
        {
            return new NatPolicy(false, false, false, false, string.Empty);
        }
    }
}