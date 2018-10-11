using System;
using lindotnet.Classes.Component.Implementation;

namespace lindotnet.Classes.Wrapper.Implementation
{
    internal class LinphoneCall : Call
    {
        public IntPtr LinphoneCallPtr { get; set; }        
    }
}
