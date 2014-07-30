using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace IcoMoon.Forms
{
	[ContentProperty ("Name")]
	public abstract class BaseImageIconExtension<T> : IMarkupExtension where T : struct
	{
		public string Name { get; set; }
		public float Size { get; set; }
		public string Color { get; set; }

		private readonly ColorTypeConverter _colorConverter = new ColorTypeConverter();
		#region IMarkupExtension implementation

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			T icon;
			float size = Size > 0 ? Size : 20;
			Color color = Xamarin.Forms.Color.Black;

			if (null != Name && Enum.TryParse(Name, out icon)) {

				if (!string.IsNullOrWhiteSpace (Color)) {
					color = (Color)_colorConverter.ConvertFrom (Color);
				}

				return IconSource.ToImageSource (icon, size, color);
			}

			return null;
		}

		#endregion
	}
	
}
