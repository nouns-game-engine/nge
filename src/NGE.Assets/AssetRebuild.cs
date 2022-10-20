using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace NGE.Assets;

public static class AssetRebuild
{
    public static bool Run()
    {
        try
        {
            var sw = Stopwatch.StartNew();

            if (!RebuildGameAssets())
                return false;

            Trace.WriteLine($"AssetRebuild: Completed, took {sw.Elapsed}");
            return true;
        }
        catch (Exception e)
        {
            Trace.TraceError($"AssetRebuild: Failed with: {e}");
            return false;
        }
    }

    private static bool RebuildGameAssets()
    {
        // This file is generated at build time (see ContentRebuilder.targets)
        var infoPath = Path.Combine(Environment.CurrentDirectory, "AssetRebuildInfo.txt");
        if (!File.Exists(infoPath))
        {
            Trace.TraceWarning($"AssetRebuild: Could not find file {infoPath}");
            return false;
        }

        var lines = File.ReadAllLines(infoPath);
        if (lines.Length < 2)
        {
            Trace.TraceWarning($"AssetRebuild: Missing data in {infoPath}");
            return false;
        }

        string msBuildDirectory = lines[0];
        if (!Directory.Exists(msBuildDirectory))
        {
            Trace.TraceWarning($"AssetRebuild: Could not find directory {msBuildDirectory}");
            return false;
        }

        var projectPath = lines[1];
        if (!File.Exists(projectPath))
        {
            Trace.TraceWarning($"AssetRebuild: Could not find {projectPath}");
            return false;
        }

        var msBuildPath = Environment.OSVersion.Platform == PlatformID.Win32NT
            ? Path.Combine(msBuildDirectory, "MSBuild")
            : "msbuild";
        
        try
        {
            var workingDir = Path.GetDirectoryName(projectPath);
            var arguments = "/t:BuildContentOnly /p:Platform=x64";

            Trace.TraceInformation($"AssetRebuild: MSBuild path is '{msBuildPath}'");
            Trace.TraceInformation($"AssetRebuild: Working directory is '{workingDir}'");
            Trace.TraceInformation($"AssetRebuild: Arguments are '{arguments}'");

            var process = new Process();
            process.StartInfo.FileName = msBuildPath;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WorkingDirectory = workingDir;
            process.Start();
            process.WaitForExit();
        }
        catch (Exception e)
        {
            Trace.TraceWarning($"AssetRebuild: Could not execute MSBuild");
            Trace.TraceWarning(e.ToString());
        }

        return true;
    }

    #region Live Reload

    private static FileSystemWatcher? assetWatcher;

    public static bool EnableLiveReload(IAssetRebuildReceiver receiver)
    {
        // This file is generated at build time (see ContentRebuilder.targets)
        var infoPath = Path.Combine(Environment.CurrentDirectory, "AssetRebuildInfo.txt");
        if (!File.Exists(infoPath))
        {
            Trace.TraceWarning($"AssetRebuild: Could not find file {infoPath}");
            return false;
        }

        var lines = File.ReadAllLines(infoPath);
        if (lines.Length < 2)
        {
            Trace.TraceWarning($"AssetRebuild: Missing data in {infoPath}");
            return false;
        }

        var msBuildDirectory = lines[0];
        if (!Directory.Exists(msBuildDirectory))
        {
            Trace.TraceWarning($"AssetRebuild: Could not find directory {msBuildDirectory}");
            return false;
        }

        var projectPath = lines[1];
        if (!File.Exists(projectPath))
        {
            Trace.TraceWarning($"AssetRebuild: Could not find {projectPath}");
            return false;
        }

        var contentPath = $"{Path.GetDirectoryName(projectPath)}\\Content";

        assetWatcher?.Dispose();
        assetWatcher = null;

        assetWatcher = new FileSystemWatcher();
        assetWatcher.Path = contentPath;
        assetWatcher.NotifyFilter = NotifyFilters.LastWrite;
        assetWatcher.Filter = "*.*";
        assetWatcher.Changed += (_, e) =>
        {
            while (IsFileLocked(new FileInfo(e.FullPath)))
                Thread.Sleep(10);

            Trace.WriteLine($"AssetRebuild: {Path.GetFileName(e.FullPath)} changed");
            receiver.ShouldRebuildAssets();
        };
        assetWatcher.EnableRaisingEvents = true;

        Trace.TraceInformation("AssetRebuild: Live load enabled");
        return true;
    }

    public static void DisableLiveReload()
    {
        assetWatcher?.Dispose();
        assetWatcher = null;
        Trace.TraceInformation("AssetRebuild: Live load disabled");
    }

    private static bool IsFileLocked(FileInfo file)
    {
        FileStream? stream = null;
        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException)
        {
            return true;
        }
        finally
        {
            stream?.Close();
        }

        return false;
    }

    #endregion
}