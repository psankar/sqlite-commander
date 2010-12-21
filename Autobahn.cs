using System;
using System.Data;
using Mono.Terminal;
using Mono.Data.SqliteClient;

namespace Autobahn {

	public class Shell : Container {

		public Shell () : base (0, 0, Application.Cols, Application.Lines)
		{
			Add (new Label (0, 0, "Hello World"));
		}

		static void Main (String [] args) {

			if (args.Length != 1)
				Console.WriteLine ("Insufficient number of parameters");
			else {
				string connectionString;
				string sql;
				IDbCommand command;
				IDbConnection db_connection;
				IDataReader reader;

				connectionString = "URI=file:" + args[0];
				db_connection = (IDbConnection) new SqliteConnection(connectionString);
				db_connection.Open ();
				command = db_connection.CreateCommand ();
				sql = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name";
				command.CommandText = sql;
				reader = command.ExecuteReader ();
				while (reader.Read ()) {
					string TableName = reader.GetString (0);
					Console.WriteLine("Name: " + TableName);
				}

				// clean up
				reader.Close (); reader = null;
				command.Dispose (); command = null;
				db_connection.Close (); db_connection = null;

				Application.Init (false);

				Shell s = new Shell ();
				Application.Run (s);
			}
		}
	}
}
