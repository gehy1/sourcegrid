using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SourceGrid.Cells;
using SourceGrid.Exporter;

namespace SourceGrid.Tests.Exporter
{
    [TestFixture]
    class CsvExporterTest
    {
        private CsvExporter underTest;

        [SetUp]
        public void SetUp()
        {
            underTest = new CsvExporter();
        }

        [Test]
        public void Should_Return_Empty_Given_Empty_Grid()
        {
            // Given
            var grid = NewEmptyGrid();
            var stream = new StringWriter();

            // When
            underTest.Export(grid, stream);

            // Then
            Assert.AreEqual(string.Empty, stream.ToString());
        }

        [Test]
        public void Should_NotEnd_With_NewLine_Given_Cell()
        {
            // Given
            const string cellValue = @"Text";
            var grid = SingeCellGrid(1, 1, cellValue);
            var stream = new StringWriter();

            // When
            underTest.Export(grid, stream);

            // Then
            Assert.AreEqual(cellValue, stream.ToString());
        }

        [Test]
        public void Should_Escape_Given_Cell_With_NewLines()
        {
            // Given
            var grid = SingeCellGrid(1, 1, "Multi \r\n line text");
            var stream = new StringWriter();

            // When
            underTest.Export(grid, stream);

            // Then
            Assert.AreEqual("\"Multi \r\n line text\"", stream.ToString());
        }

        [Test]
        public void Should_Escape_Given_Cell_With_Comma()
        {
            // Given
            var grid = SingeCellGrid(1, 1, "Text , comma");
            var stream = new StringWriter();

            // When
            underTest.Export(grid, stream);

            // Then
            Assert.AreEqual("\"Text , comma\"", stream.ToString());
        }

        [Test]
        public void Should_Escape_Given_Cell_With_QuotationMarks()
        {
            // Given
            var grid = SingeCellGrid(1, 1, "Text \" quote \" ");
            var stream = new StringWriter();

            // When
            underTest.Export(grid, stream);

            // Then
            Assert.AreEqual("\"Text \"\" quote \"\" \"", stream.ToString());
        }

        private static Grid SingeCellGrid(int rowCount, int columnCount, string cellValue)
        {
            var grid = NewGrid(rowCount, columnCount);
            grid[0, 0] = new SourceGrid.Cells.Cell(cellValue);
            return grid;
        }

        private static Grid NewGrid(int rowCount, int columnCount)
        {
            var grid = new Grid {ColumnsCount = columnCount};
            grid.Rows.InsertRange(0, rowCount);
            return grid;
        }

        private static GridVirtual NewEmptyGrid()
        {
            return new Grid();
        }
    }
}
