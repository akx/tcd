/*
 * Date: 21.12.2009
 * Time: 2:31
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;

namespace TCD
{
	public delegate void PaletteSelectedDelegate(CPalette clp);
	
	
	public partial class ColourLoversBrowser : Form
	{
		HttpWebRequest currentRequest;
		XPathDocument currentResultSet;
		List<CPalette> palettes;
		PaletteSelectedDelegate selectedDelegate;
		
		public ColourLoversBrowser(PaletteSelectedDelegate selectedDelegate)
		{
			InitializeComponent();
			setCurrentRequest(null);
			this.selectedDelegate = selectedDelegate;
			palettes = new List<CPalette>();
			SearchTypeComboBoxSelectedIndexChanged(null, null);
		}
		
		void setCurrentRequest(HttpWebRequest req)
		{
			bool EnableState = (req==null);
			if(req==null)
			{
				throbberBar.Style = ProgressBarStyle.Continuous;
				throbberBar.Value = 0;
			}
			else
			{
				throbberBar.Style = ProgressBarStyle.Marquee;
			}
			foreach(ToolStripItem tsi in toolStrip1.Items) tsi.Enabled = EnableState;
			currentRequest = req;
		}
		
		void setStatus(String s)
		{
			if(s == null) s = "...";
			statusLabel.Text = s;
		}
		
		void ColourLoversBrowserLoad(object sender, EventArgs e)
		{
			searchTypeComboBox.SelectedIndex = 2;
		}
		
		void SearchButtonClick(object sender, EventArgs e)
		{
			if(currentRequest!=null) return;
			string uri = "http://www.colourlovers.com/api/palettes";
			switch(searchTypeComboBox.SelectedIndex) {
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
			Dictionary<string, string> getKeys = new Dictionary<string, string>();
			if(searchKeywordsBox.Text.Trim() != "") getKeys["keywords"] = searchKeywordsBox.Text.Trim();
			getKeys["format"] = "xml";
			getKeys["numResults"] = "40";
			uri += "?";
			foreach(KeyValuePair<string, string> kv in getKeys) uri += kv.Key+"="+HttpUtility.UrlEncode(kv.Value)+"&";
			System.Diagnostics.Debug.Print(uri);
			
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
			setCurrentRequest(req);
			BackgroundWorker bw = new BackgroundWorker();
			bw.DoWork += delegate(object bworker, DoWorkEventArgs dwea) { 
				BackgroundWorker worker = (BackgroundWorker)bworker;
				worker.ReportProgress(10, (object)"Connecting...");
				WebResponse resp = currentRequest.GetResponse();
				Stream respStream = resp.GetResponseStream();
				worker.ReportProgress(10, (object)"Parsing...");
				XPathDocument xpd = new XPathDocument(respStream);
				respStream.Close();
				currentResultSet = xpd;
				worker.ReportProgress(10, (object)"Finished.");
			};
			bw.ProgressChanged += delegate(object bworker, ProgressChangedEventArgs evt) { 
				setStatus(evt.UserState as string);
			};
			bw.RunWorkerCompleted += UpdateResultsList;
			bw.WorkerReportsProgress = true;			
			bw.RunWorkerAsync();
			
		}
		
		void UpdateResultsList(object sender, RunWorkerCompletedEventArgs e)
		{
			UpdateResultsList();
		}
		void UpdateResultsList()
		{
			
			parsePalettesXml(currentResultSet);
			setStatus(String.Format("Search complete. {0} results.", palettes.Count));
			resultListBox.Items.Clear();
			resultListBox.Items.AddRange(palettes.ToArray());
			setCurrentRequest(null);
		}
		
		
		
		void parsePalettesXml(XPathDocument doc)
		{
			XPathNavigator xn = doc.CreateNavigator();
			xn.MoveToRoot();
			palettes.Clear();
			XPathNodeIterator paletteNodes = xn.Select("//palette");
			while(paletteNodes.MoveNext()){
				CPalette pal = new CPalette();
				pal.PopulateFromXPathNode(paletteNodes.Current);
				palettes.Add(pal);
			}
		}
		
		void drawSpanStrings(Graphics target, PointF origin, params object[] args)
		{
			for(int i=0;i<args.Length;i+=3)
			{
				Font f = (Font)args[i];
				Brush b = (Brush)args[i+1];
				String s = (String)args[i+2];
				if(s != "")
				{
					int width = (int)Math.Ceiling(target.MeasureString(s, f).Width - 1.5);
					target.DrawString(s, f, b, origin);
					origin.X += width;
				}
			}
		}
		
		
		void ResultListBoxDrawItem(object sender, DrawItemEventArgs e)
		{
			ListBox lb = (ListBox)sender;
			Graphics g = e.Graphics;
			Font font = lb.Font;
			Font boldFont = new Font(font.Name, font.Size, FontStyle.Bold);
			int fh = (int)font.GetHeight(g);
			e.DrawBackground();
			if(e.Index<0) return;
			CPalette pal = (CPalette)lb.Items[e.Index];
			
			
			drawSpanStrings(
				g, new PointF(e.Bounds.X, e.Bounds.Y),
			    boldFont,	Brushes.Black,	pal.Title,
			    font,		Brushes.Gray,	"by ",
			    font,		Brushes.Black,	pal.UserName
			);
			drawSpanStrings(
				g, new PointF(e.Bounds.X, e.Bounds.Y + fh ),
				font,		Brushes.Green,	String.Format("{0} views", pal.NumViews),
				font,		Brushes.Red,	pal.GetHeartsStr()
			);
			int y0 = e.Bounds.Y + fh * 2 + 2;
			int y1 = e.Bounds.Bottom - 3;
			int x0 = e.Bounds.X + 3;
			int x1 = e.Bounds.Right - 3;
			int w = x1-x0;
			int h = y1-y0;
			Color[] colors = pal.Colors.ToArray();
			int sw = w / colors.Length;
			for(int i=0;i<colors.Length;i++)
			{
				int xx0 = x0 + sw * i;
				g.FillRectangle(new SolidBrush(colors[i]), new RectangleF(xx0, y0, sw, h));
			}
			g.DrawRectangle(Pens.Black, new Rectangle(x0, y0, w, h));
			
			
			
		}
		
		void ContextMenuStrip1Opening(object sender, CancelEventArgs e)
		{
			foreach(ToolStripItem item in ((ContextMenuStrip)sender).Items) item.Enabled = (resultListBox.SelectedIndex>=0);
			System.Diagnostics.Debug.Print("Debug: {0}", sender);
			
		}
		
		CPalette GetCurrentlySelectedPalette()
		{
			if(resultListBox.SelectedIndex<0) return null;
			return (CPalette)resultListBox.SelectedItem;
		}
		
		void VisitPaletteURLToolStripMenuItemClick(object sender, EventArgs e)
		{
			CPalette pal = GetCurrentlySelectedPalette();
			if(pal!=null) System.Diagnostics.Process.Start(pal.Url);
		}
		
		void ApplyToolStripMenuItemClick(object sender, EventArgs e)
		{
			CPalette pal = GetCurrentlySelectedPalette();
			if(pal==null) return;
			if(selectedDelegate != null) {
				selectedDelegate(pal);
			}
			
		}
		
		void CopyColorsToolStripMenuItemClick(object sender, EventArgs e)
		{
			CPalette pal = GetCurrentlySelectedPalette();
			if(pal==null) return;
			string descStr = pal.GetColorDescStr();
			Clipboard.SetText(descStr);
			MessageBox.Show("Palette colors copied onto clipboard.", "Copied.", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		
		void ResultListBoxResize(object sender, EventArgs e)
		{
			resultListBox.Update();
		}
		
		void SearchTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			string sel = searchTypeComboBox.SelectedText;
			searchKeywordsBox.Enabled = (sel == "Search");
		}
	}
}
