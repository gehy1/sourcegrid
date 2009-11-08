using System;

namespace SourceGrid.SourceGrid4.SourceGrid.Utils
{
	public interface IPerformanceCounter : IDisposable
	{
		double GetSeconds();
		double GetMilisec();
	}
	
	public class PerformanceCounter : IDisposable, IPerformanceCounter
	{
		private DateTime m_start = DateTime.MinValue;
		
		public PerformanceCounter()
		{
			this.m_start = DateTime.Now;
		}
		
		public double GetSeconds()
		{
			TimeSpan span = DateTime.Now - m_start;
			return span.TotalSeconds;
		}
		
		public double GetMilisec()
		{
			TimeSpan span = DateTime.Now - m_start;
			return span.TotalMilliseconds;
		}
		
		public void Dispose()
		{
		}
	}
}
