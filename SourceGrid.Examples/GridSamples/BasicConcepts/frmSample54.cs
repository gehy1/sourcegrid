using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample14.
	/// </summary>
	[Sample("SourceGrid - Basic concepts", 54, "Programmatically edit cell")]
	public class frmSample54 : System.Windows.Forms.Form
	{
		private SourceGrid.Grid grid1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample54()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grid1 = new SourceGrid.Grid();
			this.buttonEditCell = new System.Windows.Forms.Button();
			this.domainUpDownColumnIndex = new System.Windows.Forms.DomainUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// grid1
			// 
			this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.grid1.Location = new System.Drawing.Point(8, 72);
			this.grid1.Name = "grid1";
			this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
			this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
			this.grid1.Size = new System.Drawing.Size(393, 242);
			this.grid1.TabIndex = 0;
			this.grid1.TabStop = true;
			this.grid1.ToolTipText = "";
			// 
			// buttonEditCell
			// 
			this.buttonEditCell.Location = new System.Drawing.Point(83, 10);
			this.buttonEditCell.Name = "buttonEditCell";
			this.buttonEditCell.Size = new System.Drawing.Size(97, 20);
			this.buttonEditCell.TabIndex = 1;
			this.buttonEditCell.Text = "Edit cell";
			this.buttonEditCell.UseVisualStyleBackColor = true;
			// 
			// domainUpDownColumnIndex
			// 
			this.domainUpDownColumnIndex.Location = new System.Drawing.Point(83, 36);
			this.domainUpDownColumnIndex.Name = "domainUpDownColumnIndex";
			this.domainUpDownColumnIndex.Size = new System.Drawing.Size(97, 20);
			this.domainUpDownColumnIndex.TabIndex = 2;
			this.domainUpDownColumnIndex.Text = "column index";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Magic button";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Column index";
			// 
			// frmSample54
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(409, 321);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.domainUpDownColumnIndex);
			this.Controls.Add(this.buttonEditCell);
			this.Controls.Add(this.grid1);
			this.Name = "frmSample54";
			this.Text = "Programatically edit cell";
			this.Load += new System.EventHandler(this.frmSample54_Load);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DomainUpDown domainUpDownColumnIndex;
		private System.Windows.Forms.Button buttonEditCell;
		#endregion

		private void frmSample54_Load(object sender, System.EventArgs e)
		{
			grid1.BorderStyle = BorderStyle.FixedSingle;

			grid1.ColumnsCount = 4;
			grid1.FixedRows = 1;
			grid1.Rows.Insert(0);

			SourceGrid.Cells.Editors.ComboBox cbEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
			cbEditor.StandardValues = new string[]{"Value 1", "Value 2", "Value 3"};
			cbEditor.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.SingleClick | SourceGrid.EditableMode.AnyKey;

			grid1[0, 0] = new SourceGrid.Cells.ColumnHeader("String");
			grid1[0, 1] = new SourceGrid.Cells.ColumnHeader("DateTime");
			grid1[0, 2] = new SourceGrid.Cells.ColumnHeader("CheckBox");
			grid1[0, 3] = new SourceGrid.Cells.ColumnHeader("ComboBox");
			for (int r = 1; r < 10; r++)
			{
				grid1.Rows.Insert(r);
				grid1[r, 0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
				grid1[r, 1] = new SourceGrid.Cells.Cell(DateTime.Today, typeof(DateTime));
				grid1[r, 2] = new SourceGrid.Cells.CheckBox(null, true);
				grid1[r, 3] = new SourceGrid.Cells.Cell("Value 1", cbEditor);
				grid1[r, 3].View = SourceGrid.Cells.Views.ComboBox.Default;
			}

			grid1.AutoSizeCells();
			
			// set up column index button
			for (int i = 0; i < 4; i++)
				domainUpDownColumnIndex.Items.Add(i);
			domainUpDownColumnIndex.SelectedIndex = 0;
			buttonEditCell.Click += delegate { StartEdit(new SourceGrid.Position(1, GetColumnIndex())); };
		}
		
		private int GetColumnIndex()
		{
			try
			{
				return Int32.Parse(domainUpDownColumnIndex.Text);
			}
			catch (FormatException)
			{
				return 1;
			}
		}
		
		public void StartEdit ( SourceGrid.Position Position )
		{
			SourceGrid.CellContext _Context = new SourceGrid.CellContext ( grid1, Position, grid1[ Position.Row, Position.Column ] );
			_Context.StartEdit ( );
		}
	}
}
