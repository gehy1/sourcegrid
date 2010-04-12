
using System;
using System.Windows.Forms;
using FluentNHibernate.Cfg;
using Microsoft.Isam.Esent.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SourceGrid.Examples.Threading;
using SourceGrid.PingGrid.Backend.Essent;
using SourceGrid.PingGrid.Backend.NHibernate;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	[Sample("SourceGrid - PingGrid", 60, "PingGrid - NHibernate backend with FireBird")]
	public partial class frmSample60 : Form
	{
		string[] customerNames = {"FireBird", "MySQL", "PostGre", "DivanDB", "CouchDB"};
		Random rnd = new Random();
		NHibernatePingData<Track> source = null;
		
		public SessionFactoryManager SessionFactoryManager{get;set;}
		public FirebirdEmbeddedPreparer FirebirdEmbeddedPreparer {get;set;}
		public EmptyFirebirdDatabasePreparer EmptyFirebirdDatabasePreparer {get;set;}
		
		
		
		public frmSample60()
		{
			
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			ServiceFactory.Init(this);
			
			FirebirdEmbeddedPreparer.EnsureFirebirdReady();
			if (EmptyFirebirdDatabasePreparer.ExistsFile() == false)
			{
				var res = MessageBox.Show("FireBird databse not yet created. Do you want to create it now?",
				                          "Db not exists",
				                          MessageBoxButtons.YesNo,
				                          MessageBoxIcon.Question);
				if (res == DialogResult.No)
				{
					this.Close();
					return;
				}
				EmptyFirebirdDatabasePreparer.Copy();
				SessionFactoryManager.Export = true;
			}
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			SessionFactoryManager.CreateSessionFactory();

			source = new NHibernatePingData<Track>(SessionFactoryManager.SessionFactory);
			
			
			
			pingGrid1.Columns.Add("Composer", "Composer property", typeof(string));
			pingGrid1.Columns.Add("Name", "Name property", typeof(string));
			
			
			pingGrid1.DataSource = source;
			pingGrid1.Columns.StretchToFit();
			
			pingGrid1.VScrollPositionChanged += delegate { UpdateCount(); };
			toolStripMenuItem1.Click += this.ToolStripMenuItem1Click;
			
			//AddRows(1, 10000);
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

		
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			var max = pingGrid1.DataSource.Count + 1;
			var count = pingGrid1.DataSource.Count;
			if (count > max )
				return;
			new ExecuteWithProgressBar(new InsertRowsOperation(this, max, max + 5000), new ProgressBarForm()).Run();
			
			UpdateCount();
		}
	}
	

}
