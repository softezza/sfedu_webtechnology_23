#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;

namespace SmartTwin.NoesisGUI.Editor
{
	/// <summary>
	/// Окно создания -blend решения модуля в контексте WPF
	/// </summary>
	public class SmartTwinNoesisMenuBlend : EditorWindow
	{
		string projectName = string.Empty;


		private void OnGUI()
		{
			projectName = EditorGUILayout.TextField("Project Name", projectName);

			if (GUILayout.Button("Create"))
			{
				var projectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
				//var projectPath = Path.GetDirectoryName(Application.dataPath);

				if (!File.Exists(Path.Combine(projectPath, projectName + "-blend.sln")))
				{
					CreateProject(projectName, projectPath);
					AssetDatabase.Refresh();
				}
			}
		}


		private void CreateProject(string projectName, string projectPath)
		{
			var solutionGuid = Guid.NewGuid().ToString().ToUpper();
			var projectGuid = Guid.NewGuid().ToString().ToUpper();

			CreateBlendSolution(projectName, projectPath, projectGuid, solutionGuid);
			CreateBlendProject(projectName, projectPath, projectGuid);
		}

		private void CreateBlendSolution(string projectName, string projectPath, string projectGuid, string solutionGuid)
		{
			using (var writer = File.CreateText(Path.Combine(projectPath, projectName + "-blend.sln")))
			{

				writer.WriteLine("Microsoft Visual Studio Solution File, Format Version 12.00");
				writer.WriteLine("# Blend for Visual Studio Version 16");
				writer.WriteLine("VisualStudioVersion = 16.0.31729.503");
				writer.WriteLine("MinimumVisualStudioVersion = 10.0.40219.1");
				writer.WriteLine($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{projectName}-blend\", \"{projectName}-blend.csproj\", \"{{{projectGuid}}}\"");
				writer.WriteLine("EndProject");
				writer.WriteLine("Global");
				writer.WriteLine("\tGlobalSection(SolutionConfigurationPlatforms) = preSolution");
				writer.WriteLine("\t\tDebug|Any CPU = Debug|Any CPU");
				writer.WriteLine("\t\tRelease|Any CPU = Release|Any CPU");
				writer.WriteLine("\tEndGlobalSection");
				writer.WriteLine("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution");
				writer.WriteLine($"\t\t{{{projectGuid}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
				writer.WriteLine($"\t\t{{{projectGuid}}}.Debug|Any CPU.Build.0 = Debug|Any CPU");
				writer.WriteLine($"\t\t{{{projectGuid}}}.Release|Any CPU.ActiveCfg = Release|Any CPU");
				writer.WriteLine($"\t\t{{{projectGuid}}}.Release|Any CPU.Build.0 = Release|Any CPU");
				writer.WriteLine("\tEndGlobalSection");
				writer.WriteLine("\tGlobalSection(SolutionProperties) = preSolution");
				writer.WriteLine("\t\tHideSolutionNode = FALSE");
				writer.WriteLine("\tEndGlobalSection");
				writer.WriteLine("\tGlobalSection(ExtensibilityGlobals) = postSolution");
				writer.WriteLine($"\t\tSolutionGuid = {{{solutionGuid}}}");
				writer.WriteLine("\tEndGlobalSection");
				writer.WriteLine("EndGlobal");
			}
		}

		private void CreateBlendProject(string projectName, string projectPath, string projectGuid)
		{
			var safeProjectName = string.Join("", projectName.AsEnumerable().Select(c => char.IsLetter(c) || char.IsDigit(c) ? c.ToString() : "_"));

			using (var writer = File.CreateText(Path.Combine(projectPath, projectName + "-blend.csproj")))
			{
				writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine("<Project ToolsVersion=\"15.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">");
				writer.WriteLine("  <PropertyGroup>");
				writer.WriteLine("    <BaseIntermediateOutputPath>Blend~\\obj\\</BaseIntermediateOutputPath>");
				writer.WriteLine("    <OutputPath>Blend~\\bin\\$(Configuration)\\</OutputPath>");
				writer.WriteLine("  </PropertyGroup>");
				writer.WriteLine("  <Import Project=\"$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props\" Condition=\"Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')\" />");
				writer.WriteLine("  <PropertyGroup>");
				writer.WriteLine("    <Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>");
				writer.WriteLine("    <Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>");
				writer.WriteLine($"    <ProjectGuid>{{{projectGuid}}}</ProjectGuid>");
				writer.WriteLine("    <OutputType>Library</OutputType>");
				writer.WriteLine($"    <RootNamespace>{safeProjectName}</RootNamespace>");
				writer.WriteLine($"    <AssemblyName>{projectName}</AssemblyName>");
				writer.WriteLine("    <FrameworkVersion>v4.7.2</FrameworkVersion>");
				writer.WriteLine("    <FrameworkVersion Condition=\"'$(VisualStudioVersion)' == '15.0'\">v4.6.1</FrameworkVersion>");
				writer.WriteLine("    <TargetFrameworkVersion>$(FrameworkVersion)</TargetFrameworkVersion>");
				writer.WriteLine("    <FileAlignment>512</FileAlignment>");
				writer.WriteLine("    <ProjectTypeGuids>{60dc8134-eba5-43b8-bcc9-bb4bc16c2548};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>");
				writer.WriteLine("    <WarningLevel>4</WarningLevel>");
				writer.WriteLine("    <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>");
				writer.WriteLine("    <Deterministic>true</Deterministic>");
				writer.WriteLine("  </PropertyGroup>");
				writer.WriteLine("  <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">");
				writer.WriteLine("    <PlatformTarget>AnyCPU</PlatformTarget>");
				writer.WriteLine("    <DebugSymbols>true</DebugSymbols>");
				writer.WriteLine("    <DebugType>full</DebugType>");
				writer.WriteLine("    <Optimize>false</Optimize>");
				writer.WriteLine("    <DefineConstants>DEBUG;TRACE</DefineConstants>");
				writer.WriteLine("    <ErrorReport>prompt</ErrorReport>");
				writer.WriteLine("    <WarningLevel>4</WarningLevel>");
				writer.WriteLine("  </PropertyGroup>");
				writer.WriteLine("  <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">");
				writer.WriteLine("    <PlatformTarget>AnyCPU</PlatformTarget>");
				writer.WriteLine("    <DebugType>pdbonly</DebugType>");
				writer.WriteLine("    <Optimize>true</Optimize>");
				writer.WriteLine("    <DefineConstants>TRACE</DefineConstants>");
				writer.WriteLine("    <ErrorReport>prompt</ErrorReport>");
				writer.WriteLine("    <WarningLevel>4</WarningLevel>");
				writer.WriteLine("  </PropertyGroup>");
				writer.WriteLine("  <ItemGroup>");
				writer.WriteLine("    <Reference Include=\"System\" />");
				writer.WriteLine("    <Reference Include=\"System.Data\" />");
				writer.WriteLine("    <Reference Include=\"System.Xml\" />");
				writer.WriteLine("    <Reference Include=\"Microsoft.CSharp\" />");
				writer.WriteLine("    <Reference Include=\"System.Core\" />");
				writer.WriteLine("    <Reference Include=\"System.Xml.Linq\" />");
				writer.WriteLine("    <Reference Include=\"System.Data.DataSetExtensions\" />");
				writer.WriteLine("    <Reference Include=\"System.Net.Http\" />");
				writer.WriteLine("    <Reference Include=\"System.Xaml\">");
				writer.WriteLine("      <RequiredTargetFramework>4.0</RequiredTargetFramework>");
				writer.WriteLine("    </Reference>");
				writer.WriteLine("    <Reference Include=\"WindowsBase\" />");
				writer.WriteLine("    <Reference Include=\"PresentationCore\" />");
				writer.WriteLine("    <Reference Include=\"PresentationFramework\" />");
				writer.WriteLine("  </ItemGroup>");
				writer.WriteLine("  <ItemGroup>");
				writer.WriteLine("    <PackageReference Include=\"Noesis.GUI.Extensions\">");
				writer.WriteLine("      <Version>3.0.*</Version>");
				writer.WriteLine("    </PackageReference>");
				writer.WriteLine("  </ItemGroup>");
				writer.WriteLine("  <Import Project=\"$(MSBuildToolsPath)\\Microsoft.CSharp.targets\" />");
				writer.WriteLine("</Project>");
			}

			Directory.CreateDirectory(Path.Combine(projectPath, "Blend~"));
		}


		[MenuItem("Assets/Create/SmartTwin NoesisGUI/Create Module Blend Project", false)]
		private static void Init()
		{
			var window = (SmartTwinNoesisMenuBlend)EditorWindow.GetWindow<SmartTwinNoesisMenuBlend>();
			window.Show();
		}
	}
}
#endif