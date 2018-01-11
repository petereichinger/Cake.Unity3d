using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Unity3d {
    public class BuildSettings {
        public class Platform {
            public string Name { get; }
            public string Parameter { get; }

            public static Platform Linux32 => new Platform("Linux 32", "-buildLinux32Player");
            public static Platform Linux64 => new Platform("Linux 64", "-buildLinux64Player");
            public static Platform LinuxUniversal => new Platform("Linux Universal", "-buildLinuxUniversalPlayer");

            public static Platform Osx32 => new Platform("OS X 32", "-buildOSXPlayer");
            public static Platform Osx64 => new Platform("OS X 64", "-buildOSX64Player");
            public static Platform OsxUniversal => new Platform("OS X Universal", "-buildOSXUniversalPlayer");

            public static Platform Windows32 => new Platform("Windows 32", "-buildWindowsPlayer");
            public static Platform Windows64 => new Platform("Windows 64", "-buildWindows64Player");
            
            private Platform(string name, string parameter) {
                Name = name;
                Parameter = parameter;
            }

            public override string ToString() {
                return Parameter;
            }
        }
        public Platform TargetPlatform { get; set; }
        public DirectoryPath Project { get; set; }
        public FilePath Unity { get; set; }
        public FilePath Target { get; set; }

        public BuildSettings WithUnity(string path) {
            Unity = new FilePath(path);
            return this;
        }

        public BuildSettings WithUnity(FilePath path) {
            Unity = path;
            return this;
        }

        public BuildSettings WithProject(string path) {
            Project = new DirectoryPath(path);
            return this;
        }

        public BuildSettings WithProject(DirectoryPath path) {
            Project = path;
            return this;
        }

        public BuildSettings WithTarget(string path) {
            Target = new FilePath(path);
            return this;
        }

        public BuildSettings WithTarget(FilePath path) {
            Target = path;
            return this;
        }

        public bool ValidateArguments(ICakeContext context) {
            bool valid = true;
            if (!context.FileSystem.Exist(Unity)) {
                context.Log.Error(Verbosity.Normal, $"Unity executable does not exist: '{Unity}'");
                valid = false;
            }

            if (!context.FileSystem.Exist(Project)) {
                context.Log.Error(Verbosity.Normal, $"Project does not exist: '{Project}'");
                valid = false;
            } else {
                var assetsFolder = Project.Combine("Assets");
                if (!context.FileSystem.Exist(assetsFolder)) {
                    context.Log.Error(Verbosity.Normal, $"Specified project folder does not look like a Unity Project: '{Project}'");
                    valid = false;
                }
            }

            if (TargetPlatform == null) {
                context.Log.Error(Verbosity.Normal, "Must specify a platform.");
                valid = false;
            }

            return valid;
        }

        public ProcessArgumentBuilder GetProcessArguments(ICakeContext context) {
            var builder = new ProcessArgumentBuilder();
            
            builder.Append("-batchmode");
            builder.Append("-quit");

            builder.Append(TargetPlatform.ToString());
            builder.AppendQuoted(Target.MakeAbsolute(context.Environment).FullPath);

            builder.Append("-projectPath");
            builder.AppendQuoted(Project.MakeAbsolute(context.Environment).FullPath);

            return builder;
        }
    }
}
