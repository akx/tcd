using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TCD.ColourLovers;

namespace TCD
{
	public partial class MainForm
	{
		private readonly CPalette _palette = new CPalette();
		private Bitmap _lastColorsBuf;
		private Graphics _lastColorsGfx;
		private int _lastColorsMIdx;
		private Point _lastColorsMPos;

		private void InitPaletteHandling()
		{
			_lastColorsBuf = new Bitmap(lastColorsBox.Size.Width, lastColorsBox.Size.Height, PixelFormat.Format32bppPArgb);
			_lastColorsGfx = Graphics.FromImage(_lastColorsBuf);
		}


		private void UpdateLastColors(bool redraw, bool isClicked)
		{
			if (redraw) _lastColorsGfx.Clear(Color.White);
			int x, y;
			x = y = 0;
			bool touchCursor = false;
			_lastColorsMIdx = -2;
			for (int i = -1; i < _palette.Colors.Count; i ++)
			{
				var bbox = new Rectangle(x, y, 16, 16);
				if (bbox.Contains(_lastColorsMPos))
				{
					touchCursor = true;
					_lastColorsMIdx = i;
					if (isClicked)
					{
						if (i == -1)
						{
							_palette.Colors.Insert(0, _currentColor);
							redraw = true;
						}
						else
						{
							if (ModifierKeys == Keys.Shift)
							{
								Color c0 = _currentColor;
								Color c1 = _palette.Colors[i];
								UpdateColor(Color.FromArgb((int) ((c0.R + c1.R)*.5), (int) ((c0.G + c1.G)*.5), (int) ((c0.B + c1.B)*.5)));
							}
							else
							{
								UpdateColor(_palette.Colors[i]);
							}
						}
					}
				}
				if (redraw)
				{
					if (i == -1)
					{
						_lastColorsGfx.DrawLine(Pens.Gray, bbox.Left + 8, bbox.Top + 4, bbox.Left + 8, bbox.Bottom - 4);
						_lastColorsGfx.DrawLine(Pens.Gray, bbox.Left + 4, bbox.Top + 8, bbox.Right - 4, bbox.Top + 8);

						_lastColorsGfx.DrawRectangle(Pens.Black, bbox);
					}
					else
					{
						_lastColorsGfx.FillRectangle(new SolidBrush(_palette.Colors[i]), bbox.Left, bbox.Top, 17, 17);
					}
				}
				x += 17;
				if (x >= _lastColorsBuf.Width - 16)
				{
					x = 0;
					y += 17;
				}
			}
			if (redraw) lastColorsBox.Image = _lastColorsBuf;
			lastColorsBox.Cursor = (touchCursor ? Cursors.Hand : Cursors.Arrow);
		}


		private void lastColorsBoxMouseMove(object sender, MouseEventArgs e)
		{
			_lastColorsMPos = e.Location;
			UpdateLastColors(false, false);
		}


		private void lastColorsBoxClick(object sender, EventArgs e)
		{
			var mea = e as MouseEventArgs;
			if (mea.Button == MouseButtons.Left) UpdateLastColors(true, true);
		}

		private void PaletteContextMenuOpening(object sender, CancelEventArgs e)
		{
			if (_lastColorsMIdx >= 0)
			{
				AddCopyItems(palCopyMenuItem, _palette.Colors[_lastColorsMIdx], false);
				palRemoveMenuItem.Enabled = palReplaceMenuItem.Enabled = palCopyMenuItem.Enabled = true;
			}
			else
			{
				palRemoveMenuItem.Enabled = palReplaceMenuItem.Enabled = palCopyMenuItem.Enabled = false;
			}
			if (_palette.Colors.Count > 0)
			{
				palCopyAllMenuItem.Enabled = true;
				AddCopyItems(palCopyAllMenuItem, _palette.Colors[0], true);
			}
			else
			{
				palCopyAllMenuItem.Enabled = false;
			}
		}


		private void ColourLoversToolStripMenuItemClick(object sender, EventArgs e)
		{
			var clb = new ColourLoversBrowser(delegate(CPalette clp)
			{
				foreach (Color c in clp.Colors)
				{
					_palette.Colors.Insert(0, c);
				}
				UpdateLastColors(true, false);
			});
			clb.Show();
		}


		private void PalRemoveMenuItemClick(object sender, EventArgs e)
		{
			if (_lastColorsMIdx >= 0)
			{
				_palette.Colors.RemoveAt(_lastColorsMIdx);
				UpdateLastColors(true, false);
			}
		}

		private void CopyPaletteUsingConverter(ColorToStringConverterDelegate converter)
		{
			var sb = new StringBuilder();
			foreach (Color c in _palette.Colors)
			{
				sb.AppendLine(converter(c));
			}
			Clipboard.SetText(sb.ToString().TrimEnd());
		}

		private EventHandler MakeCopyPaletteDelegate(ColorToStringConverterDelegate converter)
		{
			return (sender, ea) => CopyPaletteUsingConverter(converter);
		}

		private void AddCopyItems(ToolStripMenuItem parentItem, Color c, bool copyAllMode)
		{
			parentItem.DropDownItems.Clear();
			foreach (ColorToStringConverterDelegate converter in CConverters)
			{
				string s = converter(c);
				EventHandler eh;
				if (copyAllMode)
				{
					eh = MakeCopyPaletteDelegate(converter);
				}
				else
				{
					eh = (sender, ea) => Clipboard.SetText(s);
				}
				var mi = new ToolStripMenuItem(s, null, eh);
				parentItem.DropDownItems.Add(mi);
			}
		}

		private void PalClearMenuItemClick(object sender, EventArgs e)
		{
			_palette.Colors.Clear();
			UpdateLastColors(true, false);
		}

		private void PalOpenMenuItemClick(object sender, EventArgs e)
		{
			var fd = new OpenFileDialog();
			fd.CheckFileExists = true;
			fd.Filter = "GIMP Palette Format (*.GPL)|*.gpl";
			if (fd.ShowDialog() == DialogResult.OK)
			{
				using (var fs = new FileStream(fd.FileName, FileMode.Open, FileAccess.Read))
				{
					_palette.ReadGPLStream(fs);
				}
				UpdateLastColors(true, false);
			}
		}

		private void PalSaveMenuItemClick(object sender, EventArgs e)
		{
			var fd = new SaveFileDialog();
			fd.CheckPathExists = true;
			fd.AddExtension = true;
			fd.Filter = "GIMP Palette Format (*.GPL)|*.gpl";
			if (fd.ShowDialog() == DialogResult.OK)
			{
				using (var fs = new FileStream(fd.FileName, FileMode.OpenOrCreate, FileAccess.Write))
				{
					_palette.WriteGPLStream(fs);
				}
			}
		}

		private void PalReplaceMenuItemClick(object sender, EventArgs e)
		{
			if (_lastColorsMIdx >= 0)
			{
				_palette.Colors.RemoveAt(_lastColorsMIdx);
				_palette.Colors.Insert(_lastColorsMIdx, _currentColor);
				UpdateLastColors(true, false);
			}
		}
	}
}