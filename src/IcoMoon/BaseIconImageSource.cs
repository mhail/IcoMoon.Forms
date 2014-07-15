using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IcoMoon.Forms
{
	public abstract class BaseIconImageSource<T> : StreamImageSource where T : struct
	{
		public static readonly BindableProperty IconProperty = BindableProperty.Create<BaseIconImageSource<T>, T> (v => v.Icon, default(T));

		public T Icon { 
			get { 
				return (T)base.GetValue (IconProperty);
			} 
			set { 
				base.SetValue (IconProperty, value);
			} 
		}

		public static readonly BindableProperty SizeProperty = BindableProperty.Create<BaseIconImageSource<T>, float> (v => v.Size, 20.0f);

		public float Size { 
			get { 
				return (float)base.GetValue (SizeProperty);
			} 
			set { 
				base.SetValue (SizeProperty, value);
			} 
		}

		public static readonly BindableProperty ColorProperty = BindableProperty.Create<BaseIconImageSource<T>, Color> (v => v.Color, Color.Black);

		public Color Color { 
			get { 
				return (Color)base.GetValue (ColorProperty);
			} 
			set { 
				base.SetValue (ColorProperty, value);
			} 
		}
	}
}

