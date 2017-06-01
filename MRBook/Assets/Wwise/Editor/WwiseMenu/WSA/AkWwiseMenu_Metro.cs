#if UNITY_EDITOR && !UNITY_5

using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System;


public class AkWwiseMenu_WSA : MonoBehaviour {

	private static AkUnityPluginInstaller_WSA m_installer = new AkUnityPluginInstaller_WSA();

	// private static AkUnityIntegrationBuilder_WSA m_builder = new AkUnityIntegrationBuilder_WSA();

	[MenuItem("Assets/Wwise/Install Plugins/WSA/Win32/Debug", false, (int)AkWwiseMenuOrder.WSAWin32Debug)]
	public static void InstallPlugin_WSA_Win32_Debug () {
		m_installer.InstallPluginByArchConfig("WSA_Win32", "Debug");
	}

	[MenuItem("Assets/Wwise/Install Plugins/WSA/Win32/Profile", false, (int)AkWwiseMenuOrder.WSAWin32Profile)]
	public static void InstallPlugin_WSA_Win32_Profile () {
		m_installer.InstallPluginByArchConfig("WSA_Win32", "Profile");
	}

	[MenuItem("Assets/Wwise/Install Plugins/WSA/Win32/Release", false, (int)AkWwiseMenuOrder.WSAWin32Release)]
	public static void InstallPlugin_WSA_Win32_Release () {
		m_installer.InstallPluginByArchConfig("WSA_Win32", "Release");
	}

	[MenuItem("Assets/Wwise/Install Plugins/WSA/ARM/Debug", false, (int)AkWwiseMenuOrder.WSAArmDebug)]
	public static void InstallPlugin_WSA_ARM_Debug () {
		m_installer.InstallPluginByArchConfig("WSA_ARM", "Debug");
	}

	[MenuItem("Assets/Wwise/Install Plugins/WSA/ARM/Profile", false, (int)AkWwiseMenuOrder.WSAArmProfile)]
	public static void InstallPlugin_WSA_ARM_Profile () {
		m_installer.InstallPluginByArchConfig("WSA_ARM", "Profile");
	}

	[MenuItem("Assets/Wwise/Install Plugins/WSA/ARM/Release", false, (int)AkWwiseMenuOrder.WSAArmRelease)]
	public static void InstallPlugin_WSA_ARM_Release () {
		m_installer.InstallPluginByArchConfig("WSA_ARM", "Release");
	}


    [MenuItem("Assets/Wwise/Install Plugins/WSA_WindowsPhone81/Win32/Debug", false, (int)AkWwiseMenuOrder.WSAWin32Debug)]
    public static void InstallPlugin_WSA_WindowsPhone81_Win32_Debug()
    {
        m_installer.InstallPluginByArchConfig("WSA_WindowsPhone81_Win32", "Debug");
    }

    [MenuItem("Assets/Wwise/Install Plugins/WSA_WindowsPhone81/Win32/Profile", false, (int)AkWwiseMenuOrder.WSAWin32Profile)]
    public static void InstallPlugin_WSA_WindowsPhone81_Win32_Profile()
    {
        m_installer.InstallPluginByArchConfig("WSA_WindowsPhone81_Win32", "Profile");
    }

    [MenuItem("Assets/Wwise/Install Plugins/WSA_WindowsPhone81/Win32/Release", false, (int)AkWwiseMenuOrder.WSAWin32Release)]
    public static void InstallPlugin_WSA_WindowsPhone81_Win32_Release()
    {
        m_installer.InstallPluginByArchConfig("WSA_WindowsPhone81_Win32", "Release");
    }

    [MenuItem("Assets/Wwise/Install Plugins/WSA_WindowsPhone81/ARM/Debug", false, (int)AkWwiseMenuOrder.WSAArmDebug)]
    public static void InstallPlugin_WSA_WindowsPhone81_ARM_Debug()
    {
        m_installer.InstallPluginByArchConfig("WSA_WindowsPhone81_ARM", "Debug");
    }

