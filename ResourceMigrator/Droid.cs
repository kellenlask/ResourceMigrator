﻿using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResourceMigrator
{
    public static class Droid 
    {
        public static void WriteToTarget(FileSystemInfo sourceFile, string targetDir, Dictionary<string, string> strings)
        {
            var resourceType = sourceFile.GetResourceType();
            var builder = new StringBuilder();

            builder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            builder.Append(@"
<!-- ..........................................................................
    <auto-generated>
        This code was generated by a tool.
        Runtime Version:" + Helpers.GetAssemblyVersion() + @" (ResourceMigrator.exe)

        Changes to this file may cause incorrect behavior and will be lost if
        the code is regenerated.
    </auto-generated>
 ........................................................................... -->
");

            builder.AppendLine("<resources>");

            foreach (var key in strings.Keys)
            {
                builder.Append("    ");
                builder.AppendLine(string.Format("<{0} name=\"{1}\">{2}</{0}>", resourceType, key, strings[key].ToEscapedString()));
            }

            builder.AppendLine("</resources>");

            var outputFileName = Path.GetFileNameWithoutExtension(sourceFile.Name) + ".xml";
            File.WriteAllText(Path.Combine(targetDir, outputFileName), builder.ToString());
        } 
    }
}