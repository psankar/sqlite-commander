using System;
using System.Collections.Generic;
using System.Collections;
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
		public List<IEnumerable	> items = new List<IEnumerable> ();
		public ListView view;
		
		static int MAX_CHARS_PER_COLUMN = 30;

		public void Render (int line, int col, int width, int item)
		{
			string record = "";
			List<string> l = (List<string>) items[item];
			
			foreach (string column in l) {
				if (column.Length > MAX_CHARS_PER_COLUMN)
					record = record + column.Substring (0, MAX_CHARS_PER_COLUMN) + "... ";
				else
					record = record + column + " ";
			}
			Curses.addstr (record);
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

		public void Add (IEnumerable s)
		{
			items.Add (s);
		}
	}


	public class Shell : Container 
	{
		IDbCommand command;
		IDbConnection dbConnection;
		IDataReader reader;

		public Shell (string filename) : base (0, 0, Application.Cols, Application.Lines)
		{
			string connectionString;
			string sql;
			TablesList tables = new TablesList ();

			connectionString = "URI=file:" + filename;
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
			int n = 0, col_count;
			while (reader.Read () && n++ < 30) {
				List <string> record;
				record = new List<string> ();

				Console.WriteLine ("Number of the columns in the records " + reader.FieldCount);
				for (col_count = 0; col_count < reader.FieldCount; ++col_count) {
					try {
						Console.WriteLine (col_count + "Adding column " + reader.GetString (col_count));
						record.Add (reader.GetString (col_count));
					} catch (System.NullReferenceException ex){
						record.Add ("???");
					}
				}
				records.Add (record);
			}

			/* =========================================== */
			Frame tablesFrame = new Frame ("Tables");
			Add (tablesFrame);

			tablesFrame.x = 0;
			tablesFrame.y = 0;

			int TABLES_WIDTH = 25;

			tablesFrame.w = TABLES_WIDTH;
			tablesFrame.h = Application.Lines;

			ListView tables_view = new ListView (1, 1, 1, tables.Items, tables);
			tablesFrame.Add (tables_view);

			Frame recordsFrame = new Frame ("Records");
			recordsFrame.x = TABLES_WIDTH;
			recordsFrame.y = 0;
			recordsFrame.w = Application.Cols - TABLES_WIDTH - 1;
			recordsFrame.h = Application.Lines;

			Add (recordsFrame);

			ListView records_view = new ListView (1, 1, 1, records.Items, records);
			recordsFrame.Add (records_view);
		}

		~Shell ()
		{
			// clean up
			reader.Close (); reader = null;
			command.Dispose (); command = null;
			dbConnection.Close (); dbConnection = null;
		}

		static void Main (String [] args) 
		{
			if (args.Length != 1)
				Console.WriteLine ("Insufficient number of parameters");
			else {
				Application.Init (false);
				Shell s = new Shell (args [0]);
				Application.Run (s);
			}
		}
	}
}
