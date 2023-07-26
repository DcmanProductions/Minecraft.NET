﻿/*
    Minecraft.NET - LFInteractive LLC. 2021-2024﻿
    Minecraft.NET and its libraries are a collection of minecraft related libraries to handle downloading mods, modpacks, resourcepacks, and downloading and installing modloaders (fabric, forge, etc)
    Licensed under GPL-3.0
    https://www.gnu.org/licenses/gpl-3.0.en.html#license-text
*/

namespace Chase.Minecraft.Model;

public struct ModLoaderModel
{
    public ModLoaders Modloader { get; set; } = ModLoaders.None;

    public string Version { get; set; } = "";

    public ModLoaderModel()
    {
    }
}