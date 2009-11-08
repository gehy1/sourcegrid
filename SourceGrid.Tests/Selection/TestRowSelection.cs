/*
 * Created by SharpDevelop.
 * User: darius.damalakas
 * Date: 2008-12-10
 * Time: 17:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;
using SourceGrid;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using SourceGrid.Selection;

namespace SourceGrid.Tests.Selection
{

	
	[TestFixture]
	public class TestRowSelection
	{
		[Test]
		public void GetSelectionRegion_MergeRanges()
		{
			// select two ranges:
			// * from row 1 to 100
			// * from row 501 to 600
			//
			// Assert that grid.Selection.GetSelectionRegion() returns
			// only 2 ranges, not 200
			
			Grid grid = new TestRowSelectionHelper().CreateGridWithRows(30);
	
			grid.Selection.SelectRange(new Range(1, 0, 1, 0), true);
			grid.Selection.SelectRange(new Range(2, 0, 2, 0), true);
			
			grid.Selection.SelectRange(new Range(20, 0, 20, 0), true);
			grid.Selection.SelectRange(new Range(21, 0, 21, 0), true);
			
			RangeRegion region = grid.Selection.GetSelectionRegion();
			Assert.AreEqual(2, region.Count);
			Assert.AreEqual(new Range(1, 0, 2, 0), region[0]);
			Assert.AreEqual(new Range(20, 0, 21, 0), region[1]);
		}
		
		[Test]
		public void GetSelectionRegion_ReturnsContiguousRanges()
		{
			// select two ranges:
			// * from row 1 to 100
			// * from row 501 to 600
			//
			// Assert that grid.Selection.GetSelectionRegion() returns
			// only 2 ranges, not 200
			
			Grid grid = new TestRowSelectionHelper().CreateGridWithRows(30);
	
			grid.Selection.SelectRange(new Range(1, 0, 2, 0), true);
			grid.Selection.SelectRange(new Range(21, 0, 22, 0), true);
			
			RangeRegion region = grid.Selection.GetSelectionRegion();
			Assert.AreEqual(2, region.Count);
			Assert.AreEqual(new Range(1, 0, 2, 0), region[0]);
			Assert.AreEqual(new Range(21, 0, 22, 0), region[1]);
		}
		
		[Test]
		public void UnselectRegion()
		{
			// select two ranges:
			// * from row 1 to 10
			// * from row 21 to 30
			//
			// Assert that grid.Selection.GetSelectionRegion().GetRowsIndex();
			// returns correct row indexes
			
			Grid grid = new TestRowSelectionHelper().CreateGridWithRows(2000);
	
			grid.Selection.SelectRange(new Range(1, 0, 3, 0), true);
			Assert.AreEqual(true, grid.Selection.IsSelectedRow(2));
			grid.Selection.SelectRange(new Range(2, 0, 2, 0), false);
			Assert.AreEqual(false, grid.Selection.IsSelectedRow(2));
			
		}
		
		[Test]
		public void GetSelectionRegion_GetRowsIndex_SucessScenario()
		{
			// select two ranges:
			// * from row 1 to 10
			// * from row 21 to 30
			//
			// Assert that grid.Selection.GetSelectionRegion().GetRowsIndex();
			// returns correct row indexes
			
			Grid grid = new TestRowSelectionHelper().CreateGridWithRows(2000);
	
			grid.Selection.SelectRange(new Range(1, 0, 10, 0), true);
			grid.Selection.SelectRange(new Range(21, 0, 30, 0), true);
			
			int[] indexes = grid.Selection.GetSelectionRegion().GetRowsIndex();
			Assert.AreEqual(20, indexes.Length);
			
			for (int i = 0; i < 10; i++)
			{
				Assert.AreEqual(i + 1, indexes[i]);
			}
			
			for (int i = 0; i < 10; i++)
			{
				Assert.AreEqual(20 + i + 1, indexes[i + 10]);
			}
		}
	}
}
