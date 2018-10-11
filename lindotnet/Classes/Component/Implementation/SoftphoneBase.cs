using lindotnet.Classes.Component.Interfaces;
using lindotnet.Classes.Wrapper.Implementation;

namespace lindotnet.Classes.Component.Implementation
{
	internal abstract class SoftphoneBase : ISoftphoneBase
	{
		#region Props

		public ConnectState ConnectState { get; set; }

		public LineState LineState { get; set; }

		public Account Account { get; set; }

		public string Useragent { get; set; } = Constants.DefaultUserAgent;

		public string Version { get; set; } = Constants.ClientVersion;

		public LinphoneWrapper LinphoneWrapper { get; private set; }

		#endregion

		public SoftphoneBase(Account account)
		{
			Account = account ?? throw new LinphoneException("Softphone requires as Account to make calls!");
			LinphoneWrapper = new LinphoneWrapper();
		}

		#region Events

		/// <summary>
		/// Successful registered
		/// </summary>
		public delegate void OnPhoneConnected();

		/// <summary>
		/// Successful unregistered
		/// </summary>
		public delegate void OnPhoneDisconnected();

		/// <summary>
		/// Phone is ringing
		/// </summary>
		/// <param name="call"></param>
		public delegate void OnIncomingCall(Call call);

		/// <summary>
		/// Link is established
		/// </summary>
		/// <param name="call"></param>
		public delegate void OnCallActive(Call call);

		/// <summary>
		/// Call completed
		/// </summary>
		/// <param name="call"></param>
		public delegate void OnCallCompleted(Call call);

		/// <summary>
		/// Message received
		/// </summary>
		/// <param name="call"></param>
		public delegate void OnMessageReceived(string from, string message);

		/// <summary>
		/// Error notification
		/// </summary>
		/// <param name="call"></param>
		/// <param name="error"></param>
		public delegate void OnError(Call call, Error error);

		/// <summary>
		/// Call Holded
		/// </summary>
		/// <param name="call"></param>
		public delegate void OnHold(Call call);

		/// <summary>
		/// Raw log notification
		/// </summary>
		/// <param name="message"></param>
		public delegate void OnLog(string message);

		public event OnPhoneConnected PhoneConnectedEvent;

		public event OnPhoneDisconnected PhoneDisconnectedEvent;

		public event OnIncomingCall IncomingCallEvent;

		public event OnCallActive CallActiveEvent;

		public event OnCallCompleted CallCompletedEvent;

		public event OnMessageReceived MessageReceivedEvent;

		public event OnError ErrorEvent;

		public event OnHold CallHolded;

		#endregion

		#region Logger

		private event OnLog logEventHandler;

		public event OnLog LogEvent
		{
			add
			{
				logEventHandler += value;
				if (!LinphoneWrapper.LogsEnabled)
				{
					LinphoneWrapper.LogsEnabled = true;
					LinphoneWrapper.LogEvent += (message) =>
					{
						logEventHandler?.Invoke(message);
					};
				}
			}

			remove
			{
				logEventHandler -= value;
				if (logEventHandler == null)
				{
					LinphoneWrapper.LogsEnabled = false;
				}
			}
		}

		#endregion

		#region Methods



		#endregion
	}
}
