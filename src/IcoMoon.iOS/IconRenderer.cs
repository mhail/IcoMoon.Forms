using System;
using IcoMoon.Forms;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.CoreText;
using MonoTouch.UIKit;
using System.Drawing;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace IcoMoon.Forms.Platform.iOS
{
	public abstract class IconRenderer<T> : IIconRenderer<T> where T : struct
	{
		#region IIconRenderer implementation

		public Stream GetStream (T icon, float size, Color color)
		{
			NSData data;
			using (var image = DrawIcon (icon, size, color)) {
				data = image.AsPNG ();
			}
			return data.AsStream ();
		}

		#endregion

		static IconRenderer()
		{
			var assembly = typeof(T).Assembly;

			using (var stream = assembly.GetManifestResourceStream(typeof(T), "icomoon.ttf")) {
				if (null == stream) {
					throw new InvalidOperationException ("icomoon.ttf not found");
				}

				var data = new byte[stream.Length];
				stream.Read (data, 0, data.Length);
				using (var provider = new MonoTouch.CoreGraphics.CGDataProvider (data, 0, data.Length))
				using(var font = MonoTouch.CoreGraphics.CGFont.CreateFromProvider (provider))
				{
					NSError error;
					CTFontManager.RegisterGraphicsFont (font, out error); 
				}
			}
		}

		public static UIImage DrawIcon(T icon, float size, Color color)
		{
			var fontAwesome = UIFont.FromName("icomoon", size);
			string c = char.ConvertFromUtf32 ((int)(object)icon);
			NSString str = new NSString(c);
			var imgSize = str.StringSize (fontAwesome);
			UIImage image = null;

			try{
				UIGraphics.BeginImageContextWithOptions (imgSize, false, 0.0f);
				color.ToUIColor().SetFill();
				str.DrawString (PointF.Empty, fontAwesome);
				image = UIGraphics.GetImageFromCurrentImageContext ();
			} finally{
				UIGraphics.EndImageContext ();
			}
			return image;
		}
	}
}

