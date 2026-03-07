using BepInEx;
using BepInEx.Logging;
using Silksong.AssetHelper.Internal;
using Silksong.ModMenu.Elements;
using Silksong.ModMenu.Plugin;
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

            data.SerializeToFile(Path.Combine(
                Path.GetDirectoryName(GetType().Assembly.Location),
                "data.json"
                ));
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
