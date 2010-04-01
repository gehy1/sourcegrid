using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SourceGrid.Extensions.PingGrids
{
	public class EmptyPingSource : IPingData
	{
		public object GetItemValue(int index, string propertyName)
		{
			throw new NotImplementedException();
		}
		
		public int Count {
			get {
			return 0; 
			}
			set {
			}
		}
		
		public bool AllowSort {
			get {
				return false;
			}
			set {
			}
		}
		
		public void ApplySort(string propertyName, bool @ascending)
		{
			throw new NotImplementedException();
		}
	}
}
