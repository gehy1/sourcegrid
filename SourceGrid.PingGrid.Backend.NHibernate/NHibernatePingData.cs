
using System;
using System.Collections;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using SourceGrid.Extensions.PingGrids;

namespace SourceGrid.PingGrid.Backend.NHibernate
{
	public class NHibernatePingData<T> : IPingData where T : class
	{
		ISessionFactory sessionFactory = null;
		
		/// <summary>
		/// Use this to inject some custom performance optimized
		/// property value reader.
		/// </summary>
		public IPropertyResolver PropertyResolver{get;set;}
		
		private int? count = null;
		
		public void Invalidate()
		{
			count = null;
		}
		
		public NHibernatePingData(ISessionFactory factory)
		{
			this.sessionFactory = factory;
			PropertyResolver = ReflectionPropertyResolver.SharedInstance;
		}
		
		
		public int Count {
			get {
				if (count == null)
				{
					using (var session = sessionFactory.OpenSession())
					{
						// populate the database
						using (var transaction = session.BeginTransaction())
						{
							var criteria = session.CreateCriteria(typeof(T))
								.SetProjection(Projections.RowCount());
							var res = criteria.UniqueResult<int>();
							count = res;
						}
					}
				}
				return count.Value;
			}
		}
		
		public bool AllowSort {
			get {
				throw new NotImplementedException("allowSourt");
			}
			set {
				throw new NotImplementedException("allowSourt");
			}
		}
		
		public void ApplySort(string propertyName, bool @ascending)
		{
			throw new NotImplementedException("sort");
		}
		
		
		/// <summary>
		/// Gets value from NHibernate.
		/// 
		/// In fact we do not need whole object here, w
		/// </summary>
		/// <param name="index"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public object GetItemValue(int index, string propertyName)
		{
			using (var session = sessionFactory.OpenStatelessSession())
			{
				// populate the database
				using (var transaction = session.BeginTransaction())
				{
					var criteria = session.CreateCriteria(typeof(T))
						.Add(Restrictions.IdEq(index));
					var obj = criteria.UniqueResult();
					transaction.Commit();
					if (obj == null)
						return null;
					var value = PropertyResolver.ReadValue(obj, propertyName);
					return value;
				}
			}
		}
		
	}
}