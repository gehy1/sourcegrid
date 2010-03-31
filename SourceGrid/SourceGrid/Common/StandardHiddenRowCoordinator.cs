using System;
using System.Collections.Generic;

namespace SourceGrid
{
	public class StandardHiddenRowCoordinator : IHiddenRowCoordinator
	{
		private RowsBase m_rows = null;
		
		public RowsBase Rows {
			get { return m_rows; }
		}
		
		public StandardHiddenRowCoordinator(RowsBase rows)
		{
			this.m_rows = rows;
		}
		
		private int? GetNextVisibleRow(int startFrom)
		{
			var i = startFrom + 1;
			while (i < Rows.Count)
			{
				if (Rows.IsVisible(i) == true)
					return i;
				i++;
			}
			return null;
		}
		
		public int ConvertScrollbarValueToRowIndex(int scrollBarValue)
		{
			var currentRow = 0;
			// lets find first visible row from scroll bar value
			for (var i = 0; i < scrollBarValue; i++)
			{
				var nextRow = GetNextVisibleRow(currentRow);
				if (nextRow == null)
					break;
				currentRow = nextRow.Value;
			}
			return currentRow;
		}
		
		/// <summary>
		/// Returns a sequence of row indexes which are visible only.
		/// Correctly handles invisible rows.
		/// </summary>
		/// <param name="scrollBarValue">The value of the vertical scroll bar. Note that
		/// this does not directly relate to row number. If there are no hidden rows at all,
		/// then scroll bar value directly relates to row index number. However,
		/// if some rows are hidden, then this value is different.
		/// Basically, it says how many visible rows must be scrolled down</param>
		/// <param name="numberOfRowsToProduce">How many visible rows to return</param>
		/// <returns>Can return less rows than requested. This might occur
		/// if you request to return visible rows in the end of the grid,
		/// and all the rows would be hidden. In that case no indexes would be returned
		/// at all, even though specific amount of rows was requested</returns>
		public IEnumerable<int> LoopVisibleRows(int scrollBarValue, int numberOfRowsToProduce)
		{
			var producedRows = 0;
			var currentRow = scrollBarValue;
			if (Rows.IsVisible(scrollBarValue) == false)
			{
				var next = GetNextVisibleRow(currentRow);
				if (next == null)
					yield break;
				currentRow = next.Value;
			}
			
			
			// produce speicifc number of visible row indexes
			while (producedRows <= numberOfRowsToProduce)
			{
				yield return currentRow;
				producedRows++;
				
				var nextRow = GetNextVisibleRow(currentRow);
				if (nextRow == null)
					break;
				currentRow = nextRow.Value;
				
			}
			
		}
		
	}
}
