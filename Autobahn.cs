using System;
using Mono.Terminal;

namespace Autobahn {

	public class Shell : Container {

		public Shell () : base (0, 0, Application.Cols, Application.Lines)
		{
			Add(new Label (0, 0, "Hello World"));
		}

		static void Main (String [] args) {

			Console.WriteLine ("Hello World");

			Application.Init (false);

			Shell s = new Shell ();
			Application.Run (s);

		}
	}
}
