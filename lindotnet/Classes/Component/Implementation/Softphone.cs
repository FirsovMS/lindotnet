using lindotnet.Classes.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lindotnet.Classes.Component.Implementation
{
	internal class Softphone : SoftphoneBase, ISoftphone
	{
		public Softphone(Account account) : base(account)
		{

		}


	}
}
