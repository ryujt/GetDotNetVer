using System;
using Microsoft.Win32;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("Checking .NET Framework Versions...");
        CheckDotNetFrameworkVersion();

        Console.WriteLine("\nChecking .NET Core Versions...");
        CheckDotNetCoreVersion();

        Console.ReadLine();
    }

    private static void CheckDotNetFrameworkVersion()
    {
        const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
        using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
        {
            if (ndpKey != null && ndpKey.GetValue("Release") != null)
            {
                Console.WriteLine($".NET Framework Version: {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}");
            }
            else
            {
                Console.WriteLine(".NET Framework Version 4.5 or later is not detected.");
            }
        }
    }

    private static string CheckFor45PlusVersion(int releaseKey)
    {
        if (releaseKey >= 533320)
            return "4.8.1 or later";
        if (releaseKey == 528049)
            return "4.8";
        if (releaseKey == 461814)
            return "4.7.2";
        if (releaseKey == 461310)
            return "4.7.1";
        if (releaseKey == 460805)
            return "4.7";
        if (releaseKey == 394806)
            return "4.6.2";
        if (releaseKey == 394271)
            return "4.6.1";
        if (releaseKey == 393297)
            return "4.6";
        if (releaseKey == 379893)
            return "4.5.2";
        if (releaseKey >= 378758)
            return "4.5.1";
        if (releaseKey == 378389)
            return "4.5";
        return $"Unknown version: {releaseKey}";
    }

    private static void CheckDotNetCoreVersion()
    {
        ExecuteCommand("dotnet --list-sdks", "Installed .NET Core SDKs:");
        ExecuteCommand("dotnet --list-runtimes", "Installed .NET Core Runtimes:");
    }

    private static void ExecuteCommand(string command, string title)
    {
        Console.WriteLine(title);

        var processInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = Process.Start(processInfo);
        process.OutputDataReceived += (sender, data) =>
        {
            Console.WriteLine(data.Data);
        };
        process.BeginOutputReadLine();
        process.WaitForExit();
    }
}
