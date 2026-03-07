using BepInEx;
using BepInEx.Logging;
using Silksong.AssetHelper.Internal;
using Silksong.ModMenu.Elements;
using Silksong.ModMenu.Plugin;
using System.Diagnostics;
using System.IO;

namespace AHDG;

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.flibber-hk.ahdg")]
public partial class AHDGPlugin : BaseUnityPlugin, IModMenuCustomElement
{
    public static ManualLogSource InstanceLogger { get; private set; }

    public SelectableElement BuildCustomElement()
    {
        TextButton button = new("Run AHDG Generation");
        button.OnSubmit = () =>
        {
            TransferredData data = TransferredData.Create();

            string folder = Path.GetDirectoryName(GetType().Assembly.Location);

            data.SerializeToFile(Path.Combine(
                folder,
                "data.json"
                ));

            Process.Start(folder);
        };

        return button;
    }

    public string ModMenuName()
    {
        return "AHDG";
    }

    private void Awake()
    {
        InstanceLogger = Logger;

        // Put your initialization logic here
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
    }
}
