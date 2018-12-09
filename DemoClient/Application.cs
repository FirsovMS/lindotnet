using lindotnet.Classes.Component.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClient
{
	public class Application
	{
		private Account account;
		private Softphone softphone;

		public Application()
		{
			account = new Account("login", "password", "localhost");
			softphone = new Softphone(account);

			softphone.CallActiveEvent += Softphone_CallActiveEvent;
			softphone.CallCompletedEvent += Softphone_CallCompletedEvent;
			softphone.CallHolded += Softphone_CallHolded;
			softphone.PhoneConnectedEvent += Softphone_PhoneConnectedEvent;
			softphone.PhoneDisconnectedEvent += Softphone_PhoneDisconnectedEvent;
			softphone.ErrorEvent += Softphone_ErrorEvent;
		}

		private void Softphone_ErrorEvent(Call call, lindotnet.Classes.Error error)
		{
			throw new NotImplementedException();
		}

		private void Softphone_PhoneDisconnectedEvent()
		{
			throw new NotImplementedException();
		}

		private void Softphone_PhoneConnectedEvent()
		{
			throw new NotImplementedException();
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
