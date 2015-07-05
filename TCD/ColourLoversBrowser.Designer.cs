/*
 * Date: 21.12.2009
 * Time: 2:31
 */
namespace TCD
{
	partial class ColourLoversBrowser
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColourLoversBrowser));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.throbberBar = new System.Windows.Forms.ToolStripProgressBar();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.searchTypeComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.searchKeywordsBox = new System.Windows.Forms.ToolStripTextBox();
			this.searchButton = new System.Windows.Forms.ToolStripButton();
			this.resultListBox = new System.Windows.Forms.ListBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.applyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.visitPaletteURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.throbberBar,
									this.statusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 525);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(472, 22);
			this.statusStrip1.SizingGrip = false;
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// throbberBar
			// 
			this.throbberBar.Enabled = false;
			this.throbberBar.Name = "throbberBar";
			this.throbberBar.Size = new System.Drawing.Size(150, 16);
			this.throbberBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			// 
			// statusLabel
			// 
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(77, 17);
			this.statusLabel.Text = "ColourLovers";
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.searchTypeComboBox,
									this.toolStripSeparator1,
									this.toolStripLabel1,
									this.searchKeywordsBox,
									this.searchButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(472, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// searchTypeComboBox
			// 
			this.searchTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.searchTypeComboBox.DropDownWidth = 80;
			this.searchTypeComboBox.Items.AddRange(new object[] {
									"Search",
									"New",
									"Top",
									"Random"});
			this.searchTypeComboBox.Name = "searchTypeComboBox";
			this.searchTypeComboBox.Size = new System.Drawing.Size(80, 25);
			this.searchTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.SearchTypeComboBoxSelectedIndexChanged);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(61, 22);
			this.toolStripLabel1.Text = "Keywords:";
			// 
			// searchKeywordsBox
			// 
			this.searchKeywordsBox.Name = "searchKeywordsBox";
			this.searchKeywordsBox.Size = new System.Drawing.Size(100, 25);
			// 
			// searchButton
			// 
			this.searchButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.searchButton.Image = ((System.Drawing.Image)(resources.GetObject("searchButton.Image")));
			this.searchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(62, 22);
			this.searchButton.Text = "Search";
			this.searchButton.Click += new System.EventHandler(this.SearchButtonClick);
			// 
			// resultListBox
			// 
			this.resultListBox.ContextMenuStrip = this.contextMenuStrip1;
			this.resultListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resultListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.resultListBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.resultListBox.FormattingEnabled = true;
			this.resultListBox.IntegralHeight = false;
			this.resultListBox.ItemHeight = 65;
			this.resultListBox.Location = new System.Drawing.Point(0, 25);
			this.resultListBox.Name = "resultListBox";
			this.resultListBox.ScrollAlwaysVisible = true;
			this.resultListBox.Size = new System.Drawing.Size(472, 500);
			this.resultListBox.TabIndex = 2;
			this.resultListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ResultListBoxDrawItem);
			this.resultListBox.Resize += new System.EventHandler(this.ResultListBoxResize);
			this.resultListBox.DoubleClick += new System.EventHandler(this.ApplyToolStripMenuItemClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.applyToolStripMenuItem,
									this.visitPaletteURLToolStripMenuItem,
									this.copyColorsToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(160, 70);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1Opening);
			// 
			// applyToolStripMenuItem
			// 
			this.applyToolStripMenuItem.Name = "applyToolStripMenuItem";
			this.applyToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.applyToolStripMenuItem.Text = "Apply";
			this.applyToolStripMenuItem.Click += new System.EventHandler(this.ApplyToolStripMenuItemClick);
			// 
			// visitPaletteURLToolStripMenuItem
			// 
			this.visitPaletteURLToolStripMenuItem.Name = "visitPaletteURLToolStripMenuItem";
			this.visitPaletteURLToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.visitPaletteURLToolStripMenuItem.Text = "Visit Palette URL";
			this.visitPaletteURLToolStripMenuItem.Click += new System.EventHandler(this.VisitPaletteURLToolStripMenuItemClick);
			// 
			// copyColorsToolStripMenuItem
			// 
			this.copyColorsToolStripMenuItem.Name = "copyColorsToolStripMenuItem";
			this.copyColorsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.copyColorsToolStripMenuItem.Text = "Copy Colors";
			this.copyColorsToolStripMenuItem.Click += new System.EventHandler(this.CopyColorsToolStripMenuItemClick);
			// 
			// ColourLoversBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(472, 547);
			this.Controls.Add(this.resultListBox);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ColourLoversBrowser";
			this.Text = "COLOURlovers Browser";
			this.Shown += new System.EventHandler(this.ColourLoversBrowserLoad);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripMenuItem applyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyColorsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem visitPaletteURLToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ListBox resultListBox;
		private System.Windows.Forms.ToolStripProgressBar throbberBar;
		private System.Windows.Forms.ToolStripTextBox searchKeywordsBox;
		private System.Windows.Forms.ToolStripButton searchButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripComboBox searchTypeComboBox;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel;
		private System.Windows.Forms.StatusStrip statusStrip1;
	}
}
