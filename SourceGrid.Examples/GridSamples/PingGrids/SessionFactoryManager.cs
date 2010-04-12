
using System;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	public class SessionFactoryManager
	{
		/// <summary>
		/// Set to true to automatically create DB schema
		/// upon connecting
		/// </summary>
		public bool Export {get;set;}
		private ISessionFactory m_factory = null;
		
		public ISessionFactory SessionFactory {
			get { return m_factory; }
		}
		
		public ISessionFactory CreateSessionFactory()
		{
			var conf = new FluentNHibernate.Cfg.Db.FirebirdConfiguration()
				.ShowSql()
				.ConnectionString(@"
	User=SYSDBA;Password=masterkey;Database=frmSample60.fdb;
	DataSource=localhost; Port=3050;Dialect=3; Charset=UNICODE_FSS;Role=;Connection lifetime=15;Pooling=true;
	MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=1;");
			
			m_factory = Fluently.Configure()
				.Database(conf)
				.Mappings(m =>
				          m.FluentMappings.AddFromAssemblyOf<frmSample60>())
				.ExposeConfiguration(BuildSchema)
				.BuildSessionFactory();
			return m_factory;
		}
		
		private void BuildSchema(Configuration config)
		{
			// delete the existing db on each run
			//if (File.Exists(DbFile))
			//    File.Delete(DbFile);
	
			// this NHibernate tool takes a configuration (with mapping info in)
			// and exports a database schema from it
			new SchemaExport(config)
				.Create(false, Export);
		}
	}
}
