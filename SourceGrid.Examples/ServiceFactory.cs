using System;
using Castle.Windsor;

namespace WindowsFormsSample
{
	public class ServiceFactory
	{
		private static WindsorContainer m_container = null;
		
		public static WindsorContainer Container {
			get { return m_container; }
			set { m_container = value; }
		}
		
		public static T GetService<T>()
		{
			return m_container.Resolve<T>();
		}
		
	}
}
