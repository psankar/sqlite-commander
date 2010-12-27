using System;
using System.Collections.Generic;
using System.Data;
using Mono.Terminal;
using Mono.Data.SqliteClient;

namespace Autobahn 
{
	public class TablesList : IListProvider 
	{
		public List<string> items = new List<string> ();
		public ListView view;

		public void Render (int line, int col, int width, int item)
		{
			Curses.addstr (items[item]);
		}

		public bool AllowMark 	{
			get {
				return false;
			}
		}

		public int Items {
			get {
				return items.Count;
			}
		}

		public bool IsMarked (int item)
		{
			return false;
		}

		public bool ProcessKey (int ch)
		{
			return false;
		}

		public void SelectedChanged ()
		{
			return;
		}

		public void SetListView (ListView target)
		{
			view = target;	
		}

		public void Add (string s)
		{
			items.Add (s);
		}

	}

	public class RecordsList : IListProvider
	{
		public List<string> items = new List<string> ();
		public ListView view;

		public void Render (int line, int col, int width, int item)
		{
			Curses.addstr (items[item]);
		}

		public bool AllowMark 	{
			get {
				return false;
			}
		}

		public int Items {
			get {
				return items.Count;
			}
		}

		public bool IsMarked (int item)
		{
			return false;
		}

		public bool ProcessKey (int ch)
		{
			return false;
		}

		public void SelectedChanged ()
		{
			return;
		}

		public void SetListView (ListView target)
		{
			view = target;
		}

		public void Add (string s)
		{
			items.Add (s);
		}
	}


	public class Shell : Container 
	{
		public Shell (TablesList tables, RecordsList records) : base (0, 0, Application.Cols, Application.Lines)
		{
			Frame masterFrame = new Frame ("Autobahn - The SQLite Browser");
			Add (masterFrame);
			
			masterFrame.x = 0;
			masterFrame.y = 0;
			masterFrame.w = Application.Cols;
			masterFrame.h = Application.Lines;

			ListView tables_view = new ListView (1, 1, 1, tables.Items, tables);
			masterFrame.Add (tables_view);

			ListView records_view = new ListView (20, 1, 1, records.Items, records);
			masterFrame.Add (records_view);

		}

		static void Main (String [] args) 
		{
			if (args.Length != 1)
				Console.WriteLine ("Insufficient number of parameters");
			else {
				string connectionString;
				string sql;
				IDbCommand command;
				IDbConnection dbConnection;
				IDataReader reader;

				TablesList tables = new TablesList ();

				connectionString = "URI=file:" + args[0];
				dbConnection = (IDbConnection) new SqliteConnection(connectionString);
				dbConnection.Open ();
				command = dbConnection.CreateCommand ();
				sql = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name";
				command.CommandText = sql;
				reader = command.ExecuteReader ();
				while (reader.Read ()) {
					tables.Add (reader.GetString (0));
				}

				RecordsList records = new RecordsList ();
				sql = "SELECT * FROM " + tables.items [0];
				command.CommandText = sql;
				reader = command.ExecuteReader ();
				int n = 0;
				while (reader.Read () && n++ < 30) {
					records.Add (reader.GetString (0));
				}

				// clean up
				reader.Close (); reader = null;
				command.Dispose (); command = null;
				dbConnection.Close (); dbConnection = null;

				Application.Init (false);
				Shell s = new Shell (tables, records);
				Application.Run (s);
			}
		}
	}
}
