using lindotnet.Classes.Component.Implementation;
using System;

namespace lindotnet.Classes.Wrapper.Implementation
{
    public struct LinphoneConnectionParams
    {
        private string username;

        private string accountAlias;

        private string password;

        private string server;

        private int port;

        private string agent;

        private string version;

        private NatPolicy natPolicy;

        public string Username
        {
            get { return username; }
            set
            {
                CheckError(value, "Username");
                username = value;
            }
        }

        public string AccountAlias
        {
            get { return accountAlias; }
            set
            {
                CheckError(value, "AccountAlias");
                accountAlias = value;
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                CheckError(value, "Password");
                password = value;
            }
        }

        public string Host
        {
            get { return server; }
            set
            {
                CheckError(value, "Server");
                server = value;
            }
        }

        public int Port
        {
            get { return port; }
            set
            {
                if (value >= 0 && value <= UInt16.MaxValue)
                {
                    port = value;
                }
                else
                {
                    throw new LinphoneException("The Port number is unsigned 16-bit integer!");
                }
            }
        }

        public string Agent
        {
            get { return agent; }
            set
            {
                CheckError(value, "Agent");
                agent = value;
            }
        }

        public string Version
        {
            get { return version; }
            set
            {
                CheckError(value, "Version");
                version = value;
            }
        }

        public NatPolicy NatPolicy
        {
            get { return natPolicy; }
            set { natPolicy = value ?? NatPolicy.GetDefaultNatPolicy(); }
        }

        private static void CheckError(string fieldValue, string propName)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                throw new LinphoneException($"{propName} can't be null or empty!");
            }
        }
    }
}