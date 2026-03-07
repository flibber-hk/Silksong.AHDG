using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AssetHelperLib.PreloadTable;
using Silksong.AssetHelper;
using Silksong.AssetHelper.Core;
using Silksong.AssetHelper.Dev;
using Silksong.AssetHelper.Internal;

namespace AHDG;

public class TransferredData
{
    public string OSFolderName => AssetPaths.OSFolderName;
    public string SilksongVersion => VersionData.SilksongVersion;
    public string AssetHelperVersion => AssetHelperPlugin.Version;
    public string PluginVersion => AHDGPlugin.Version;

    // Ad9s keys
    public List<string>? AddressablesKeys { get; set; }

    // Cabs
    public Dictionary<string, string>? CabLookup { get; set; }

    // Bundle names
    public Dictionary<string, string>? BundleNameLookup { get; set; }

    // Direct deps
    public Dictionary<string, List<string>>? DirectDeps { get; set; }

    // CPPCache
    public Dictionary<string, ContainerPointerPreloadsBundleData>? CPPCache { get; set; }

    public static TransferredData Create()
    {
        TransferredData data = new();

        AHDGPlugin.InstanceLogger.LogInfo($"Ad9s keys");
        data.AddressablesKeys = AddressablesData.MainLocator!.Keys.OfType<string>().ToList();

        AHDGPlugin.InstanceLogger.LogInfo($"Cabs");
        data.CabLookup = BundleMetadata.CabLookup.ToSortedDict();

        AHDGPlugin.InstanceLogger.LogInfo($"Bundle names");
        data.BundleNameLookup = DebugTools.GenerateBundleNameLookup().ToSortedDict();

        AHDGPlugin.InstanceLogger.LogInfo($"Direct deps");
        data.DirectDeps = data.CabLookup.Values.ToDictionary(x => x, x => BundleMetadata.DetermineDirectDeps(x));

        AHDGPlugin.InstanceLogger.LogInfo($"Cpp cache");
        Dictionary<string, ContainerPointerPreloadsBundleData> cppData = new();
        int ctr = 0;

        foreach ((string cab, string name) in data.CabLookup)
        {
            if (name.StartsWith("scenes_scenes_scenes")) continue;

            AHDGPlugin.InstanceLogger.LogInfo($"[{++ctr}] {name}");
            cppData[cab] = ContainerPointerPreloadsBundleData.FromFile(
                Path.Combine(AssetPaths.BundleFolder, name)
                );
        }

        data.CPPCache = cppData;

        return data;
    }
}

file static class Ext
{
    public static Dictionary<string, T> ToSortedDict<T>(this IReadOnlyDictionary<string, T> self)
    {
        return self.Keys.OrderBy(x => x).ToDictionary(x => x, x => self[x]);
    }
}
