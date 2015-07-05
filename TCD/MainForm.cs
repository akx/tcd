/*
 * Date: 7.7.2010
 * Time: 19:47
 */
using System;
using System.Collections.Generic;
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
		ColorToStringConverterDelegate[] cConverters = new ColorToStringConverterDelegate[]{
			Converters.SixHex,
			Converters.SixHexPS,
			Converters.CSSRGB,
			Converters.CSSHSV,
			Converters.DecimalRGB,
			Converters.HSL,
			Converters.CMYK,
		};
		StringToColorConverterDelegate[] cRecognizers = new StringToColorConverterDelegate[]{
			Recognizers.FromHex,
			Recognizers.FromHSV,
			Recognizers.FromHSL,
			Recognizers.FromRGB
		};
		Timer updateTimer = new Timer();
		Bitmap screenGrabBuf = new Bitmap(16, 16, PixelFormat.Format32bppPArgb);
		Bitmap scaleBuf = new Bitmap(64, 64, PixelFormat.Format32bppPArgb);
		Graphics screenGrabGfx, scaleGfx;
		Color currentColor = Color.Black;
		Point? grabFromPic;
		string configFile;
		
		public MainForm()
		{
			InitializeComponent();
			updateTimer.Interval = 1000 / 40;
			updateTimer.Tick += new EventHandler(updateTimer_Tick);
			updateTimer.Enabled = true;
			screenGrabGfx = Graphics.FromImage(screenGrabBuf);
			scaleGfx = Graphics.FromImage(scaleBuf);
			scaleGfx.InterpolationMode = InterpolationMode.NearestNeighbor;
			blurBox.SelectedIndex = 0;
			Version v = new Version(Application.ProductVersion);
			Text += " " + v.Major + "." + v.Minor;
			InitPaletteHandling();
			
			configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TCD.conf.xml");
			try {
				LoadConfig();
				UpdateColor(currentColor);
				UpdateLastColors(true, false);				
			} catch(Exception exc) {
				MessageBox.Show("Failed loading the configuration file:" + configFile + "\n(Error: " + exc.Message + ")");
			}
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			try {
				SaveConfig();
			} catch(Exception exc) {
				MessageBox.Show("Failed saving the configuration file:" + configFile + "\n(Error: " + exc.Message + ")");
			}
			e.Cancel = false;
		}
		
		void LoadConfig() {
			Dictionary<string, string> config = new Dictionary<string, string>();
			if(File.Exists(configFile)) {
				using(FileStream fileStream = new FileStream(configFile, FileMode.Open)) {
					XmlReaderSettings xrs = new XmlReaderSettings();
					xrs.IgnoreWhitespace = true;
					xrs.IgnoreComments = true;
					XmlReader xr = XmlReader.Create(fileStream, xrs);
					xr.Read();
					xr.ReadStartElement("Config");
					while(true) {					
						xr.MoveToElement();
						if(xr.NodeType == XmlNodeType.EndElement) break;
						string key = xr.Name;
						string value = xr.ReadElementString();
						config[key] = value;
					}
				}
				if(config.ContainsKey("AlwaysOnTop")) alwaysOnTopToolStripMenuItem.Checked = Convert.ToBoolean(config["AlwaysOnTop"]);
				if(config.ContainsKey("DecimalFormat")) decimalFormatToolStripMenuItem.Checked = Convert.ToBoolean(config["DecimalFormat"]);
				if(config.ContainsKey("LastColor")) currentColor = Recognizers.FromHex(config["LastColor"]).GetValueOrDefault(Color.Black);
				if(config.ContainsKey("Palette")) {
					foreach(string hex in config["Palette"].Split(null)) {
						cPalette.Colors.Add(Recognizers.FromHex(hex).GetValueOrDefault(Color.Black));
					}
				}
				if(config.ContainsKey("Position")) {
					string[] xy = config["Position"].Split(',');
					Location = new Point(Convert.ToInt32(xy[0]), Convert.ToInt32(xy[1]));
				}
			}
		}
		
		void SaveConfig() {
			Dictionary<string, string> config = new Dictionary<string, string>();
			config["AlwaysOnTop"] = alwaysOnTopToolStripMenuItem.Checked.ToString();
			config["DecimalFormat"] = decimalFormatToolStripMenuItem.Checked.ToString();
			StringBuilder paletteSB = new StringBuilder();
			foreach(Color c in cPalette.Colors) {
				paletteSB.Append(Converters.SixHex(c));
				paletteSB.Append(" ");
			}
			config["Palette"] = paletteSB.ToString().TrimEnd();
			config["LastColor"] = Converters.SixHex(currentColor);
			config["Position"] = String.Format("{0},{1}", Location.X, Location.Y);
			using(FileStream fileStream = new FileStream(configFile, FileMode.Create, FileAccess.Write)) {
				XmlWriterSettings xws = new XmlWriterSettings();
				xws.Indent = true;
				xws.IndentChars = "\t";
				XmlWriter xw = XmlWriter.Create(fileStream, xws);
				xw.WriteStartDocument();
				xw.WriteStartElement("Config");
				foreach(KeyValuePair<string, string> kvp in config) {
					xw.WriteStartElement(kvp.Key);
					xw.WriteString(kvp.Value);
					xw.WriteEndElement();
				}
				xw.WriteEndElement();
				xw.Flush();
			}
		}

		void updateTimer_Tick(object sender, EventArgs e)
		{
			if(Control.ModifierKeys == (Keys.Shift | Keys.Control)) {
				Point p = Control.MousePosition;
				p.Offset(-8, -8);
				screenGrabGfx.CopyFromScreen(p, Point.Empty, screenGrabBuf.Size);
				if(blurBox.SelectedIndex > 0) {
					screenGrabGfx.DrawImage(Blur(screenGrabBuf, blurBox.SelectedIndex * 2), 0, 0);
				}
				Color c = Color.FromArgb(255, screenGrabBuf.GetPixel(8, 8));
				scaleGfx.Clear(Color.Transparent);
				scaleGfx.DrawImage(screenGrabBuf, 0, 0, 64, 64);
				scaleGfx.DrawRectangle(Pens.Black, 32 - 2, 32 - 2, 4, 4);
				pictureBox1.Image = scaleBuf;
				UpdateColor(c);
			}
			if(grabFromPic.HasValue) {
				if(grabFromPic.Value.X >= 0 && grabFromPic.Value.Y >= 0) {
					Color c = Color.FromArgb(255, scaleBuf.GetPixel(grabFromPic.Value.X, grabFromPic.Value.Y));
					UpdateColor(c);				
				}
			}
		}
		
		void UpdateColor(Color c) {
			currentColor = c;
			for(int i=0;i<cConverters.Length;i++) {
				string s = (cConverters[i])(c);
				if(i>=listBox1.Items.Count) listBox1.Items.Add(s);
				else listBox1.Items[i] = s;
			}
			colorPreviewPanel.BackColor = c;
		}
		
		void ListBox1DoubleClick(object sender, EventArgs e)
		{
			MouseEventArgs me = e as MouseEventArgs;
			string s = listBox1.SelectedItem.ToString();
			Clipboard.SetText(s);
			if(me != null) {
				new ToolTip().Show("Copied '" + s + "'", listBox1, me.Location, 500);
			}
		}
		
		
		
		private static Bitmap Blur(Bitmap image, Int32 blurSize)
		{
			Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
		    Bitmap blurred = new Bitmap(image.Width, image.Height);
		
		    // make an exact copy of the bitmap provided
		    using(Graphics graphics = Graphics.FromImage(blurred))
		        graphics.DrawImage(image, rectangle, rectangle, GraphicsUnit.Pixel);
		
		    // look at every pixel in the blur rectangle
		    for (Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
		    {
		        for (Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
		        {
		            float avgR = 0, avgG = 0, avgB = 0;
		            float blurPixelCount = 0;
		
		            // average the color of the red, green and blue for each pixel in the
		            // blur size while making sure you don't go outside the image bounds
		            for (Int32 x = Math.Max(0, xx - blurSize); x < Math.Min(xx + blurSize, image.Width); x++)
		            {
		            	for (Int32 y = Math.Max(0, yy - blurSize); y < Math.Min(yy + blurSize, image.Height); y++)
		                {
		            		float dist = 1.0f - (float)(Math.Sqrt((x - xx) * (x - xx) + (y - yy) * (y - yy)) / blurSize);
		            		if(dist <= 0) continue;
		                    Color pixel = image.GetPixel(x, y);
		
		                    avgR += pixel.R * dist;
		                    avgG += pixel.G * dist;
		                    avgB += pixel.B * dist;
		
		                    blurPixelCount += dist;
		                }
		            }
		
		            avgR = avgR / blurPixelCount;
		            avgG = avgG / blurPixelCount;
		            avgB = avgB / blurPixelCount;
		
		            // now that we know the average for the blur size, set each pixel to that color
		            blurred.SetPixel(xx, yy, Color.FromArgb((int)avgR, (int)avgG, (int)avgB));
		        }
		    }
		
		    return blurred;
		}

		
		void DecimalFormatToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Converters.PercentageDecimal = (decimalFormatToolStripMenuItem.Checked);
			UpdateColor(currentColor);
		}
		
		void ColorPickerToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			OpenColorPicker();
		}
		
		void OpenColorPicker() {
			ColorDialog cd = new ColorDialog();
			cd.FullOpen = true;
			cd.Color = currentColor;
			if(cd.ShowDialog() == DialogResult.OK) {
				
				scaleGfx.FillRectangle(new LinearGradientBrush(new Rectangle(0, 0, scaleBuf.Width, scaleBuf.Height/2), Color.White, cd.Color, 90), 0, 0, scaleBuf.Width, scaleBuf.Height / 2);
				scaleGfx.FillRectangle(new LinearGradientBrush(new Rectangle(0, scaleBuf.Height / 2, scaleBuf.Width, scaleBuf.Height/2), cd.Color, Color.Black, 90), 0, scaleBuf.Height / 2, scaleBuf.Width, scaleBuf.Height / 2);
				pictureBox1.Image = scaleBuf;
				UpdateColor(cd.Color);
			}
		}
		
		
		void AlwaysOnTopToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			TopMost = (alwaysOnTopToolStripMenuItem.Checked);
		}
		
		void PasteToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(!Clipboard.ContainsText()) {
				MessageBox.Show("There is no text on the clipboard.");
				return;
			}
			string paste = Clipboard.GetText().Trim();
			paste = paste.Substring(0, Math.Min(32, paste.Length));
			foreach(StringToColorConverterDelegate converter in cRecognizers) {
				Color? c = converter(paste);
				if(c.HasValue) {
					UpdateColor(c.Value);
					return;
				}
			}
			MessageBox.Show(String.Format("The color format for \"{0}\" could not be recognized.", paste));
			return;
		}
		
		void ContextMenuStrip1Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ContextMenuStrip cms = (ContextMenuStrip)sender;
			copyToolStripMenuItem.Enabled = (cms.SourceControl == listBox1 && listBox1.SelectedItem != null);
		}
		
		void CopyToolStripMenuItemClick(object sender, EventArgs e)
		{
			ListBox1DoubleClick(listBox1, e);
		}
		
		void ListBox1MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right) {
				listBox1.SelectedIndex = listBox1.IndexFromPoint(e.Location);
				listBox1.Refresh();
			}
		}
		
		void PictureBox1MouseDown(object sender, MouseEventArgs e)
		{
			grabFromPic = e.Location;
		}
		
		void PictureBox1MouseUp(object sender, MouseEventArgs e)
		{
			grabFromPic = null;
		}
		
		void PictureBox1MouseMove(object sender, MouseEventArgs e)
		{
			if(grabFromPic.HasValue) grabFromPic = e.Location;
		}
		
		void ColorPreviewPanelDoubleClick(object sender, EventArgs e)
		{
			OpenColorPicker();
		}

	}
}
