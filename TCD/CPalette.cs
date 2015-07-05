/*
 * Date: 21.12.2009
 * Time: 21:28
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.IO;
using System.Xml.XPath;

namespace TCD
{
	/// <summary>
	/// Description of ColourLoversPalette.
	/// </summary>
	public class CPalette
	{
		List<Color> colors;
		int id = 0;
		string title = "";
		string userName = "";
		int numViews = 0;
		int numVotes = 0;
		int numComments = 0;
		double numHearts = 0;
		int rank = 0;
		DateTime dateCreated;
		string description = "";
		string url = "";
		string imageUrl = "";
		string badgeUrl = "";
		string apiUrl = "";
		
		public List<Color> Colors {
			get { return colors; }
		}
	
		public int Id {
			get { return id; }
		}
		
		public string Title {
			get { return title; }
		}
		
		public string UserName {
			get { return userName; }
		}
		
		public int NumViews {
			get { return numViews; }
		}
		
		public int NumVotes {
			get { return numVotes; }
		}
		
		public int NumComments {
			get { return numComments; }
		}
		
		public double NumHearts {
			get { return numHearts; }
		}
		
		public int Rank {
			get { return rank; }
		}
		
		public DateTime DateCreated {
			get { return dateCreated; }
		}
		
		public string Description {
			get { return description; }
		}
		
		public string Url {
			get { return url; }
		}
		
		public string ImageUrl {
			get { return imageUrl; }
		}
		
		public string BadgeUrl {
			get { return badgeUrl; }
		}
		
		public string ApiUrl {
			get { return apiUrl; }
		}
		
		
		public CPalette()
		{
			colors = new List<Color>(5);
		}
		
		public void ReadGPLStream(Stream s) {
			using(StreamReader sr = new StreamReader(s, Encoding.UTF8)) {
				bool markFound = false;
				while(s.CanRead) {
					string line = sr.ReadLine();
					if(line == null) break;
					line = line.TrimEnd();
					if(!markFound) {
						if(line.StartsWith("Name:")) title = line.Substring(5).Trim();
						else if(line == "#") markFound = true;
						continue;
					}
					string[] parts = line.Split(null);
					colors.Add(Color.FromArgb(Convert.ToInt16(parts[0]), Convert.ToInt16(parts[1]), Convert.ToInt16(parts[2])));
				}
			}
		}
		
		public void WriteGPLStream(Stream s) {
			using(StreamWriter sw = new StreamWriter(s, Encoding.UTF8)) {
				sw.WriteLine("GIMP Palette");
				sw.WriteLine("Name: " + (title.Length > 0 ? title : "Untitled"));
				sw.WriteLine("Columns: 3");
				sw.WriteLine("#");
				foreach(Color c in colors) {
					sw.WriteLine("{0}\t{1}\t{2}\tColor", c.R, c.G, c.B);
				}
				sw.Flush();
			}
		}
		
		private static string getXPathExprContent(XPathNavigator node, string expr, string defaultValue)
		{
			XPathNodeIterator xpni = node.Select(expr);
			if(xpni.Count == 0) return defaultValue;
			xpni.MoveNext();
			return xpni.Current.Value;
		}
		
		public void PopulateFromXPathNode(XPathNavigator node)
		{
			//System.Diagnostics.Debug.Print("X = {0}", node.OuterXml);
			id = Convert.ToInt32(getXPathExprContent(node, "./id", "0"));
			title = getXPathExprContent(node, "./title", "(unknown title)");
			userName = getXPathExprContent(node, "./userName", "(unknown user)");
			numViews = Convert.ToInt32(getXPathExprContent(node, "./numViews", "-1"));
			numVotes = Convert.ToInt32(getXPathExprContent(node, "./numVotes", "-1"));
			numComments = Convert.ToInt32(getXPathExprContent(node, "./numComments", "-1"));
			numHearts = Double.Parse(getXPathExprContent(node, "./numHearts", "-1"), new CultureInfo("en-US", false));
			rank = Convert.ToInt32(getXPathExprContent(node, "./rank", "-1"));
			dateCreated = DateTime.Parse(getXPathExprContent(node, "./dateCreated", "1970-01-01 00:00:00"));
			description = getXPathExprContent(node, "./description", "");
			url = getXPathExprContent(node, "./url", "");
			imageUrl = getXPathExprContent(node, "./imageUrl", "");
			badgeUrl = getXPathExprContent(node, "./badgeUrl", "");
			apiUrl = getXPathExprContent(node, "./apiUrl", "");
			XPathNodeIterator colorNodes = node.Select("./colors/hex");
			while(colorNodes.MoveNext()) colors.Add(ColorTranslator.FromHtml("#"+colorNodes.Current.Value));
			colors.TrimExcess();
		}
		
		public string GetColorDescStr()
		{
			string colorsDesc = "";
			foreach(Color c in colors) colorsDesc += ColorTranslator.ToHtml(c) + " ";
			return colorsDesc.TrimEnd();
		}
		
		public override string ToString()
		{
			return string.Format("[#{1}/{2} by {3} - {4}: ({0})]", GetColorDescStr(), this.id, this.title, this.userName, this.url);
		}
		
		public string GetHeartsStr()
		{
			if(numHearts<0) return "?";
			string heartsStr = "";
			int entireHearts = (int)Math.Floor(numHearts);
			bool hasHalfHeart = ((numHearts - entireHearts) > 0);
			while(entireHearts-- > 0) heartsStr += "\u2665";
			if(hasHalfHeart) heartsStr += "\u00BD";
			return heartsStr;
		}
		
	}
}
