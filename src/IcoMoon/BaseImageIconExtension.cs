using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IcoMoon.Forms
{
	[ContentProperty ("Name")]
	public abstract class BaseImageIconExtension<S, T> : IMarkupExtension where T : struct where S : BaseIconImageSource<T>, new()

	{
		public string Name { get; set; }
		public float Size { get; set; }
		public string Color { get; set; }

		private readonly ColorTypeConverter _colorConverter = new ColorTypeConverter();
		#region IMarkupExtension implementation

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			T icon;

			if (null != Name && Enum.TryParse(Name, out icon)) {
				var source = new S { 
					Icon = icon,
				};

				if (Size > 0) {
					source.Size = Size;
				}

				if (!string.IsNullOrWhiteSpace (Color)) {
					source.Color = (Color)_colorConverter.ConvertFrom (Color);
				}

				return source;
			}

			return null;
		}

		#endregion
	}
	
}
