using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace IcoMoon.Forms
{
	// Interface for rendering icons on a specific platform
	public interface IIconRenderer<T>
	{
		Stream GetStream (T icon, float size, Color color);
	}

	// Helper class for cerating and imagesource from an icon
	public static class IconSource
	{
		public const float DefaultSize = 20.0f;
		public static readonly Color DefaultColor = Color.Black;

		public static ImageSource ToImageSource<T> (this T icon, float size = DefaultSize, Color? color = null)
		{
			return ImageSource.FromStream(() => {
				// Load the renderer from the platform DependancyService
				var renderer = DependencyService.Get<IIconRenderer<T>> (DependencyFetchTarget.GlobalInstance);
				// render the icon
				var s = renderer.GetStream( icon, size, color.GetValueOrDefault(DefaultColor) );
				return s;
			});
		}
	}
	
}
