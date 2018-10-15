using lindotnet.Classes.Component.Interfaces;
using lindotnet.Classes.Wrapper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lindotnet.Classes.Component.Implementation
{
    internal class Softphone : SoftphoneBase, ISoftphone
    {
        #region Props

        public Media MediaController { get; private set; }

        public LinphoneCall ActiveCall { get; private set; }

        #endregion

        public Softphone(Account account) : base(account)
        {
            MediaController = new Media(this);
        }

        #region Implement interface

        public void HoldCall(Call call)
        {
            CheckError(call);
            var linphonceCall = call as LinphoneCall;
            if (linphonceCall != null)
            {
                LinphoneWrapper.HoldCall(linphonceCall);
            }
        }

        public void MakeCall(string uri)
        {
            CheckError();
            CheckError(uri);
            if (LineState == LineState.Free)
            {
                LinphoneWrapper.MakeCall(uri);
            }
        }

        public void MakeCallAndRecord(string uri, string filename, bool recordStartInstantly = false)
        {
            CheckError();
            CheckError(filename, uri);
            if (LineState == LineState.Free)
            {
                LinphoneWrapper.MakeCallAndRecord(uri, filename, recordStartInstantly);
            }
        }

        public void PauseRecording(Call call)
        {
            CheckError(call);
            var linphoneCall = call as LinphoneCall;
            if (linphoneCall != null)
            {
                LinphoneWrapper.PauseRecording(linphoneCall);
            }
        }

        public void ReceiveCall(Call call)
        {
            CheckError(call);
            var linphoneCall = call as LinphoneCall;
            if (linphoneCall != null)
            {
                LinphoneWrapper.ReceiveCall(linphoneCall);
            }
        }

        public void ReceiveCallAndRecord(Call call, string filename, bool recordStartInstantly = false)
        {
            CheckError(call, filename);
            var linphoneCall = call as LinphoneCall;
            if (linphoneCall != null)
            {
                LinphoneWrapper.ReceiveCallAndRecord(linphoneCall, filename, recordStartInstantly);
            }
        }

        public void RedirectCall(Call call, string redirectURI)
        {
            CheckError(call, redirectURI);
            var linphoneCall = call as LinphoneCall;
            if (linphoneCall != null)
            {
                LinphoneWrapper.RedirectCall(linphoneCall, redirectURI);
            }
        }

        public void ResumeCall(Call call)
        {
            CheckError(call);
            var linphoneCall = call as LinphoneCall;
            if (linphoneCall != null)
            {
                LinphoneWrapper.ResumeCall(linphoneCall);
            }
        }

        public void SendDTMFs(Call call, string dtmfs)
        {
            CheckError(call, dtmfs);
            var linphoneCall = call as LinphoneCall;
            if (linphoneCall != null)
            {
                LinphoneWrapper.SendDTMFs(linphoneCall, dtmfs);
            }
        }

        public void SendMessage(string to, string message)
        {
            CheckError();
            CheckError(to, message);
            LinphoneWrapper.SendMessage(to, message);
        }

        public void SetIncomingRingSound(string filename)
        {
            CheckError();
            CheckError(filename);
            LinphoneWrapper.SetIncomingRingSound(filename);
        }

        public void SetRingbackSound(string filename)
        {
            CheckError();
            CheckError(filename);
            LinphoneWrapper.SetRingbackSound(filename);
        }

        public void StartRecording(Call call)
        {
            CheckError(call);
            var linphoneCall = call as LinphoneCall;
            if (linphoneCall != null)
            {
                LinphoneWrapper.StartRecording(linphoneCall);
            }
        }

        public void TerminateCall(Call call)
        {
            CheckError(call);
            var linphoneCall = call as LinphoneCall;
            if (linphoneCall != null)
            {
                LinphoneWrapper.TerminateCall(linphoneCall);
            }
        }

        public void TransferCall(Call call, string redirectURI)
        {
            CheckError(call, redirectURI);
            var linphoneCall = call as LinphoneCall;
            if (linphoneCall != null)
            {
                LinphoneWrapper.TransferCall(linphoneCall, redirectURI);
            }
        }

        #endregion

        #region Methods

        private void CheckError()
        {
            if (ConnectState != ConnectState.Connected)
            {
                throw new LinphoneException("Softphone didn't connected!");
            }
            if (LinphoneWrapper == null || LinphoneWrapper?.IsRunning == false)
            {
                throw new LinphoneException("Wrapper didn't ready!");
            }
        }

        private void CheckError(Call call)
        {
            CheckError();
            if (call == null)
            {
                throw new LinphoneException("Call can't be null!");
            }
        }

        private void CheckError(Call call, params string[] additionalStringParams)
        {
            CheckError(call);
            CheckError(additionalStringParams);
        }

        private void CheckError(params string[] additionalStringParams)
        {
            if (additionalStringParams.Any(str => string.IsNullOrWhiteSpace(str)))
            {
                throw new ArgumentException("Additional string must be not null, or empty, or whitespace!");
            }
        }

        #endregion
    }
}
