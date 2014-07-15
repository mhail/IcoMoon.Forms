using System;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace IcoMoonToEnum
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine( IcoMoon.Build.IcoMoonCodeGen.CreateFromSelectionJs("selection.json", "Foo") );

			//Console.WriteLine( IcoMoon.Build.IcoMoonCodeGen.CreateiOSRenderer ("Foo") );
		}
	}
}
