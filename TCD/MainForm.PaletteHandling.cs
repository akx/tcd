using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TCD
{
	/// <summary>
	/// Description of MainForm_PaletteHandling.
	/// </summary>
	public partial class MainForm
	{
		CPalette cPalette = new CPalette();
		//List<Color> cPalette.Colors = new List<Color>();
		Bitmap lastColorsBuf;
		Graphics lastColorsGfx;
		Point lastColorsMPos;
		int lastColorsMIdx;
		
		void InitPaletteHandling() {			
			lastColorsBuf = new Bitmap(lastColorsBox.Size.Width, lastColorsBox.Size.Height, PixelFormat.Format32bppPArgb);
			lastColorsGfx = Graphics.FromImage(lastColorsBuf);
		}
		
		
		void UpdateLastColors(bool redraw, bool isClicked) {
			if(redraw) lastColorsGfx.Clear(Color.White);
			int x, y;
			x = y = 0;
			bool touchCursor = false;
			lastColorsMIdx = -2;
			for(int i = -1; i < cPalette.Colors.Count; i ++) {
				Rectangle bbox = new Rectangle(x, y, 16, 16);
				if(bbox.Contains(lastColorsMPos)) {
					touchCursor = true;
					lastColorsMIdx = i;
					if(isClicked) {
						if(i == -1) {
							cPalette.Colors.Insert(0, currentColor);
							redraw = true;
						} else {
							if(Control.ModifierKeys == Keys.Shift) {
								Color c0 = currentColor;
								Color c1 = cPalette.Colors[i];
								UpdateColor(Color.FromArgb((int)((c0.R + c1.R)*.5), (int)((c0.G + c1.G)*.5), (int)((c0.B + c1.B)*.5)));
							} else {
								UpdateColor(cPalette.Colors[i]);
							}
						}
					}
				}
				if(redraw) {
					if(i == -1) {
						
						lastColorsGfx.DrawLine(Pens.Gray, bbox.Left + 8, bbox.Top + 4, bbox.Left + 8, bbox.Bottom - 4);
						lastColorsGfx.DrawLine(Pens.Gray, bbox.Left + 4, bbox.Top + 8, bbox.Right - 4, bbox.Top + 8);	
						
						lastColorsGfx.DrawRectangle(Pens.Black, bbox);
					} else {
						
						lastColorsGfx.FillRectangle(new SolidBrush(cPalette.Colors[i]), bbox.Left, bbox.Top, 17, 17);
					}
				}
				x += 17;
				if(x >= lastColorsBuf.Width - 16) {
					x = 0;
					y += 17;
				}
			}
			if(redraw) lastColorsBox.Image = lastColorsBuf;
			lastColorsBox.Cursor = (touchCursor ? Cursors.Hand : Cursors.Arrow);
		}
		
		
		void lastColorsBoxMouseMove(object sender, MouseEventArgs e)
		{
			lastColorsMPos = e.Location;
			UpdateLastColors(false, false);
		}
		
		
		void lastColorsBoxClick(object sender, EventArgs e)
		{
			MouseEventArgs mea = e as MouseEventArgs;
			if(mea.Button == MouseButtons.Left) UpdateLastColors(true, true);
		}
		
		void PaletteContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(lastColorsMIdx >= 0) {
				AddCopyItems(palCopyMenuItem, cPalette.Colors[lastColorsMIdx], false);
				palRemoveMenuItem.Enabled = palReplaceMenuItem.Enabled = palCopyMenuItem.Enabled = true;
			} else {
				palRemoveMenuItem.Enabled = palReplaceMenuItem.Enabled = palCopyMenuItem.Enabled = false;
			}
			if(cPalette.Colors.Count > 0) {
				palCopyAllMenuItem.Enabled = true;
				AddCopyItems(palCopyAllMenuItem, cPalette.Colors[0], true);
			} else {
				palCopyAllMenuItem.Enabled = false;
			}
		}
		
		
		
		void ColourLoversToolStripMenuItemClick(object sender, EventArgs e)
		{
			ColourLoversBrowser clb = new ColourLoversBrowser(delegate(CPalette clp) {
	        	foreach(Color c in clp.Colors) {
	        		cPalette.Colors.Insert(0, c);
	        	}
	        	UpdateLastColors(true, false);
			});
			clb.Show();
		}		
		
		
		void PalRemoveMenuItemClick(object sender, EventArgs e)
		{
			if(lastColorsMIdx >= 0) {
				cPalette.Colors.RemoveAt(lastColorsMIdx);
				UpdateLastColors(true, false);
			}
		}
		
		private void CopyPaletteUsingConverter(ColorToStringConverterDelegate converter) {
			StringBuilder sb = new StringBuilder();
			foreach(Color c in cPalette.Colors) {
				sb.AppendLine(converter(c));
			}
			Clipboard.SetText(sb.ToString().TrimEnd());
		}
		
		private EventHandler MakeCopyPaletteDelegate(ColorToStringConverterDelegate converter) {
			return new EventHandler(delegate(object sender, EventArgs ea) { CopyPaletteUsingConverter(converter); });
		}
		
		private void AddCopyItems(ToolStripMenuItem parentItem, Color c, bool copyAllMode) {
			parentItem.DropDownItems.Clear();
			foreach(ColorToStringConverterDelegate converter in cConverters) {
				string s = converter(c);
				EventHandler eh;
				if(copyAllMode) {
					eh = MakeCopyPaletteDelegate(converter);
				} else {
					eh = new EventHandler(delegate(object sender, EventArgs ea) { Clipboard.SetText(s); });
				}
				ToolStripMenuItem mi = new ToolStripMenuItem(s, null, eh);
				parentItem.DropDownItems.Add(mi);
			}
		}
		
		void PalClearMenuItemClick(object sender, EventArgs e)
		{
			cPalette.Colors.Clear();
			UpdateLastColors(true, false);
		}
		
		void PalOpenMenuItemClick(object sender, EventArgs e)
		{
			OpenFileDialog fd = new OpenFileDialog();
			fd.CheckFileExists = true;
			fd.Filter = "GIMP Palette Format (*.GPL)|*.gpl";
			if(fd.ShowDialog() == DialogResult.OK) {
				using(FileStream fs = new FileStream(fd.FileName, FileMode.Open, FileAccess.Read)) {
					cPalette.ReadGPLStream(fs);
				}
				UpdateLastColors(true, false);
			}
		}
		
		void PalSaveMenuItemClick(object sender, EventArgs e)
		{
			SaveFileDialog fd = new SaveFileDialog();
			fd.CheckPathExists = true;
			fd.AddExtension = true;
			fd.Filter = "GIMP Palette Format (*.GPL)|*.gpl";
			if(fd.ShowDialog() == DialogResult.OK) {
				using(FileStream fs = new FileStream(fd.FileName, FileMode.OpenOrCreate, FileAccess.Write)) {
					cPalette.WriteGPLStream(fs);
				}
			}
		}

		void PalReplaceMenuItemClick(object sender, EventArgs e)
		{
			if(lastColorsMIdx >= 0) {
				cPalette.Colors.RemoveAt(lastColorsMIdx);
				cPalette.Colors.Insert(lastColorsMIdx, currentColor);
				UpdateLastColors(true, false);
			}
		}
	}
}
