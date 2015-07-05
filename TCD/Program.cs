/*
 * Date: 7.7.2010
 * Time: 19:47
 */

using System;
using System.Windows.Forms;

namespace TCD
{
	/// <summary>
	///     Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		///     Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}