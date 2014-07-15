using System;
using Xamarin.Forms;
using MonoTouch.UIKit;
using System.IO;
using MonoTouch.CoreText;
using MonoTouch.Foundation;
using System.Drawing;
using Xamarin.Forms.Platform.iOS;

namespace IcoMoon.Forms.Platform.iOS
{
	public abstract class BaseIconImageSourceRenderer<S, T> : IImageSourceHandler where T : struct where S : BaseIconImageSource<T>
	{
		private static bool _iconLoaded;
		private void LoadIconFont() {
			if (_iconLoaded)
				return;

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
					_iconLoaded = CTFontManager.RegisterGraphicsFont (font, out error); 
				}
			}
		}


		#region IImageSourceHandler implementation

		public async System.Threading.Tasks.Task<MonoTouch.UIKit.UIImage> LoadImageAsync (Xamarin.Forms.ImageSource imagesource, System.Threading.CancellationToken cancelationToken = default(System.Threading.CancellationToken), float scale = 1f)
		{
			LoadIconFont ();

			var iconSource = imagesource as S;

			var color = iconSource.Color.ToUIColor();

			var fontAwesome = UIFont.FromName("icomoon", iconSource.Size);
			string c = char.ConvertFromUtf32 ((int)(object)iconSource.Icon);
			NSString str = new NSString(c);
			var imgSize = str.StringSize (fontAwesome);
			UIImage image = null;

			try{
				UIGraphics.BeginImageContextWithOptions (imgSize, false, 0.0f);
				color.SetFill();
				str.DrawString (PointF.Empty, fontAwesome);
				image = UIGraphics.GetImageFromCurrentImageContext ();
			} finally{
				UIGraphics.EndImageContext ();
			}
			return image;
		}

		#endregion


	}
}

