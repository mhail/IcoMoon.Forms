﻿<#@ template language="C#" debug="true" hostspecific="true" #>
<#@ assembly name="System.Core" #>
<#
	var ns = "$rootnamespace$";
#>

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using IcoMoon.Forms.Platform.iOS;

using <#= ns.Replace(".iOS", string.Empty) #>;
using <#= ns #>;

[assembly: ExportImageSourceHandler(typeof(IconImageSource), typeof(IconImageSourceRenderer))]

namespace <#= ns #>
{
	public class IconImageSourceRenderer : BaseIconImageSourceRenderer<IconImageSource, Icons>
	{
	}
}
