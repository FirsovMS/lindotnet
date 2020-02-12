using lindotnet.Classes.Component.Implementation;
using lindotnet.Classes.Helpers;
using System;

namespace lindotnet.Classes.Wrapper.Implementation
{
    public class LinphoneCall : Call
    {
        private IntPtr linphoneCallPtr;

        public IntPtr LinphoneCallPtr
        {
            get
            {
                if (linphoneCallPtr == null || linphoneCallPtr.IsZero())
                {
                    throw new ArgumentNullException("LinphoneCallPtr can't be null or zero pattern!");
                }
                return linphoneCallPtr;
            }
            set
            {
                linphoneCallPtr = value;
            }
        }
    }
}