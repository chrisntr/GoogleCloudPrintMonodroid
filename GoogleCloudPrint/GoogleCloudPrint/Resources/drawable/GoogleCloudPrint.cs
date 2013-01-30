using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Webkit;
//using Android.Widget;
//using System.IO;
//using Java.IO;
//using Java.Interop;
//
//namespace GoogleCloudPrint
//{
//	[Activity (Label = "Google Cloud Print")]
//	public class PrintActivity : Activity
//	{
//		private static string PRINT_DIALOG_URL = "https://www.google.com/cloudprint/dialog.html";
//		private static string JS_INTERFACE = "AndroidPrintDialog";
//		private static string CONTENT_TRANSFER_ENCODING = "base64";
//		
//		private static string ZXING_URL = "http://zxing.appspot.com";
//		private static int ZXING_SCAN_REQUEST = 65743;
//
//		private static string CLOSE_POST_MESSAGE_NAME = "cp-dialog-on-close";
//
//		private WebView webView;
//		
//		protected override void OnCreate (Bundle savedInstanceState)
//		{
//			base.OnCreate (savedInstanceState);
//
//			var frame = new FrameLayout(this);
//			webView = new WebView(this);
//			webView.Settings.JavaScriptEnabled = true;
//			webView.SetWebViewClient(new PrintDialogWebClient(this.Intent,this));
//			webView.AddJavascriptInterface(new PrintDialogJavaScriptInterface(this), JS_INTERFACE);
//			webView.LoadUrl(PRINT_DIALOG_URL);
//			frame.AddView(webView);
//
//			this.SetContentView(frame);
//	         
//		}
//		
//		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
//		{
//			if (requestCode == ZXING_SCAN_REQUEST && resultCode == Result.Ok)
//				webView.LoadUrl(Intent.GetStringExtra("SCAN_RESULT"));
//		}
//
//		public class PrintDialogJavaScriptInterface : Java.Lang.Object
//		{
//			private Activity _activity;
//			private string _type;
//			private string _title;
//			private Android.Net.Uri _uri;
//			
//			public PrintDialogJavaScriptInterface(Activity activity)
//			{
//				_activity = activity;
//				_type = activity.Intent.Type;
//				_title = activity.Intent.GetStringExtra("title");
//				_uri = activity.Intent.Data;
//			}
//		
//		
//	
//			public string getEncoding()
//			{
//				return CONTENT_TRANSFER_ENCODING;
//			}
//			
//			public void onPostMessage(string msg)
//			{
//				//if (msg.StartsWith(CLOSE_POST_MESSAGE_NAME))
//				//	_activity.Finish();
//			}
//		}
//		
//		public class PrintDialogWebClient : WebViewClient
//		{
//			private Activity _activity;
//			private string _type;
//			private string _title;
//			private Android.Net.Uri _uri;
//			public PrintDialogWebClient(Intent intent,Activity activity)
//			{
//				_activity = activity;
//				_type = intent.Type;
//				_title = intent.GetStringExtra("title");
//				_uri = intent.Data;
//			}
//			public string getType()
//			{
//				return _type;
//			}
//			
//			public string getTitle()
//			{
//				return _title;
//			}
//			public string getEncoding()
//			{
//				return CONTENT_TRANSFER_ENCODING;
//			}
//			public string getContent()
//			{
//				try
//				{
//
//					System.IO.Stream inStream = (Stream) _activity.Assets.Open("gs.pdf");
//					System.IO.MemoryStream outStream = new System.IO.MemoryStream();
//					
//					byte[] buffer = new byte[4096];
//					int n = inStream.Read(buffer, 0, buffer.Length);
//					while (n > 0)
//					{
//						outStream.Write(buffer, 0, n);
//						n = inStream.Read(buffer, 0, buffer.Length);
//					}
//					
//					inStream.Close();
//					
//					return Base64.EncodeToString(outStream.ToArray(), Android.Util.Base64Flags.Default);
//				}
//				catch (Java.IO.FileNotFoundException e)
//				{
//					e.PrintStackTrace();
//				}
//				catch (Java.IO.IOException e)
//				{
//					e.PrintStackTrace();
//				}
//				
//				return "";
//			}
//		
//			
//			protected static string EscapeJsString (String s)
//			{
//				if (s == null) {
//					return "";
//				}
//				
//				return s.Replace ("'", "\\'").Replace ("\"", "\\\"");
//			}
//			
//			public override void OnPageFinished (WebView view, string url)
//			{
//
//				if (PRINT_DIALOG_URL == url)
//				{
//								
//
//					//Submit print document
//					string newUrl = "javascript:printApp.setPrintDocument(printApp.createPrintDocument(";
//					newUrl +=  getType()+","+getTitle()+","+getContent()+","+getEncoding()+"))";
//					view.LoadUrl(newUrl);
//					
//
//				}
//
//			}
//		}
//
//	}
//}




