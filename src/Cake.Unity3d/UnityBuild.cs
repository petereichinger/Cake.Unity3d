using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Unity3d {
    public static class Build {
        [CakeMethodAlias]
        public static void UnityBuild(this ICakeContext context, Action<BuildSettings> buildSettingsAction) {
            var buildSettings = new BuildSettings();
            buildSettingsAction(buildSettings);

            var valid = buildSettings.ValidateArguments(context);
            if (!valid) {
                throw new Exception("Invalid build settings");
            }

            var processArguments = buildSettings.GetProcessArguments(context);
            
            var runner = new ProcessRunner(context.Environment,context.Log);
            var settings = new ProcessSettings {Arguments = processArguments};
            var process = runner.Start(buildSettings.Unity, settings);
            process.WaitForExit();
        }
    }
}
