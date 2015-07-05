/*
 * Date: 21.12.2009
 * Time: 2:31
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using System.Windows.Forms;
using System.Xml.XPath;

namespace TCD.ColourLovers
{
	public delegate void PaletteSelectedDelegate(CPalette clp);


	public partial class ColourLoversBrowser : Form
	{
		private readonly List<CPalette> palettes;
		private readonly PaletteSelectedDelegate selectedDelegate;
		private HttpWebRequest currentRequest;
		private XPathDocument currentResultSet;

		public ColourLoversBrowser(PaletteSelectedDelegate selectedDelegate)
		{
			InitializeComponent();
			setCurrentRequest(null);
			this.selectedDelegate = selectedDelegate;
			palettes = new List<CPalette>();
			SearchTypeComboBoxSelectedIndexChanged(null, null);
		}

		private void setCurrentRequest(HttpWebRequest req)
		{
			bool EnableState = (req == null);
			if (req == null)
			{
				throbberBar.Style = ProgressBarStyle.Continuous;
				throbberBar.Value = 0;
			}
			else
			{
				throbberBar.Style = ProgressBarStyle.Marquee;
			}
			foreach (ToolStripItem tsi in toolStrip1.Items) tsi.Enabled = EnableState;
			currentRequest = req;
		}

		private void setStatus(String s)
		{
			if (s == null) s = "...";
			statusLabel.Text = s;
		}

		private void ColourLoversBrowserLoad(object sender, EventArgs e)
		{
			searchTypeComboBox.SelectedIndex = 2;
		}

		private void SearchButtonClick(object sender, EventArgs e)
		{
			if (currentRequest != null) return;
			string uri = "http://www.colourlovers.com/api/palettes";
			switch (searchTypeComboBox.SelectedIndex)
			{
				case 0: // search;
					break;
				case 1: // new
					uri += "/new";
					break;
				case 2: // top
					uri += "/top";
					break;
				case 3: // random
					uri += "/random";
					break;
			}
			var getKeys = new Dictionary<string, string>();
			if (searchKeywordsBox.Text.Trim() != "") getKeys["keywords"] = searchKeywordsBox.Text.Trim();
			getKeys["format"] = "xml";
			getKeys["numResults"] = "40";
			uri += "?";
			foreach (var kv in getKeys) uri += kv.Key + "=" + HttpUtility.UrlEncode(kv.Value) + "&";
			Debug.Print(uri);

			var req = (HttpWebRequest) WebRequest.Create(uri);
			setCurrentRequest(req);
			var bw = new BackgroundWorker();
			bw.DoWork += delegate(object bworker, DoWorkEventArgs dwea)
			{
				var worker = (BackgroundWorker) bworker;
				worker.ReportProgress(10, "Connecting...");
				WebResponse resp = currentRequest.GetResponse();
				Stream respStream = resp.GetResponseStream();
				worker.ReportProgress(10, "Parsing...");
				var xpd = new XPathDocument(respStream);
				respStream.Close();
				currentResultSet = xpd;
				worker.ReportProgress(10, "Finished.");
			};
			bw.ProgressChanged += delegate(object bworker, ProgressChangedEventArgs evt) { setStatus(evt.UserState as string); };
			bw.RunWorkerCompleted += UpdateResultsList;
			bw.WorkerReportsProgress = true;
			bw.RunWorkerAsync();
		}

		private void UpdateResultsList(object sender, RunWorkerCompletedEventArgs e)
		{
			UpdateResultsList();
		}

		private void UpdateResultsList()
		{
			parsePalettesXml(currentResultSet);
			setStatus(String.Format("Search complete. {0} results.", palettes.Count));
			resultListBox.Items.Clear();
			resultListBox.Items.AddRange(palettes.ToArray());
			setCurrentRequest(null);
		}


		private void parsePalettesXml(XPathDocument doc)
		{
			XPathNavigator xn = doc.CreateNavigator();
			xn.MoveToRoot();
			palettes.Clear();
			XPathNodeIterator paletteNodes = xn.Select("//palette");
			while (paletteNodes.MoveNext())
			{
				var pal = new CPalette();
				pal.PopulateFromXPathNode(paletteNodes.Current);
				palettes.Add(pal);
			}
		}

		private void drawSpanStrings(Graphics target, PointF origin, params object[] args)
		{
			for (int i = 0; i < args.Length; i += 3)
			{
				var f = (Font) args[i];
				var b = (Brush) args[i + 1];
				var s = (String) args[i + 2];
				if (s != "")
				{
					var width = (int) Math.Ceiling(target.MeasureString(s, f).Width - 1.5);
					target.DrawString(s, f, b, origin);
					origin.X += width;
				}
			}
		}


		private void ResultListBoxDrawItem(object sender, DrawItemEventArgs e)
		{
			var lb = (ListBox) sender;
			Graphics g = e.Graphics;
			Font font = lb.Font;
			var boldFont = new Font(font.Name, font.Size, FontStyle.Bold);
			var fh = (int) font.GetHeight(g);
			e.DrawBackground();
			if (e.Index < 0) return;
			var pal = (CPalette) lb.Items[e.Index];


			drawSpanStrings(
				g, new PointF(e.Bounds.X, e.Bounds.Y),
				boldFont, Brushes.Black, pal.Title,
				font, Brushes.Gray, "by ",
				font, Brushes.Black, pal.UserName
				);
			drawSpanStrings(
				g, new PointF(e.Bounds.X, e.Bounds.Y + fh),
				font, Brushes.Green, String.Format("{0} views", pal.NumViews),
				font, Brushes.Red, pal.GetHeartsStr()
				);
			int y0 = e.Bounds.Y + fh*2 + 2;
			int y1 = e.Bounds.Bottom - 3;
			int x0 = e.Bounds.X + 3;
			int x1 = e.Bounds.Right - 3;
			int w = x1 - x0;
			int h = y1 - y0;
			Color[] colors = pal.Colors.ToArray();
			int sw = w/colors.Length;
			for (int i = 0; i < colors.Length; i++)
			{
				int xx0 = x0 + sw*i;
				g.FillRectangle(new SolidBrush(colors[i]), new RectangleF(xx0, y0, sw, h));
			}
			g.DrawRectangle(Pens.Black, new Rectangle(x0, y0, w, h));
		}

		private void ContextMenuStrip1Opening(object sender, CancelEventArgs e)
		{
			foreach (ToolStripItem item in ((ContextMenuStrip) sender).Items) item.Enabled = (resultListBox.SelectedIndex >= 0);
			Debug.Print("Debug: {0}", sender);
		}

		private CPalette GetCurrentlySelectedPalette()
		{
			if (resultListBox.SelectedIndex < 0) return null;
			return (CPalette) resultListBox.SelectedItem;
		}

		private void VisitPaletteURLToolStripMenuItemClick(object sender, EventArgs e)
		{
			CPalette pal = GetCurrentlySelectedPalette();
			if (pal != null) Process.Start(pal.Url);
		}

		private void ApplyToolStripMenuItemClick(object sender, EventArgs e)
		{
			CPalette pal = GetCurrentlySelectedPalette();
			if (pal == null) return;
			if (selectedDelegate != null)
			{
				selectedDelegate(pal);
			}
		}

		private void CopyColorsToolStripMenuItemClick(object sender, EventArgs e)
		{
			CPalette pal = GetCurrentlySelectedPalette();
			if (pal == null) return;
			string descStr = pal.GetColorDescStr();
			Clipboard.SetText(descStr);
			MessageBox.Show("Palette colors copied onto clipboard.", "Copied.", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ResultListBoxResize(object sender, EventArgs e)
		{
			resultListBox.Update();
		}

		private void SearchTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			string sel = searchTypeComboBox.SelectedText;
			searchKeywordsBox.Enabled = (sel == "Search");
		}
	}
}