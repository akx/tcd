using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace TCD
{
	public partial class MainForm : Form
	{
		private static readonly ColorToStringConverterDelegate[] CConverters =
		{
			Converters.SixHex,
			Converters.SixHexPS,
			Converters.CSSRGB,
			Converters.CSSHSV,
			Converters.DecimalRGB,
			Converters.HSL,
			Converters.CMYK
		};

		private static readonly StringToColorConverterDelegate[] CRecognizers =
		{
			Recognizers.FromHex,
			Recognizers.FromHSV,
			Recognizers.FromHSL,
			Recognizers.FromRGB
		};

		private readonly string _configFile;
		private readonly Bitmap _scaleBuf = new Bitmap(64, 64, PixelFormat.Format32bppPArgb);
		private readonly Graphics _scaleGfx;
		private readonly Bitmap _screenGrabBuf = new Bitmap(16, 16, PixelFormat.Format32bppPArgb);
		private readonly Graphics _screenGrabGfx;
		private readonly Timer _updateTimer = new Timer();
		private Color _currentColor = Color.Black;
		private Point? _grabFromPic;

		public MainForm()
		{
			InitializeComponent();
			_updateTimer.Interval = 1000/40;
			_updateTimer.Tick += updateTimer_Tick;
			_updateTimer.Enabled = true;
			_screenGrabGfx = Graphics.FromImage(_screenGrabBuf);
			_scaleGfx = Graphics.FromImage(_scaleBuf);
			_scaleGfx.InterpolationMode = InterpolationMode.NearestNeighbor;
			blurBox.SelectedIndex = 0;
			Text += " " + Application.ProductVersion;
			InitPaletteHandling();

			_configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TCD.conf.xml");
			try
			{
				LoadConfig();
				UpdateColor(_currentColor);
				UpdateLastColors(true, false);
			}
			catch (Exception exc)
			{
				MessageBox.Show("Failed loading the configuration file:" + _configFile + "\n(Error: " + exc.Message + ")");
			}
		}

		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				SaveConfig();
			}
			catch (Exception exc)
			{
				MessageBox.Show("Failed saving the configuration file:" + _configFile + "\n(Error: " + exc.Message + ")");
			}
			e.Cancel = false;
		}

		private void LoadConfig()
		{
			var config = new Dictionary<string, string>();
			if (!File.Exists(_configFile)) return;
			using (var fileStream = new FileStream(_configFile, FileMode.Open))
			{
				var xrs = new XmlReaderSettings {IgnoreWhitespace = true, IgnoreComments = true};
				XmlReader xr = XmlReader.Create(fileStream, xrs);
				xr.Read();
				xr.ReadStartElement("Config");
				while (true)
				{
					xr.MoveToElement();
					if (xr.NodeType == XmlNodeType.EndElement) break;
					config[xr.Name] = xr.ReadElementString();
				}
			}
			if (config.ContainsKey("AlwaysOnTop")) alwaysOnTopToolStripMenuItem.Checked = Convert.ToBoolean(config["AlwaysOnTop"]);
			if (config.ContainsKey("DecimalFormat")) decimalFormatToolStripMenuItem.Checked = Convert.ToBoolean(config["DecimalFormat"]);
			if (config.ContainsKey("LastColor")) _currentColor = Recognizers.FromHex(config["LastColor"]).GetValueOrDefault(Color.Black);
			if (config.ContainsKey("Palette"))
			{
				foreach (string hex in config["Palette"].Split(null))
				{
					_palette.Colors.Add(Recognizers.FromHex(hex).GetValueOrDefault(Color.Black));
				}
			}
			if (config.ContainsKey("Position"))
			{
				string[] xy = config["Position"].Split(',');
				Location = new Point(Convert.ToInt32(xy[0]), Convert.ToInt32(xy[1]));
			}
		}

		private void SaveConfig()
		{
			var config = new Dictionary<string, string>();
			config["AlwaysOnTop"] = alwaysOnTopToolStripMenuItem.Checked.ToString();
			config["DecimalFormat"] = decimalFormatToolStripMenuItem.Checked.ToString();
			var paletteSb = new StringBuilder();
			foreach (Color c in _palette.Colors)
			{
				paletteSb.Append(Converters.SixHex(c));
				paletteSb.Append(" ");
			}
			config["Palette"] = paletteSb.ToString().TrimEnd();
			config["LastColor"] = Converters.SixHex(_currentColor);
			config["Position"] = String.Format("{0},{1}", Location.X, Location.Y);
			using (var fileStream = new FileStream(_configFile, FileMode.Create, FileAccess.Write))
			{
				var xws = new XmlWriterSettings();
				xws.Indent = true;
				xws.IndentChars = "\t";
				XmlWriter xw = XmlWriter.Create(fileStream, xws);
				xw.WriteStartDocument();
				xw.WriteStartElement("Config");
				foreach (var kvp in config)
				{
					xw.WriteStartElement(kvp.Key);
					xw.WriteString(kvp.Value);
					xw.WriteEndElement();
				}
				xw.WriteEndElement();
				xw.Flush();
			}
		}

		private void updateTimer_Tick(object sender, EventArgs e)
		{
			if (ModifierKeys == (Keys.Shift | Keys.Control))
			{
				Point p = MousePosition;
				p.Offset(-8, -8);
				_screenGrabGfx.CopyFromScreen(p, Point.Empty, _screenGrabBuf.Size);
				if (blurBox.SelectedIndex > 0)
				{
					_screenGrabGfx.DrawImage(Blur(_screenGrabBuf, blurBox.SelectedIndex*2), 0, 0);
				}
				Color c = Color.FromArgb(255, _screenGrabBuf.GetPixel(8, 8));
				_scaleGfx.Clear(Color.Transparent);
				_scaleGfx.DrawImage(_screenGrabBuf, 0, 0, 64, 64);
				_scaleGfx.DrawRectangle(Pens.Black, 32 - 2, 32 - 2, 4, 4);
				pictureBox1.Image = _scaleBuf;
				UpdateColor(c);
			}
			if (!_grabFromPic.HasValue) return;
			if (_grabFromPic.Value.X >= 0 && _grabFromPic.Value.Y >= 0)
			{
				Color c = Color.FromArgb(255, _scaleBuf.GetPixel(_grabFromPic.Value.X, _grabFromPic.Value.Y));
				UpdateColor(c);
			}
		}

		private void UpdateColor(Color color)
		{
			_currentColor = color;
			try
			{
				listBox1.BeginUpdate();
				listBox1.Items.Clear();
				for (int i = 0; i < CConverters.Length; i++)
				{
					listBox1.Items.Add((CConverters[i])(color));
				}
			}
			finally
			{
				listBox1.EndUpdate();
			}
			colorPreviewPanel.BackColor = color;
		}

		private void ListBox1DoubleClick(object sender, EventArgs e)
		{
			var me = e as MouseEventArgs;
			string s = listBox1.SelectedItem.ToString();
			Clipboard.SetText(s);
			if (me != null)
			{
				new ToolTip().Show("Copied '" + s + "'", listBox1, me.Location, 500);
			}
		}


		private static Bitmap Blur(Bitmap image, Int32 blurSize)
		{
			var rectangle = new Rectangle(0, 0, image.Width, image.Height);
			var blurred = new Bitmap(image.Width, image.Height);

			// make an exact copy of the bitmap provided
			using (Graphics graphics = Graphics.FromImage(blurred))
				graphics.DrawImage(image, rectangle, rectangle, GraphicsUnit.Pixel);

			// look at every pixel in the blur rectangle
			for (int xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
			{
				for (int yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
				{
					float avgR = 0, avgG = 0, avgB = 0;
					float blurPixelCount = 0;

					// average the color of the red, green and blue for each pixel in the
					// blur size while making sure you don't go outside the image bounds
					for (int x = Math.Max(0, xx - blurSize); x < Math.Min(xx + blurSize, image.Width); x++)
					{
						for (int y = Math.Max(0, yy - blurSize); y < Math.Min(yy + blurSize, image.Height); y++)
						{
							float dist = 1.0f - (float) (Math.Sqrt((x - xx)*(x - xx) + (y - yy)*(y - yy))/blurSize);
							if (dist <= 0) continue;
							Color pixel = image.GetPixel(x, y);

							avgR += pixel.R*dist;
							avgG += pixel.G*dist;
							avgB += pixel.B*dist;

							blurPixelCount += dist;
						}
					}

					avgR = avgR/blurPixelCount;
					avgG = avgG/blurPixelCount;
					avgB = avgB/blurPixelCount;

					// now that we know the average for the blur size, set each pixel to that color
					blurred.SetPixel(xx, yy, Color.FromArgb((int) avgR, (int) avgG, (int) avgB));
				}
			}

			return blurred;
		}


		private void DecimalFormatToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Converters.PercentageDecimal = (decimalFormatToolStripMenuItem.Checked);
			UpdateColor(_currentColor);
		}

		private void ColorPickerToolStripMenuItemClick(object sender, EventArgs e)
		{
			OpenColorPicker();
		}

		private void OpenColorPicker()
		{
			var cd = new ColorDialog();
			cd.FullOpen = true;
			cd.Color = _currentColor;
			if (cd.ShowDialog() != DialogResult.OK) return;
			_scaleGfx.FillRectangle(new LinearGradientBrush(new Rectangle(0, 0, _scaleBuf.Width, _scaleBuf.Height/2), Color.White, cd.Color, 90), 0, 0, _scaleBuf.Width, _scaleBuf.Height/2);
			_scaleGfx.FillRectangle(new LinearGradientBrush(new Rectangle(0, _scaleBuf.Height/2, _scaleBuf.Width, _scaleBuf.Height/2), cd.Color, Color.Black, 90), 0, _scaleBuf.Height/2, _scaleBuf.Width, _scaleBuf.Height/2);
			pictureBox1.Image = _scaleBuf;
			UpdateColor(cd.Color);
		}


		private void AlwaysOnTopToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			TopMost = (alwaysOnTopToolStripMenuItem.Checked);
		}

		private void PasteToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!Clipboard.ContainsText())
			{
				MessageBox.Show("There is no text on the clipboard.");
				return;
			}
			string paste = Clipboard.GetText().Trim();
			paste = paste.Substring(0, Math.Min(32, paste.Length));
			foreach (StringToColorConverterDelegate converter in CRecognizers)
			{
				Color? c = converter(paste);
				if (!c.HasValue) continue;
				UpdateColor(c.Value);
				return;
			}
			MessageBox.Show(String.Format("The color format for \"{0}\" could not be recognized.", paste));
		}

		private void ContextMenuStrip1Opening(object sender, CancelEventArgs e)
		{
			var cms = (ContextMenuStrip) sender;
			copyToolStripMenuItem.Enabled = (cms.SourceControl == listBox1 && listBox1.SelectedItem != null);
		}

		private void CopyToolStripMenuItemClick(object sender, EventArgs e)
		{
			ListBox1DoubleClick(listBox1, e);
		}

		private void ListBox1MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				listBox1.SelectedIndex = listBox1.IndexFromPoint(e.Location);
				listBox1.Refresh();
			}
		}

		private void PictureBox1MouseDown(object sender, MouseEventArgs e)
		{
			_grabFromPic = e.Location;
		}

		private void PictureBox1MouseUp(object sender, MouseEventArgs e)
		{
			_grabFromPic = null;
		}

		private void PictureBox1MouseMove(object sender, MouseEventArgs e)
		{
			if (_grabFromPic.HasValue) _grabFromPic = e.Location;
		}

		private void ColorPreviewPanelDoubleClick(object sender, EventArgs e)
		{
			OpenColorPicker();
		}
	}
}