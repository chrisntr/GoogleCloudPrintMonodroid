
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.ServiceModel.Web;

namespace GoogleCloudPrint
{
	public class GoogleCloudPrint
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Source { get; set; }
		
		private const int ServiceTimeout = 10000;
		

		
		public CloudPrintJob PrintDocument (string printerId, string title, byte[] document, String mimeType)
		{
			try
			{
				string authCode;
				if (!Authorize (out authCode))
				return new CloudPrintJob { success = false };
				
				var b64 = Convert.ToBase64String (document);
				
				var request = (HttpWebRequest)WebRequest.Create ("http://www.google.com/cloudprint/submit?output=json&printerid=" + printerId);
				request.Method = "POST";
				
				// Setup the web request
				request.ServicePoint.Expect100Continue = false;
				
				// Add the headers
				request.Headers.Add ("X-CloudPrint-Proxy", Source);
				request.Headers.Add ("Authorization", "GoogleLogin auth=" + authCode);
				
				var p = new PostData ();
				
				p.Params.Add (new PostDataParam { Name = "printerid", Value = printerId, Type = PostDataParamType.Field });
				p.Params.Add (new PostDataParam { Name = "capabilities", Value = "{\"capabilities\":[{}]}", Type = PostDataParamType.Field });
				p.Params.Add (new PostDataParam { Name = "contentType", Value = "dataUrl", Type = PostDataParamType.Field });
				p.Params.Add (new PostDataParam { Name = "title", Value = title, Type = PostDataParamType.Field });
				
				p.Params.Add (new PostDataParam
				              {
					Name = "content",
					Type = PostDataParamType.Field,
					Value = "data:" + mimeType + ";base64," + b64
				});
				
				var postData = p.GetPostData ();

				
				byte[] data = Encoding.UTF8.GetBytes (postData);
				
				request.ContentType = "multipart/form-data; boundary=" + p.Boundary;
				
				Stream stream = request.GetRequestStream ();
				stream.Write (data, 0, data.Length);
				stream.Close ();
				
				// Get response
				var response = (HttpWebResponse)request.GetResponse ();
				var responseContent = new StreamReader (response.GetResponseStream ()).ReadToEnd ();
				
				var serializer = new DataContractJsonSerializer (typeof (CloudPrintJob));
				var ms = new MemoryStream (Encoding.Unicode.GetBytes (responseContent));
				var printJob = serializer.ReadObject (ms) as CloudPrintJob;
				
				return printJob;
			}
			catch (Exception ex)
			{
				return new CloudPrintJob { success = false, message = ex.Message };
			}
		}
		
		public CloudPrinters Printers
		{
			get
			{
				var printers = new CloudPrinters ();
				
				string authCode;
				if (!Authorize (out authCode))
				return new CloudPrinters { success = false };
				
				try
				{
					var request = (HttpWebRequest)WebRequest.Create ("http://www.google.com/cloudprint/search?output=json");
					request.Method = "POST";
					
					// Setup the web request
					request.ServicePoint.Expect100Continue = false;
					
					// Add the headers
					request.Headers.Add ("X-CloudPrint-Proxy", Source);
					request.Headers.Add ("Authorization", "GoogleLogin auth=" + authCode);
					
					request.ContentType = "application/x-www-form-urlencoded";
					request.ContentLength = 0;
					
					var response = (HttpWebResponse)request.GetResponse ();
					var responseContent = new StreamReader (response.GetResponseStream ()).ReadToEnd ();
					
					var serializer = new DataContractJsonSerializer (typeof (CloudPrinters));
					var ms = new MemoryStream (Encoding.Unicode.GetBytes (responseContent));
					printers = serializer.ReadObject (ms) as CloudPrinters;
					
					return printers;
				}
				catch (Exception)
				{
					return printers;
				}
			}
		}
		
