namespace lindotnet.Classes
{
	public enum CallType
	{
		None,
		Incoming,
		Outcoming
	}

	public enum CallState
	{
		None,
		Loading,
		Active,
		Hold,
		Completed,
		Error
	}

	public enum ConnectState
	{
		/// <summary>
		///  Idle
		/// </summary>
		Disconnected,
		/// <summary>
		/// Registering on server
		/// </summary>
		Progress,
		/// <summary>
		/// Successfull registered
		/// </summary>
		Connected
	}

	public enum Error
	{
		/// <summary>
		/// Registration error
		/// </summary>
		RegisterFailed,

		/// <summary>
		/// Trying to make/receive call while another call is active
		/// </summary>
		LineIsBusyError,

		/// <summary>
		/// Trying to connect while connected / connecting or disconnect when not connected
		/// </summary>
		OrderError,

		/// <summary>
		/// Call failed
		/// </summary>
		CallError,

		/// <summary>
		/// Unknow error
		/// </summary>
		UnknownError
	}

	public enum LineState
	{
		Free,
		Busy
	}

	/// <summary>
	/// Logging level
	/// https://github.com/BelledonneCommunications/ortp/blob/master/include/ortp/logging.h
	/// https://github.com/BelledonneCommunications/bctoolbox/blob/master/include/bctoolbox/logging.h
	/// </summary>
	public enum OrtpLogLevel
	{
		DEBUG = 1,
		TRACE = 1 << 1,
		MESSAGE = 1 << 2,
		WARNING = 1 << 3,
		ERROR = 1 << 4,
		FATAL = 1 << 5,
		END = 1 << 6
	}

	/// <summary>
	/// Describes proxy registration states
	/// http://www.linphone.org/docs/liblinphone/group__proxies.html
	/// </summary>
	public enum LinphoneRegistrationState
	{
		/// <summary>
		/// Initial state for registrations
		/// </summary>
		LinphoneRegistrationNone,

		/// <summary>
		/// Registration is in progress
		/// </summary>
		LinphoneRegistrationProgress,

		/// <summary>
		/// Registration is successful
		/// </summary>
		LinphoneRegistrationOk,

		/// <summary>
		/// Unregistration succeeded
		/// </summary>
		LinphoneRegistrationCleared,

		/// <summary>
		/// Registration failed
		/// </summary>
		LinphoneRegistrationFailed
	}

	/// <summary>
	/// Represents the different state a call can reach into
	/// http://www.linphone.org/docs/liblinphone/group__call__control.html
	/// </summary>
	public enum LinphoneCallState
	{
		/// <summary>
		/// Initial call state
		/// </summary>
		LinphoneCallIdle,

		/// <summary>
		/// This is a new incoming call
		/// </summary>
		LinphoneCallIncomingReceived,

		/// <summary>
		/// An outgoing call is started
		/// </summary>
		LinphoneCallOutgoingInit,

		/// <summary>
		/// An outgoing call is in progress
		/// </summary>
		LinphoneCallOutgoingProgress,

		/// <summary>
		/// An outgoing call is ringing at remote end
		/// </summary>
		LinphoneCallOutgoingRinging,

		/// <summary>
		/// An outgoing call is proposed early media
		/// </summary>
		LinphoneCallOutgoingEarlyMedia,

		/// <summary>
		/// Connected, the call is answered
		/// </summary>
		LinphoneCallConnected,

		/// <summary>
		/// The media streams are established and running
		/// </summary>
		LinphoneCallStreamsRunning,

		/// <summary>
		/// The call is pausing at the initiative of local end
		/// </summary>
		LinphoneCallPausing,

		/// <summary>
		/// The call is paused, remote end has accepted the pause
		/// </summary>
		LinphoneCallPaused,

		/// <summary>
		/// The call is being resumed by local end
		/// </summary>
		LinphoneCallResuming,

		/// <summary>
		/// <The call is being transfered to another party, resulting in a new outgoing call to follow immediately
		/// </summary>
		LinphoneCallRefered,

		/// <summary>
		/// The call encountered an error
		/// </summary>
		LinphoneCallError,

		/// <summary>
		/// The call ended normally
		/// </summary>
		LinphoneCallEnd,

		/// <summary>
		/// The call is paused by remote end
		/// </summary>
		LinphoneCallPausedByRemote,

		/// <summary>
		/// The call's parameters change is requested by remote end, used for example when video is added by remote
		/// </summary>
		LinphoneCallUpdatedByRemote,

		/// <summary>
		/// We are proposing early media to an incoming call
		/// </summary>
		LinphoneCallIncomingEarlyMedia,

		/// <summary>
		/// A call update has been initiated by us
		/// </summary>
		LinphoneCallUpdating,

		/// <summary>
		/// The call object is no more retained by the core
		/// </summary>
		LinphoneCallReleased
	}

	public enum DeviceType
	{
		Playback,
		SoundCapture,
		VideoCapture
	}

	public enum LinphoneMediaEncryption
	{
		LinphoneMediaEncryptionNone,
		LinphoneMediaEncryptionSRTP,
		LinphoneMediaEncryptionZRTP,
		LinphoneMediaEncryptionDTLS
	}
}
