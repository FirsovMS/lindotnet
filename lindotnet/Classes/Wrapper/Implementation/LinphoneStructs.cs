using System;

namespace lindotnet.Classes.Wrapper.Implementation
{
	public static class LinphoneStructs
	{
		/// <summary>
		/// Linphone core SIP transport ports
		/// http://www.linphone.org/docs/liblinphone/struct__LinphoneSipTransports.html
		/// </summary>
		public struct LCSipTransports
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

		/// <summary>
		/// Policy to use to pass through NATs/firewalls.
		/// https://github.com/BelledonneCommunications/linphone/blob/master/coreapi/private.h
		/// </summary>
		public struct LinphoneNatPolicy
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


		/// <summary>
		/// Holds all callbacks that the application should implement. None is mandatory.
		/// http://www.linphone.org/docs/liblinphone/struct__LinphoneCoreVTable.html
		/// </summary>
		public struct LinphoneCoreVTable
		{
			/// <summary>
			/// Notifies global state changes
			/// </summary>
			public IntPtr global_state_changed;

			/// <summary>
			/// Notifies registration state changes
			/// </summary>
			public IntPtr registration_state_changed;

			/// <summary>
			/// Notifies call state changes
			/// </summary>
			public IntPtr call_state_changed;

			/// <summary>
			/// Notify received presence events
			/// </summary>
			public IntPtr notify_presence_received;

			/// <summary>
			/// Notify received presence events
			/// </summary>
			public IntPtr notify_presence_received_for_uri_or_tel;

			/// <summary>
			/// Notify about pending presence subscription request
			/// </summary>
			public IntPtr new_subscription_requested;

			/// <summary>
			/// Ask the application some authentication information
			/// </summary>
			public IntPtr auth_info_requested;

			/// <summary>
			/// Ask the application some authentication information
			/// </summary>
			public IntPtr authentication_requested;

			/// <summary>
			/// Notifies that call log list has been updated
			/// </summary>
			public IntPtr call_log_updated;

			/// <summary>
			/// A message is received, can be text or external body
			/// </summary>
			public IntPtr message_received;

			/// <summary>
			/// An encrypted message is received but we can't decrypt it
			/// </summary>
			public IntPtr message_received_unable_decrypt;

			/// <summary>
			/// An is-composing notification has been received
			/// </summary>
			public IntPtr is_composing_received;

			/// <summary>
			/// A dtmf has been received received
			/// </summary>
			public IntPtr dtmf_received;

			/// <summary>
			/// An out of call refer was received
			/// </summary>
			public IntPtr refer_received;

			/// <summary>
			/// Notifies on change in the encryption of call streams
			/// </summary>
			public IntPtr call_encryption_changed;

			/// <summary>
			/// Notifies when a transfer is in progress
			/// </summary>
			public IntPtr transfer_state_changed;

			/// <summary>
			/// A LinphoneFriend's BuddyInfo has changed
			/// </summary>
			public IntPtr buddy_info_updated;

			/// <summary>
			/// Notifies on refreshing of call's statistics.
			/// </summary>
			public IntPtr call_stats_updated;

			/// <summary>
			/// Notifies an incoming informational message received.
			/// </summary>
			public IntPtr info_received;

			/// <summary>
			/// Notifies subscription state change
			/// </summary>
			public IntPtr subscription_state_changed;

			/// <summary>
			/// Notifies a an event notification, see linphone_core_subscribe()
			/// </summary>
			public IntPtr notify_received;

			/// <summary>
			/// Notifies publish state change (only from #LinphoneEvent api)
			/// </summary>
			public IntPtr publish_state_changed;

			/// <summary>
			/// Notifies configuring status changes
			/// </summary>
			public IntPtr configuring_status;

			/// <summary>
			/// Callback that notifies various events with human readable text (deprecated)
			/// </summary>
			[System.Obsolete]
			public IntPtr display_status;

			/// <summary>
			/// Callback to display a message to the user (deprecated)
			/// </summary>
			[System.Obsolete]
			public IntPtr display_message;

			/// <summary>
			/// Callback to display a warning to the user (deprecated)
			/// </summary>
			[System.Obsolete]
			public IntPtr display_warning;

			[System.Obsolete]
			public IntPtr display_url;

			/// <summary>
			/// Notifies the application that it should show up
			/// </summary>
			[System.Obsolete]
			public IntPtr show;

			/// <summary>
			/// Use #message_received instead <br> A text message has been received
			/// </summary>
			[System.Obsolete]
			public IntPtr text_received;

			/// <summary>
			/// Callback to store file received attached to a LinphoneChatMessage
			/// </summary>
			[System.Obsolete]
			public IntPtr file_transfer_recv;

			/// <summary>
			/// Callback to collect file chunk to be sent for a LinphoneChatMessage
			/// </summary>
			[System.Obsolete]
			public IntPtr file_transfer_send;

			/// <summary>
			/// Callback to indicate file transfer progress
			/// </summary>
			[System.Obsolete]
			public IntPtr file_transfer_progress_indication;

			/// <summary>
			/// Callback to report IP network status (I.E up/down)
			/// </summary>
			public IntPtr network_reachable;

			/// <summary>
			/// Callback to upload collected logs
			/// </summary>
			public IntPtr log_collection_upload_state_changed;

			/// <summary>
			/// Callback to indicate log collection upload progress
			/// </summary>
			public IntPtr log_collection_upload_progress_indication;

			public IntPtr friend_list_created;

			public IntPtr friend_list_removed;

			/// <summary>
			/// User data associated with the above callbacks
			/// </summary>
			public IntPtr user_data;
		}
	}
}
