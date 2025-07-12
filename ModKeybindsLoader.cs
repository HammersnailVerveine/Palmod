using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace VervPalMod
{
    public class ModKeybindsLoader : ILoadable
    {
        public static ModKeybind Teleport { get; private set; }
        public static ModKeybind Release { get; private set; }
        public static ModKeybind NoKill { get; private set; }

        void ILoadable.Load(Mod mod)
        {
            Teleport = KeybindLoader.RegisterKeybind(mod, "Teleport", Keys.OemQuotes);
            Release = KeybindLoader.RegisterKeybind(mod, "Release", Keys.X);
            NoKill = KeybindLoader.RegisterKeybind(mod, "NoKill", Keys.C);
        }

        void ILoadable.Unload()
        {
            Teleport = null;
            Release = null;
            NoKill = null;
        }
    }
}