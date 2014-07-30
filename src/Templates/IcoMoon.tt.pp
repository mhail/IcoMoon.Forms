<#@ template language="C#" debug="true" hostspecific="true" #>
<#@ assembly name="System.Core" #>
<#@ assembly name="$(SolutionDir)/packages/IcoMoon.Forms.0.0.1.3/build/portable-win+net45+wp80+MonoAndroid10+MonoTouch10/Newtonsoft.Json.dll" #>
<#@ assembly name="$(SolutionDir)/packages/IcoMoon.Forms.0.0.1.3/build/portable-win+net45+wp80+MonoAndroid10+MonoTouch10/IcoMoon.Build.dll" #>
<#@ import namespace="IcoMoon.Build" #>
<#
	var ns = "$rootnamespace$";
#>
namespace <#= ns #>
{
	using IcoMoon.Forms;
	using Xamarin.Forms.Xaml;

	<#= IcoMoonCodeGen.CreateFromSelectionJs(Host.ResolvePath("selection.json")) #>
	public class ImageIconExtension : BaseImageIconExtension<Icons>, IMarkupExtension
	{
  }
}
