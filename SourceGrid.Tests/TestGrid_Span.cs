/* */

using System;
using NUnit.Framework;
using SourceGrid.Cells;

namespace SourceGrid.Tests
{
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
