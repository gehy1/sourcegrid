
namespace WindowsFormsSample.GridSamples.PingGrids
{
	partial class frmSample59
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			SourceGrid.Extensions.PingGrids.EmptyPingSource emptyPingSource1 = new SourceGrid.Extensions.PingGrids.EmptyPingSource();
			this.pingGrid1 = new SourceGrid.Extensions.PingGrids.PingGrid();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pingGrid1
			// 
			emptyPingSource1.AllowSort = false;
			emptyPingSource1.Count = 0;
			this.pingGrid1.DataSource = emptyPingSource1;
			this.pingGrid1.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
			this.pingGrid1.EnableSort = true;
			this.pingGrid1.FixedRows = 1;
			this.pingGrid1.Location = new System.Drawing.Point(12, 27);
			this.pingGrid1.Name = "pingGrid1";
			this.pingGrid1.SelectionMode = SourceGrid.GridSelectionMode.Row;
			this.pingGrid1.Size = new System.Drawing.Size(268, 203);
			this.pingGrid1.TabIndex = 0;
			this.pingGrid1.TabStop = true;
			this.pingGrid1.ToolTipText = "";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 244);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(292, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// frmSample59
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.pingGrid1);
			this.Name = "frmSample59";
			this.Text = "frmSample59";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private SourceGrid.Extensions.PingGrids.PingGrid pingGrid1;
	}
}
