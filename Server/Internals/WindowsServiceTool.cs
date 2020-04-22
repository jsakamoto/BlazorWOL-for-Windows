using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;

namespace BlazorWOL.Server.Internals
{
    internal static class WindowsServiceTool
    {
        public enum StartUp
        {
            Auto,
            Demand,
            Disabled
        }

        private static string ExecutableFilePath => Process.GetCurrentProcess().MainModule.FileName;

        private static bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private static void RunAsAdmin(string fileName, params string[] args) => Run(true, fileName, args);

        private static void Run(string fileName, params string[] args) => Run(false, fileName, args);

        private static void Run(bool runAsAdmin, string fileName, params string[] args)
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = fileName,
                Arguments = string.Join(" ", args.Select(arg => arg.Contains(" ") ? "\"" + arg.Replace("\"", "\\\"") + "\"" : arg)),
            };
            if (runAsAdmin)
            {
                startInfo.UseShellExecute = true;
                startInfo.Verb = "runas";
            }

            Process.Start(startInfo).WaitForExit();
        }

        public static void RegisterToWindowsService(string serviceName, string displaName, string description, StartUp startUp = StartUp.Auto)
        {
            if (!IsAdministrator)
            {
                RunAsAdmin(ExecutableFilePath, "install");
                return;
            }

            var startupText = startUp switch
            {
                StartUp.Auto => "auto",
                StartUp.Demand => "demand",
                StartUp.Disabled => "disabled",
                _ => throw new Exception()
            };

            Run("sc", "create",
                serviceName,
                "binpath=", $"\"{ExecutableFilePath}\" --service",
                "start=", startupText,
                "displayname=", displaName
            );

            Run("sc", "description", serviceName, description);
        }

        public static void UnregisterToWindowsService(string serviceName)
        {
            if (!IsAdministrator)
            {
                RunAsAdmin(ExecutableFilePath, "uninstall");
                return;
            }

            Run("net", "stop", serviceName);
            Run("sc", "delete", serviceName);
        }
    }
}
