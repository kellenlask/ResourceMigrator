﻿using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResourceMigrator
{
	public class Touch : IDeviceHandler
	{
		private static string _targetDir;
		private static IDictionary<string, string> _strings;
		private static string _touchNameSpace;

		public void WriteToTarget(ProjectModel project, IDictionary<string, string> strings, FileInfo sourceFile)
		{
            // Prepare to write to iOS resources directory
			_targetDir = Path.Combine(project.ProjectPath, "resources/");
			_strings = strings;
			_touchNameSpace = project.ProjectNamespace + ".Resources";

            // Translate the resx loaded resources into their particular resource type
			var resourceType = sourceFile.GetResourceType();
			if (resourceType == "color")
			{
				BuildColorResourceForTouch();
			}
		}


		private static void BuildColorResourceForTouch()
		{
			var builder = new StringBuilder();
			builder.Append(@"
#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:" + Helpers.GetAssemblyVersion() + @"
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;

namespace " + _touchNameSpace + @"
{
    internal static class CustomUIColor
    {

        private static MonoTouch.UIKit.UIColor FromHexString(string hexValue)
        {
            var colorString = hexValue.Replace(""#"", """");
            float red, green, blue;

            switch (colorString.Length)
            {
                case 3: // #RGB
                    {
                        red = Convert.ToInt32(string.Format(""{0}{0}"", colorString.Substring(0, 1)), 16) / 255f;
                        green = Convert.ToInt32(string.Format(""{0}{0}"", colorString.Substring(1, 1)), 16) / 255f;
                        blue = Convert.ToInt32(string.Format(""{0}{0}"", colorString.Substring(2, 1)), 16) / 255f;
                        return MonoTouch.UIKit.UIColor.FromRGB(red, green, blue);
                    }
                case 6: // #RRGGBB
                    {
                        red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                        green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                        blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                        return MonoTouch.UIKit.UIColor.FromRGB(red, green, blue);
                    }
                case 8: // #AARRGGBB
                    {
                        var alpha = Convert.ToInt32(colorString.Substring(0, 2), 16)/255f;
                        red = Convert.ToInt32(colorString.Substring(2, 2), 16)/255f;
                        green = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                        blue = Convert.ToInt32(colorString.Substring(6, 2), 16) / 255f;
                        return MonoTouch.UIKit.UIColor.FromRGBA(red, green, blue, alpha);
                    }
                default:
                    throw new ArgumentOutOfRangeException(string.Format(""Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB, or #AARRGGBB"", hexValue));

            }
        }
");

			foreach (var key in _strings.Keys)
			{
				builder.Append("        ");
				builder.AppendLine(string.Format("public static MonoTouch.UIKit.UIColor {0} = FromHexString(\"{1}\");", key, _strings[key].ToEscapedString()));
			}

			builder.Append(@"    }
}
#pragma warning restore 1591");


			File.WriteAllText(Path.Combine(_targetDir, "CustomUIColor.cs"), builder.ToString());
		}
	}
}