
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GoogleCloudPrint;
using Android.Net;
using System.IO;
using Android.Util;


namespace GoogleCloudPrint
{
	[Activity (Label = "Main", MainLauncher = true)]			
	public class Main : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.SetContentView(Resource.Layout.Main);
			Button button = FindViewById<Button>(Resource.Id.mybutton);
			button.Click += delegate 
			{
				GoogleCloudPrint cloudprint = new GoogleCloudPrint();
				//Modify to use your email address for Google Print.
				cloudprint.UserName = "gmailaccountemailaddress";
				//Modify to use your password for Google Print.
			    cloudprint.Password = "Password";
				CloudPrinters cp = cloudprint.Printers;
				Stream s = Assets.Open("gs.pdf");
				MemoryStream ms = new MemoryStream();
				CopyStream(s,ms);
				//Modify to add Printer Name.
				CloudPrinter printer = cp.printers.Where(t => t.name.ToLower().Contains("printername")).First();
				cloudprint.PrintDocument(printer.id,"GS Payscale",ms.ToArray(),"application/pdf");

			};
		}
		public static void CopyStream(Stream input, Stream output)
		{
			byte[] buffer = new byte[16*1024];
			int read;
			while((read = (input.Read(buffer, 0, buffer.Length))) > 0) output.Write (buffer, 0, read);
	     }
	    

	}
}

