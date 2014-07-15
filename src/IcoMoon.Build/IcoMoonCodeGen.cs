using System;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace IcoMoon.Build
{
	public static class IcoMoonCodeGen
	{
		public static string CreateFromSelectionJs(string path, string enumName = "Icons")
		{
			//var codeNs = new CodeNamespace(ns);


			var provider = new CSharpCodeProvider();
			var ctEnum = new System.CodeDom.CodeTypeDeclaration (enumName){
				IsEnum = true,
			};

			//codeNs.Types.Add (ctEnum);

			var json = File.ReadAllText (path);
			JObject o = JObject.Parse(json);
			foreach (var icon in o ["icons"].AsJEnumerable ()) {
				var p = icon ["properties"];
				var name = p ["name"].Value<string>();
				var code = p ["code"].Value<int> ();
				ctEnum.Members.Add (new CodeMemberField () {
					Name = provider.CreateValidIdentifier(name.Replace('-', '_')),
					InitExpression = new CodePrimitiveExpression (code),
				});
			}
			/*
			codeNs.Imports.Add (new CodeNamespaceImport ("IcoMoon.Forms"));

			var ctIconImageSource = new CodeTypeDeclaration ("IconImageSource") {
				IsClass = true,
			};

			var ctrBaseIconImageSource = new CodeTypeReference ("BaseIconImageSource<"+ctEnum.Name+">");
			ctIconImageSource.BaseTypes.Add (ctrBaseIconImageSource);

			var ctImageIconExtension = new CodeTypeDeclaration ("ImageIconExtension") { 
				IsClass = true,
			};

			var ctrBaseImageIconExtension = new CodeTypeReference (string.Format("BaseImageIconExtension<{0},{1}>", ctIconImageSource.Name,ctEnum.Name));

			ctImageIconExtension.BaseTypes.Add (ctrBaseImageIconExtension);

			codeNs.Types.Add (ctImageIconExtension);

			codeNs.Types.Add (ctIconImageSource);
			*/
			using (var w = new StringWriter ()) {

				provider.GenerateCodeFromType (ctEnum, w, new CodeGeneratorOptions { 

				});
				return w.ToString ();
			}
		}
		/*
		public static string CreateiOSRenderer(string ns, string enumName = "Icons")
		{
			var codeNs = new CodeNamespace(ns);
			var platformNs = new CodeNamespace (ns + ".iOS");

			var renderer = new CodeTypeDeclaration ("IconImageSourceRenderer");

			var ctrIconImageSource = new CodeTypeReference ("IconImageSource");

			var ctrBaseIconImageSourceRenderer = new CodeTypeReference (string.Format("BaseIconImageSourceRenderer<{0}, {1}>","IconImageSource", enumName));

			renderer.BaseTypes.Add (ctrBaseIconImageSourceRenderer);

			platformNs.Types.Add (renderer);

			var unit = new CodeCompileUnit ();

			unit.Namespaces.Add (platformNs);

			var attribute = new CodeAttributeDeclaration(
				new CodeTypeReference("ExportImageSourceHandler"));

			attribute.Arguments.Add (new CodeAttributeArgument (new CodeTypeOfExpression (ctrIconImageSource)));
			attribute.Arguments.Add (new CodeAttributeArgument (new CodeTypeOfExpression (renderer.Name)));

			unit.AssemblyCustomAttributes.Add (attribute);

			unit.StartDirectives.Add (new CodeNamespaceImport ("Xamarin.Forms"));

			var provider = new CSharpCodeProvider();

			using (var w = new StringWriter ()) {

				provider.GenerateCodeFromCompileUnit (unit, w, new CodeGeneratorOptions { 

				});
				return w.ToString ();
			}
		}
		*/
	}
}

