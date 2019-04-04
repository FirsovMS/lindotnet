namespace lindotnet.Classes.Component.Implementation
{
	public class NatPolicy
	{
		public bool UseSTUN { get; private set; }

		public bool UseTURN { get; private set; }

		public bool UseICE { get; private set; }

		public bool UseUPNP { get; private set; }

		public string Server { get; private set; }

		public NatPolicy(bool useStun, bool useTurn, bool useIce, bool useUpnp, string server)
		{
			UseSTUN = useStun;
			UseTURN = useTurn;
			UseICE = useIce;
			UseUPNP = useUpnp;
			Server = server;
		}

		public static NatPolicy GetDefaultNatPolicy()
		{
			return new NatPolicy(false, false, false, false, string.Empty);
		}
	}
}
