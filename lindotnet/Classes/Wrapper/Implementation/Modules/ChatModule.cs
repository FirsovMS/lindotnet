using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Modules
{
    /// <summary>
    /// http://www.linphone.org/docs/liblinphone/group__chatroom.html
    /// </summary>
    internal static class ChatModule
    {
        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_core_get_chat_room_from_uri(IntPtr lc, string to);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_chat_room_send_chat_message(IntPtr chatroom, IntPtr chatmessage);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_chat_room_create_message(IntPtr chatroom, string chatmessage);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_chat_room_get_peer_address(IntPtr chatroom);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_chat_message_get_text(IntPtr chatmessage);
    }
}