		private bool Authorize (out string authCode)
		{
			var result = false;
			authCode = "";
			
			var queryString = String.Format ("https://www.google.com/accounts/ClientLogin?accountType=HOSTED_OR_GOOGLE&Email={0}&Passwd={1}&service=cloudprint&source={2}",
			                                 UserName, Password, Source);
			var request = (HttpWebRequest)WebRequest.Create (queryString);
			
			request.ServicePoint.Expect100Continue = false;
			
			var response = (HttpWebResponse)request.GetResponse ();
			var responseContent = new StreamReader (response.GetResponseStream ()).ReadToEnd ();
			
			var split = responseContent.Split ('\n');
			foreach (var s in split)
			{
				var nvsplit = s.Split ('=');
				if (nvsplit.Length == 2)
				{
					if (nvsplit[0] == "Auth")
					{
						authCode = nvsplit[1];
						result = true;
					}
				}
			}
			
			return result;
		}

	}

	[DataContract]
	public class CloudPrinter
	{
		[DataMember (Order = 0)]
		public string id { get; set; }
		
		[DataMember (Order = 1)]
		public string name { get; set; }
		
		[DataMember (Order = 2)]
		public string description { get; set; }
		
		[DataMember (Order = 3)]
		public string proxy { get; set; }
		
		[DataMember (Order = 4)]
		public string status { get; set; }
		
		[DataMember (Order = 5)]
		public string capsHash { get; set; }
		
		[DataMember (Order = 6)]
		public string createTime { get; set; }
		
		[DataMember (Order = 7)]
		public string updateTime { get; set; }
		
		[DataMember (Order = 8)]
		public string accessTime { get; set; }
		
		[DataMember (Order = 9)]
		public bool confirmed { get; set; }
		
		[DataMember (Order = 10)]
		public int numberOfDocuments { get; set; }
		
		[DataMember (Order = 11)]
		public int numberOfPages { get; set; }
	}

	[DataContract]
	public class CloudPrinters
	{
		[DataMember (Order = 0)]
		public bool success { get; set; }
		
		[DataMember (Order = 1)]
		public List<CloudPrinter> printers { get; set; }
	}



	[DataContract]
	public class CloudPrintJob
	{
		[DataMember (Order = 0)]
		public bool success { get; set; }
		
		[DataMember (Order = 1)]
		public string message { get; set; }
	}



	internal class PostData
	{
		private const String CRLF = "\r\n";
		
		public string Boundary { get; set; }
		private List<PostDataParam> _mParams;
		
		public List<PostDataParam> Params
		{
			get { return _mParams; }
			set { _mParams = value; }
		}
		
		public PostData ()
		{
			// Get boundary, default is --AaB03x
			Boundary = "----CloudPrintFormBoundary" + DateTime.UtcNow;
			
			// The set of parameters
			_mParams = new List<PostDataParam> ();
		}
		
		public string GetPostData ()
		{
			var sb = new StringBuilder ();
			foreach (var p in _mParams)
			{
				sb.Append ("--" + Boundary).Append (CRLF);
				
				if (p.Type == PostDataParamType.File)
				{
					sb.Append (string.Format ("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", p.Name, p.FileName)).Append (CRLF);
					sb.Append ("Content-Type: ").Append (p.FileMimeType).Append (CRLF);
					sb.Append ("Content-Transfer-Encoding: base64").Append (CRLF);
					sb.Append ("").Append (CRLF);
					sb.Append (p.Value).Append (CRLF);
				}
				else
				{
					sb.Append (string.Format ("Content-Disposition: form-data; name=\"{0}\"", p.Name)).Append (CRLF);
					sb.Append ("").Append (CRLF);
					sb.Append (p.Value).Append (CRLF);
				}
			}
			
			sb.Append ("--" + Boundary + "--").Append (CRLF);
			
			return sb.ToString ();
		}
	}
	
	public enum PostDataParamType
	{
		Field,
		File
	}
	
	public class PostDataParam
	{
		public string Name { get; set; }
		public string FileName { get; set; }
		public string FileMimeType { get; set; }
		public string Value { get; set; }
		public PostDataParamType Type { get; set; }
		
		public PostDataParam ()
		{
			FileMimeType = "text/plain";
		}
	}
}