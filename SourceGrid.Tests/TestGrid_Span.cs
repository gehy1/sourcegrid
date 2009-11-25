/* */

using System;
using NUnit.Framework;
using SourceGrid.Cells;

namespace SourceGrid.Tests
{
	
	[TestFixture]
	public class TestValueCellComparer
	{
		[Test]
		public void Bug_3424()
		{
			// if compared values are incompatible,
			// return -1
			Assert.AreEqual(-1, new ValueCellComparer()
			                .Compare(true, DateTime.Now));
		}
	}
	
	[TestFixture]
	public class TestGrid_Sorting
	{
		[Test]
		public void Bug_3424()
		{
			var grid1 = new Grid();
			grid1.ColumnsCount = 3;
			grid1.FixedRows = 1;
			grid1.Rows.Insert(0);

			SourceGrid.Cells.ColumnHeader header1 = new SourceGrid.Cells.ColumnHeader("String");

			// here you can se the other column to sort when the current column is equal
			header1.SortComparer = new SourceGrid.MultiColumnsComparer(1, 2);

			var sorter = new SourceGrid.Cells.ColumnHeader("CheckBox");
			grid1[0, 0] = header1;
			grid1[0, 1] = new SourceGrid.Cells.ColumnHeader("DateTime");
			grid1[0, 2] = sorter;
			for (int r = 1; r < 10; r++)
			{
				grid1.Rows.Insert(r);
				grid1[r, 0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
				grid1[r, 1] = new SourceGrid.Cells.Cell(DateTime.Today.AddDays(7 * r), typeof(DateTime));
				grid1[r, 2] = new SourceGrid.Cells.CheckBox("", true);
			}
			
			grid1[9, 2] = null;
			grid1[9, 1].ColumnSpan = 2;
			
			sorter.Sort(true);
		}
	}
	
	
	[TestFixture]
	public class TestGrid_Span
	{
		[Test]
		public void AttachedExistingSpannedCell()
		{
			Grid grid1 = new Grid();
			grid1.Redim(1, 4);

			var cell = new SourceGrid.Cells.Cell();
			cell.ColumnSpan = 5;
			
			grid1[0, 1] = new SourceGrid.Cells.Cell();
			
			var b = false;
			// this line should throw exception
			try
			{
				grid1[0, 0] = cell;
			}
			catch (OverlappingCellException)
			{
				b = true;
			}
			
			Assert.AreEqual(b, true);
		}
		
		[Test]
		public void AttachedExistingSpannedCell_AddCellLater()
		{
			Grid grid1 = new Grid();
			grid1.Redim(1, 40);

			var cell = new SourceGrid.Cells.Cell();
			cell.ColumnSpan = 5;
			
			// this line should not throw exception, since everything is ok
			grid1[0, 0] = cell;
			
			// this line should throw exception, as there is a spanned cell
			// already
			var b = false;
			try
			{
				grid1[0, 1] = new SourceGrid.Cells.Cell();
			}
			catch (OverlappingCellException)
			{
				b = true;
			}
			
			Assert.AreEqual(b, true);
			
		}
		
		[Test]
		public void Bug0003()
		{
			var grid = new Grid();
			int rowCount = 10, colCount = 10;
			grid.Redim(rowCount, colCount);

			grid[0, 0] = new SourceGrid.Cells.Cell();
			grid[0, 0].ColumnSpan = 3;

			grid.Rows.Insert(0);
			grid[0, 0] = new SourceGrid.Cells.Cell();
			grid[0, 0].ColumnSpan = 3;
		}
		
