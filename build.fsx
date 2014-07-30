// include Fake lib
#I "tools/FAKE/tools"
#r @"FakeLib.dll"


open System
open System.IO

open Fake
open Fake.FileUtils
open Fake.ProcessHelper

#I "tools"
#load "XamarinHelper.fsx"

open Fake.XamarinHelper

exception Exited of int
let sh command args =
    let result =
        ExecProcess (fun info ->
            info.FileName <- command
            info.Arguments <- args
        ) TimeSpan.MaxValue

    if result <> 0 then raise (Exited result)

let buildDir = "build"
let packageDir = buildDir @@ "package"

let mdtool (cfg, prj, sln) =
    let result =
      ExecProcess (fun info ->
        info.FileName <- "/Applications/Xamarin Studio.app/Contents/MacOS/mdtool"
        info.Arguments <- (sprintf "-v build '--configuration:%s' -p:%s %s" cfg prj sln)
      ) (System.TimeSpan.MaxValue)

    if result <> 0 then failwithf "mdtool failed with a non-zero exit code."

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir]
)

Target "BuildiOS" (fun _ ->
   mdtool ("Release", "IcoMoon.iOS", "IcoMoon.sln")
   CopyDir buildDir ("src" @@ "IcoMoon.iOS" @@ "bin" @@ "Release") allFiles
)

Target "BuildBuild" (fun _ ->
    !!("src" @@ "IcoMoon.Build" @@ "*.csproj")
    |> MSBuildDebug buildDir "Build"
    |> Log "BuildBuild-Output: "
)


let version = "0.0.1.3"

Target "CreateNugetPackage" (fun _ ->
    let pcl = "portable-win+net45+wp80+MonoAndroid10+MonoTouch10"
    let monotouch = "MonoTouch10"

    let libDir = packageDir @@ "lib"
    let monoTouchDir = libDir @@ monotouch
    let pclDir = libDir @@ pcl

    CleanDirs [packageDir;libDir;monoTouchDir;pclDir;(packageDir @@ "build" @@ pcl);(packageDir @@ "content" @@ pcl);(packageDir @@ "content" @@ monotouch)]

    //let libfiles = !!(touchProjectDir @@ "bin" @@ "Release" @@ "*.dll")

    CopyFile monoTouchDir (buildDir @@ "IcoMoon.iOS.dll")
    CopyFile pclDir (buildDir @@ "IcoMoon.dll")
    CopyFile (packageDir @@ "build" @@ pcl) (buildDir @@ "IcoMoon.Build.dll")
    CopyFile (packageDir @@ "build" @@ pcl) (buildDir @@ "Newtonsoft.Json.dll")
    CopyFile (packageDir @@ "content" @@ pcl) ("src" @@ "Templates" @@ "IcoMoon.tt.pp")
    CopyFile (packageDir @@ "content" @@ monotouch) ("src" @@ "Templates" @@ "IcoMoon.Platform.iOS.tt.pp")

    let files = [
      (@"lib" @@ monotouch @@ "IcoMoon.iOS.dll", Some(@"lib" @@ monotouch @@ "IcoMoon.iOS.dll"), None)
      (@"lib" @@ pcl @@ "IcoMoon.dll", Some(@"lib" @@ monotouch @@ "IcoMoon.dll"), None)
      (@"lib" @@ pcl @@ "IcoMoon.dll", Some(@"lib" @@ pcl @@ "IcoMoon.dll"), None)
      (@"build" @@ pcl @@ "IcoMoon.Build.dll", Some(@"build" @@ pcl @@ "IcoMoon.Build.dll"), None)
      (@"build" @@ pcl @@ "Newtonsoft.Json.dll", Some(@"build" @@ pcl @@ "Newtonsoft.Json.dll"), None)
      (@"content" @@ pcl @@ "IcoMoon.tt.pp", Some(@"content" @@ pcl @@ "IcoMoon.tt.pp"), None)
      (@"content" @@ monotouch @@ "IcoMoon.Platform.iOS.tt.pp", Some(@"content" @@ monotouch @@ "IcoMoon.Platform.iOS.tt.pp"), None)
    ]

    NuGet(fun p ->
        {p with
            Authors = ["Matthew Hail"]
            Project = "IcoMoon.Forms"
            Description = "IcoMoon for Xamarin Forms"

            WorkingDir = packageDir
            OutputPath = packageDir

            Version = version
            Publish = false

            Files = files
            Dependencies = [
              "Xamarin.Forms", GetPackageVersion "./packages/" "Xamarin.Forms"
              //"Newtonsoft.Json", GetPackageVersion "./packages/" "Newtonsoft.Json"
            ]

        }) "template.nuspec"
)

Target "Default" DoNothing

"Clean"
==> "BuildiOS"
==> "BuildBuild"
==> "CreateNugetPackage"
==> "Default"

RunTargetOrDefault "Default"
