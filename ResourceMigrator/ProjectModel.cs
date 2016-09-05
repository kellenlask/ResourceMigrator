﻿namespace ResourceMigrator
{
	public class ProjectModel
	{
		public string ProjectNamespace { get; set; }
		public string ProjectPath { get; set; }
		public PlatformType PlatformType { get; set; }
	}

	public enum PlatformType
	{
		Windows, // Windows Phone
		Ios, // iOS 
		Droid, // Android
		Pcl // Portable Class Library
	}
}