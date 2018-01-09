using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Unity3d {
    public static class Build {
        [CakeMethodAlias]
        public static void UnityBuild(this ICakeContext context, DirectoryPath projectPath, FilePath target, FilePath unity) {
            var builder = new ProcessArgumentBuilder();

            builder.Append("-batchmode");
            builder.Append("-quit");

            builder.Append("-buildWindows64Player");
            builder.AppendQuoted(target.MakeAbsolute(context.Environment).FullPath);

            builder.Append("-projectPath");
            builder.AppendQuoted(projectPath.MakeAbsolute(context.Environment).FullPath);

            builder.Append("-logFile");
            builder.AppendQuoted(projectPath.CombineWithFilePath(new FilePath("build.log")).FullPath);

            var runner = new ProcessRunner(context.Environment,context.Log);
            var settings = new ProcessSettings {Arguments = builder, RedirectStandardOutput = true, RedirectStandardError = true};
            var process = runner.Start(unity, settings);
            process.WaitForExit();
        }
    }
}
