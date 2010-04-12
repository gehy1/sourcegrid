
using System;
using System.Windows.Forms;
using FluentNHibernate.Cfg;
using Microsoft.Isam.Esent.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SourceGrid.PingGrid.Backend.Essent;
using SourceGrid.PingGrid.Backend.NHibernate;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	[Sample("SourceGrid - PingGrid", 60, "PingGrid - NHibernate backend with FireBird")]
	public partial class frmSample60 : Form
	{
		string[] customerNames = {"FireBird", "MySQL", "PostGre", "DivanDB", "CouchDB"};
		Random rnd = new Random();
		ISessionFactory sessionFactory = null;
		NHibernatePingData<Track> source = null;
			
		private ISessionFactory CreateSessionFactory()
		{
			var conf = new FluentNHibernate.Cfg.Db.FirebirdConfiguration()
				.ShowSql()
				.ConnectionString(@"
User=SYSDBA;Password=masterkey;Database=..\..\CHINOOK.FDB;
DataSource=localhost; Port=3050;Dialect=3; Charset=UNICODE_FSS;Role=;Connection lifetime=15;Pooling=true;
MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=1;");
			
			return Fluently.Configure()
				.Database(conf)
				.Mappings(m =>
				          m.FluentMappings.AddFromAssemblyOf<frmSample60>())
				.ExposeConfiguration(BuildSchema)
				.BuildSessionFactory();
		}
		
		private static void BuildSchema(Configuration config)
		{
			// delete the existing db on each run
			//if (File.Exists(DbFile))
			//    File.Delete(DbFile);

			// this NHibernate tool takes a configuration (with mapping info in)
			// and exports a database schema from it
			new SchemaExport(config)
				.Create(false, false);
		}
		
		public frmSample60()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
			sessionFactory = CreateSessionFactory();
			source = new NHibernatePingData<Track>(sessionFactory);
			
			
			
			pingGrid1.Columns.Add("Composer", "Composer property", typeof(string));
			pingGrid1.Columns.Add("Name", "Name property", typeof(string));
			
			
			pingGrid1.DataSource = source;
			pingGrid1.Columns.StretchToFit();
			
			pingGrid1.VScrollPositionChanged += delegate { UpdateCount(); };
			toolStripMenuItem1.Click += this.ToolStripMenuItem1Click;
			
			AddRows(1, 10000);
			UpdateCount();
		}

		void UpdateCount()
		{
			UpdateCountInternal(pingGrid1.DataSource.Count);
		}
		
		void UpdateCountInternal(int count)
		{
			if (pingGrid1.Rows.FirstVisibleScrollableRow == null)
				return;
			int row = pingGrid1.Rows.FirstVisibleScrollableRow.Value;
			this.toolStripStatusLabel1.Text = string.Format("Viewing record {0}/ {1}", row, pingGrid1.DataSource.Count);
		}

		private void AddRows(int from, int to)
		{
			var count = pingGrid1.DataSource.Count;
			if (count > to )
				return;
			using (var session = sessionFactory.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					for (var i = from; i < to; i++)
					{
						// create a couple of Stores each with some Products and Employees
						var track = new Track{ Name = "SuperMart", Composer = "random composer" };
						session.Save(track);
						
						if (i % 100 == 0)
						{
							session.Flush();
							session.Clear();
						}
					}
					transaction.Commit();

				}
			}
			source.Invalidate();
		}
		
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			var max = pingGrid1.DataSource.Count + 1;
			AddRows(max, max + 200000);
			UpdateCount();
		}
	}
	

}