    [MenuItem("Assets/Wwise/Install Plugins/WSA_WindowsPhone81/ARM/Profile", false, (int)AkWwiseMenuOrder.WSAArmProfile)]
    public static void InstallPlugin_WSA_WindowsPhone81_ARM_Profile()
    {
        m_installer.InstallPluginByArchConfig("WSA_WindowsPhone81_ARM", "Profile");
    }

    [MenuItem("Assets/Wwise/Install Plugins/WSA_WindowsPhone81/ARM/Release", false, (int)AkWwiseMenuOrder.WSAArmRelease)]
    public static void InstallPlugin_WSA_WindowsPhone81_ARM_Release()
    {
        m_installer.InstallPluginByArchConfig("WSA_WindowsPhone81_ARM", "Release");
    }	  




//	[MenuItem("Assets/Wwise/Rebuild Integration/WSA/Win32/Debug")]
//	public static void RebuildIntegration_Debug_Win32() {
//		m_builder.BuildByConfig("Debug", "WSA_Win32");
//	}
//
//	[MenuItem("Assets/Wwise/Rebuild Integration/WSA/Win32/Profile")]
//	public static void RebuildIntegration_Profile_Win32() {
//		m_builder.BuildByConfig("Profile", "WSA_Win32");
//	}
//
//	[MenuItem("Assets/Wwise/Rebuild Integration/WSA/Win32/Release")]
//	public static void RebuildIntegration_Release_Win32() {
//		m_builder.BuildByConfig("Release", "WSA_Win32");
//	}
//
//	[MenuItem("Assets/Wwise/Rebuild Integration/WSA/ARM/Debug")]
//	public static void RebuildIntegration_Debug_ARM() {
//		m_builder.BuildByConfig("Debug", "WSA_ARM");
//	}
//
//	[MenuItem("Assets/Wwise/Rebuild Integration/WSA/ARM/Profile")]
//	public static void RebuildIntegration_Profile_ARM() {
//		m_builder.BuildByConfig("Profile", "WSA_ARM");
//	}
//
//	[MenuItem("Assets/Wwise/Rebuild Integration/WSA/ARM/Release")]
//	public static void RebuildIntegration_Release_ARM() {
//		m_builder.BuildByConfig("Release", "WSA_ARM");
//	}
}


public class AkUnityPluginInstaller_WSA : AkUnityPluginInstallerMultiArchBase
{
	public AkUnityPluginInstaller_WSA()
	{
		m_platform = "WSA";
		m_arches = new string[] {"WSA_Win32", "WSA_ARM", "WSA_WindowsPhone81_Win32", "WSA_WindowsPhone81_ARM"};
		m_excludes.AddRange(new string[] {".pdb", ".exp", ".lib", ".pri", "winmd"});
	}

	protected override string GetPluginDestPath(string arch)
	{
        string BaseDestPath = Path.Combine(m_pluginDir, m_platform);
		
		switch(arch)
		{
			case "WSA_WindowsPhone81_ARM":
				return Path.Combine(Path.Combine(BaseDestPath, "WindowsPhone81"), "ARM");
				
			case "WSA_WindowsPhone81_Win32":
				return Path.Combine(Path.Combine(BaseDestPath, "WindowsPhone81"), "x86");
				
			case "WSA_ARM":
				return Path.Combine(Path.Combine(BaseDestPath, "Win80"), "ARM");
				
			case "WSA_Win32":
			default:
				return Path.Combine(Path.Combine(BaseDestPath, "Win80"), "x86");
		}
	}	
}


public class AkUnityIntegrationBuilder_WSA : AkUnityIntegrationBuilderBase
{
	public AkUnityIntegrationBuilder_WSA()
	{
		m_platform = "WSA";
	}

}
#endif // #if UNITY_EDITOR