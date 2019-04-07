using lindotnet.Classes;
using lindotnet.Classes.Component.Implementation;
using lindotnet.Classes.Component.Interfaces;
using System;
using System.Linq;

namespace DemoClient
{
	public class Application
	{
		private readonly Account account;
		private Softphone softphone;

		public Application()
		{
			account = new Account("test", "test", "local.dev", "localhost", accountName: "test");

			try
			{
				softphone = new Softphone(account);

				softphone.CallActiveEvent += Softphone_CallActiveEvent;
				softphone.CallCompletedEvent += Softphone_CallCompletedEvent;
				softphone.CallHolded += Softphone_CallHolded;
				softphone.PhoneConnectedEvent += Softphone_PhoneConnectedEvent;
				softphone.PhoneDisconnectedEvent += Softphone_PhoneDisconnectedEvent;
				softphone.ErrorEvent += Softphone_ErrorEvent;

				softphone.Connect();
			}
			catch (TypeInitializationException)
			{
				Console.WriteLine($"Can't load Linphone dll's!");
			}
		}

		private void Softphone_ErrorEvent(Call call, Error error, string message)
		{
			if(message.Contains("io error"))
			{
				throw new LinphoneException("Phone can't connected to server! IO Error!");
			}

			Console.WriteLine(message);
		}

		private void Softphone_PhoneDisconnectedEvent()
		{
			Console.WriteLine("Phone was disconected!");
		}

		private void Softphone_PhoneConnectedEvent()
		{
			Console.WriteLine("Phone connected!");

			Console.WriteLine("List of sound devices:");
			foreach (Device dev in softphone.MediaController.GetSoundDevices())
			{
				Console.Write("\t{0}\n", dev);
			}
		}

		private void Softphone_CallHolded(Call call)
		{
			Console.WriteLine($"Call {call.ToString()} holded!");
		}

		private void Softphone_CallCompletedEvent(Call call)
		{
			Console.WriteLine($"Call {call.ToString()} completed!");
		}

		private void Softphone_CallActiveEvent(Call call)
		{
			Console.WriteLine($"Call: {call.ToString()} active now!");
		}
	}
}