		[Test]
		public void Bug0002()
		{
			// the last call to change rowspan to 3 throws
			// exception
			// Could not find a spanned cell range with the same starting point as 0;3
			Grid grid1 = new Grid();
			grid1.Redim(1, 4);

			//grid1[0, 0] = new SourceGrid.Cells.Cell();
			//grid1[0, 0].ColumnSpan = 3;
			//grid1[0, 0].RowSpan = 3;

			grid1[0, 3] = new SourceGrid.Cells.Cell();
			grid1[0, 3].ColumnSpan = 3;
			grid1[0, 3].RowSpan = 3;
			
			// the bug was that there was not correctly
			// quadTreeNode.IsEmpty property defined
		}
		
		
		[Test]
		public void Bug0001()
		{
			var grid1 = new Grid();
			grid1.Redim(4, 12);
			grid1.FixedRows = 2;
			
			grid1[0, 3] = new Cell("5 Column Header");
			grid1[0, 3].ColumnSpan = 5;

			//2 Header Row
			grid1[1, 0] = new Cell("1");
			grid1[1, 1] = new Cell("2");
			
			var cell = new SourceGrid.Cells.CheckBox("CheckBox Column/Row Span", false);
			grid1[2, 2] = cell;
			grid1[2, 2].ColumnSpan = 2;
			grid1[2, 2].RowSpan = 2;
			
			// test that all cells point to the same cell
			Assert.AreEqual(cell, grid1.GetCell(2, 3));
			Assert.AreEqual(cell, grid1.GetCell(3, 2));
			Assert.AreEqual(cell, grid1.GetCell(3, 3));
		}
		
		[Test]
		public void PositionToCellRange_OutOfBounds()
		{
			Grid grid1 = new Grid();
			grid1.Redim(0, 0);
			Assert.AreEqual(Range.Empty, grid1.PositionToCellRange(new Position(0, 0)));
		}
		
		[Test]
		public void IncreaseRowSpan()
		{
			// when we delete row, if the given row
			// is in the range of some cell spann,
			// then throw an exception
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);

			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 2;
			
			grid1[1, 0].RowSpan = 3;
			
			Assert.AreEqual(1, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			Assert.AreEqual(new Range(1, 0, 3, 0), grid1.SpannedCellReferences.SpannedRangesCollection.ToArray()[0]);

		}
		
		[Test]
		public void DecreaseRowSpan()
		{
			// when we delete row, if the given row
			// is in the range of some cell spann,
			// then throw an exception
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);

			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 3;
			
			grid1.Rows.Remove(2);
			
			Assert.AreEqual(1, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			Assert.AreEqual(new Range(1, 0, 2, 0), grid1.SpannedCellReferences.SpannedRangesCollection.ToArray()[0]);
			Assert.AreEqual(2, grid1[1, 0].RowSpan);

		}
		
		[Test]
		public void DecreaseRowSpan_To1_ShouldRemoveIt()
		{
			// when we delete row, if the given row
			// is in the range of some cell spann,
			// then throw an exception
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);

			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 2;
			
			grid1.Rows.Remove(2);
			
			Assert.AreEqual(0, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			Assert.AreEqual(1, grid1[1, 0].RowSpan);

		}
		
		[Test]
		public void CorrectSpannedCellRefShouldBeAdded()
		{
			// when we delete row, if the given row
			// is in the range of some cell spann,
			// then throw an exception
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);

			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 2;

			Assert.AreEqual(1, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			Assert.AreEqual(new Range(1, 0, 2, 0), grid1.SpannedCellReferences.SpannedRangesCollection.ToArray()[0]);
		}
		
		[Test]
		public void RemoveRowsShouldUpdateCellReferences()
		{
			// when we delete row, spanned row cells should update,
			// for rows, which are below our row
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);

			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].ColumnSpan = 2;

			// remove row
			grid1.Rows.Remove(0);
			
			// add again cell at row 1
			// this should not throw exception,
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].ColumnSpan = 2;
		}
		
		[Test]
		public void PositionToCellRange()
		{
			Grid grid1 = new Grid();
			grid1.Redim(6, 6);
			
			grid1[0, 0] = new SourceGrid.Cells.Cell();
			grid1[0, 0].ColumnSpan = 3;
			grid1[0, 0].RowSpan = 3;
			
			Assert.AreEqual(new Range(0, 0, 2, 2), grid1.PositionToCellRange(new Position(0, 0)));
		}
		
		[Test]
		public void RangeToCellRange()
		{
			Grid grid1 = new Grid();
			grid1.Redim(6, 6);
			
			grid1[0, 0] = new SourceGrid.Cells.Cell();
			grid1[0, 0].ColumnSpan = 3;
			grid1[0, 0].RowSpan = 3;
			
			grid1[0, 3] = new SourceGrid.Cells.Cell();
			grid1[0, 3].ColumnSpan = 3;
			grid1[0, 3].RowSpan = 3;
			
			Assert.AreEqual(new Range(0, 0, 2, 5), grid1.RangeToCellRange(new Range(0, 0, 0, 3)));
		}
	}
}
