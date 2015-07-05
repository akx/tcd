/*
 * Date: 7.7.2010
 * Time: 19:47
 */
namespace TCD
{
	partial class MainForm
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.decimalFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.colorPickerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.colorPreviewPanel = new System.Windows.Forms.Panel();
			this.blurBox = new System.Windows.Forms.ComboBox();
			this.lastColorsBox = new System.Windows.Forms.PictureBox();
			this.paletteContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.palCopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.palReplaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.palRemoveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.palCopyAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.palClearMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.palOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.palSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.colourLoversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.tableLayoutPanel1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lastColorsBox)).BeginInit();
			this.paletteContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.tableLayoutPanel1.Controls.Add(this.listBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lastColorsBox, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(187, 179);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// listBox1
			// 
			this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.IntegralHeight = false;
			this.listBox1.ItemHeight = 11;
			this.listBox1.Items.AddRange(new object[] {
									"Hold SHIFT+CTRL",
									"to capture color",
									"",
									"Double-click items",
									"here to copy;",
									"right-click for options"});
			this.listBox1.Location = new System.Drawing.Point(0, 0);
			this.listBox1.Margin = new System.Windows.Forms.Padding(0);
			this.listBox1.Name = "listBox1";
			this.tableLayoutPanel1.SetRowSpan(this.listBox1, 2);
			this.listBox1.Size = new System.Drawing.Size(123, 128);
			this.listBox1.TabIndex = 1;
			this.listBox1.DoubleClick += new System.EventHandler(this.ListBox1DoubleClick);
			this.listBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBox1MouseDown);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.BackColor = System.Drawing.Color.White;
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.copyToolStripMenuItem,
									this.pasteToolStripMenuItem,
									this.toolStripSeparator1,
									this.colorPickerToolStripMenuItem,
									this.toolStripSeparator3,
									this.decimalFormatToolStripMenuItem,
									this.alwaysOnTopToolStripMenuItem});
			this.contextMenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.contextMenuStrip1.ShowCheckMargin = true;
			this.contextMenuStrip1.ShowImageMargin = false;
			this.contextMenuStrip1.Size = new System.Drawing.Size(190, 148);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1Opening);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItemClick);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.PasteToolStripMenuItemClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(186, 6);
			// 
			// decimalFormatToolStripMenuItem
			// 
			this.decimalFormatToolStripMenuItem.CheckOnClick = true;
			this.decimalFormatToolStripMenuItem.Name = "decimalFormatToolStripMenuItem";
			this.decimalFormatToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.decimalFormatToolStripMenuItem.Text = "100% Decimal Format";
			this.decimalFormatToolStripMenuItem.CheckedChanged += new System.EventHandler(this.DecimalFormatToolStripMenuItemCheckedChanged);
			// 
			// colorPickerToolStripMenuItem
			// 
			this.colorPickerToolStripMenuItem.Name = "colorPickerToolStripMenuItem";
			this.colorPickerToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.colorPickerToolStripMenuItem.Text = "Color Picker";
			this.colorPickerToolStripMenuItem.Click += new System.EventHandler(this.ColorPickerToolStripMenuItemClick);
			// 
			// alwaysOnTopToolStripMenuItem
			// 
			this.alwaysOnTopToolStripMenuItem.CheckOnClick = true;
			this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
			this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.alwaysOnTopToolStripMenuItem.Text = "Always On Top";
			this.alwaysOnTopToolStripMenuItem.CheckedChanged += new System.EventHandler(this.AlwaysOnTopToolStripMenuItemCheckedChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Cross;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Location = new System.Drawing.Point(123, 0);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(64, 64);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1MouseMove);
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1MouseDown);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox1MouseUp);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.colorPreviewPanel);
			this.panel1.Controls.Add(this.blurBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(123, 64);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(64, 64);
			this.panel1.TabIndex = 4;
			// 
			// colorPreviewPanel
			// 
			this.colorPreviewPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.colorPreviewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.colorPreviewPanel.Location = new System.Drawing.Point(0, 19);
			this.colorPreviewPanel.Name = "colorPreviewPanel";
			this.colorPreviewPanel.Size = new System.Drawing.Size(64, 45);
			this.colorPreviewPanel.TabIndex = 6;
			this.colorPreviewPanel.Click += new System.EventHandler(this.ColorPreviewPanelDoubleClick);
			// 
			// blurBox
			// 
			this.blurBox.BackColor = System.Drawing.SystemColors.Control;
			this.blurBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.blurBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.blurBox.FormattingEnabled = true;
			this.blurBox.Items.AddRange(new object[] {
									"Precise",
									"Blurry",
									"Blurrier",
									"Blurriest"});
			this.blurBox.Location = new System.Drawing.Point(0, 0);
			this.blurBox.Name = "blurBox";
			this.blurBox.Size = new System.Drawing.Size(64, 19);
			this.blurBox.TabIndex = 5;
			// 
			// lastColorsBox
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.lastColorsBox, 2);
			this.lastColorsBox.ContextMenuStrip = this.paletteContextMenu;
			this.lastColorsBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lastColorsBox.Location = new System.Drawing.Point(0, 128);
			this.lastColorsBox.Margin = new System.Windows.Forms.Padding(0);
			this.lastColorsBox.Name = "lastColorsBox";
			this.lastColorsBox.Size = new System.Drawing.Size(187, 51);
			this.lastColorsBox.TabIndex = 5;
			this.lastColorsBox.TabStop = false;
			this.lastColorsBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lastColorsBoxMouseMove);
			this.lastColorsBox.Click += new System.EventHandler(this.lastColorsBoxClick);
			// 
			// paletteContextMenu
			// 
			this.paletteContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.palCopyMenuItem,
									this.palReplaceMenuItem,
									this.palRemoveMenuItem,
									this.toolStripSeparator2,
									this.palCopyAllMenuItem,
									this.palClearMenuItem,
									this.palOpenMenuItem,
									this.palSaveMenuItem,
									this.colourLoversToolStripMenuItem});
			this.paletteContextMenu.Name = "paletteContextMenu";
			this.paletteContextMenu.ShowImageMargin = false;
			this.paletteContextMenu.Size = new System.Drawing.Size(137, 186);
			this.paletteContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.PaletteContextMenuOpening);
			// 
			// palCopyMenuItem
			// 
			this.palCopyMenuItem.Name = "palCopyMenuItem";
			this.palCopyMenuItem.Size = new System.Drawing.Size(136, 22);
			this.palCopyMenuItem.Text = "Copy";
			// 
			// palReplaceMenuItem
			// 
			this.palReplaceMenuItem.Name = "palReplaceMenuItem";
			this.palReplaceMenuItem.Size = new System.Drawing.Size(136, 22);
			this.palReplaceMenuItem.Text = "Replace";
			this.palReplaceMenuItem.Click += new System.EventHandler(this.PalReplaceMenuItemClick);
			// 
			// palRemoveMenuItem
			// 
			this.palRemoveMenuItem.Name = "palRemoveMenuItem";
			this.palRemoveMenuItem.Size = new System.Drawing.Size(136, 22);
			this.palRemoveMenuItem.Text = "Remove";
			this.palRemoveMenuItem.Click += new System.EventHandler(this.PalRemoveMenuItemClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(133, 6);
			// 
			// palCopyAllMenuItem
			// 
			this.palCopyAllMenuItem.Name = "palCopyAllMenuItem";
			this.palCopyAllMenuItem.Size = new System.Drawing.Size(136, 22);
			this.palCopyAllMenuItem.Text = "Copy All";
			// 
			// palClearMenuItem
			// 
			this.palClearMenuItem.Name = "palClearMenuItem";
			this.palClearMenuItem.Size = new System.Drawing.Size(136, 22);
			this.palClearMenuItem.Text = "Clear";
			this.palClearMenuItem.Click += new System.EventHandler(this.PalClearMenuItemClick);
			// 
			// palOpenMenuItem
			// 
			this.palOpenMenuItem.Name = "palOpenMenuItem";
			this.palOpenMenuItem.Size = new System.Drawing.Size(136, 22);
			this.palOpenMenuItem.Text = "Open...";
			this.palOpenMenuItem.Click += new System.EventHandler(this.PalOpenMenuItemClick);
			// 
			// palSaveMenuItem
			// 
			this.palSaveMenuItem.Name = "palSaveMenuItem";
			this.palSaveMenuItem.Size = new System.Drawing.Size(136, 22);
			this.palSaveMenuItem.Text = "Save...";
			this.palSaveMenuItem.Click += new System.EventHandler(this.PalSaveMenuItemClick);
			// 
			// colourLoversToolStripMenuItem
			// 
			this.colourLoversToolStripMenuItem.Name = "colourLoversToolStripMenuItem";
			this.colourLoversToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.colourLoversToolStripMenuItem.Text = "COLOURlovers...";
			this.colourLoversToolStripMenuItem.Click += new System.EventHandler(this.ColourLoversToolStripMenuItemClick);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(186, 6);
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(187, 179);
			this.ContextMenuStrip = this.contextMenuStrip1;
			this.Controls.Add(this.tableLayoutPanel1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Tiny Colorful Dots";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.lastColorsBox)).EndInit();
			this.paletteContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem palReplaceMenuItem;
		private System.Windows.Forms.ToolStripMenuItem palOpenMenuItem;
		private System.Windows.Forms.ToolStripMenuItem palSaveMenuItem;
		private System.Windows.Forms.ToolStripMenuItem palClearMenuItem;
		private System.Windows.Forms.ToolStripMenuItem palCopyAllMenuItem;
		private System.Windows.Forms.ToolStripMenuItem palRemoveMenuItem;
		private System.Windows.Forms.ToolStripMenuItem palCopyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem colourLoversToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ContextMenuStrip paletteContextMenu;
		private System.Windows.Forms.PictureBox lastColorsBox;
		private System.Windows.Forms.Panel colorPreviewPanel;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem colorPickerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem decimalFormatToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ComboBox blurBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ListBox listBox1;
	}
}
