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
			account = new Account("test", "testpass", "officesip.local", accountName: "TestUser");

			softphone = new Softphone(account);

			softphone.Connect();

			softphone.CallActiveEvent += Softphone_CallActiveEvent;
			softphone.CallCompletedEvent += Softphone_CallCompletedEvent;
			softphone.CallHolded += Softphone_CallHolded;
			softphone.PhoneConnectedEvent += Softphone_PhoneConnectedEvent;
			softphone.PhoneDisconnectedEvent += Softphone_PhoneDisconnectedEvent;
			softphone.ErrorEvent += Softphone_ErrorEvent;
		}

		private void Softphone_ErrorEvent(Call call, lindotnet.Classes.Error error, string message)
		{
			Console.WriteLine(message);
		}

		private void Softphone_PhoneDisconnectedEvent()
		{
			throw new NotImplementedException();
		}

		private void Softphone_PhoneConnectedEvent()
		{
			foreach (Device dev in softphone.MediaController.GetSoundDevices())
			{
				Console.WriteLine(dev);
			}
		}

		private void Softphone_CallHolded(Call call)
		{
			throw new NotImplementedException();
		}

		private void Softphone_CallCompletedEvent(Call call)
		{
			throw new NotImplementedException();
		}

		private void Softphone_CallActiveEvent(Call call)
		{
			throw new NotImplementedException();
		}
	}
}
