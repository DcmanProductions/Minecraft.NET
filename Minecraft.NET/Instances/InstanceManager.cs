﻿/*
    Minecraft.NET - LFInteractive LLC. 2021-2024﻿
    Minecraft.NET and its libraries are a collection of minecraft related libraries to handle downloading mods, modpacks, resourcepacks, and downloading and installing modloaders (fabric, forge, etc)
    Licensed under GPL-3.0
    https://www.gnu.org/licenses/gpl-3.0.en.html#license-text
*/

using Chase.Minecraft.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Chase.Minecraft.Instances;

public class InstanceManager
{
    private readonly string path;
    public Dictionary<Guid, InstanceModel> Instances { get; private set; }

    public InstanceManager(string path)
    {
        this.path = Directory.CreateDirectory(path).FullName;
        Instances = new();
        Load();
    }

    public InstanceModel Create(InstanceModel instance)
    {
        instance.InstanceManager = this;
        instance.Path = Directory.CreateDirectory(Path.Combine(path, GetUniqueInstanceDirectoryName(instance.Name))).FullName;
        Instances.Add(instance.Id, instance);
        Save(instance.Id, instance);
        return instance;
    }

    public InstanceModel Save(Guid id, InstanceModel instance)
    {
        Log.Debug("Saving instance to file: {PATH}", Path.Combine(instance.Path, "instance.json"));
        instance.InstanceManager = this;
        instance.LastModified = DateTime.Now;
        Instances[id] = instance;
        File.WriteAllText(Path.Combine(instance.Path, "instance.json"), JsonConvert.SerializeObject(instance));
        return instance;
    }

    public void Load()
    {
        Instances.Clear();
        string[] json = Directory.GetFiles(path, "instance.json", SearchOption.AllDirectories);
        foreach (string file in json)
        {
            Log.Debug("Attempting to load instance from file: {PATH}", file);
            string item = File.ReadAllText(file);
            InstanceModel? instance = JObject.Parse(item).ToObject<InstanceModel>();
            if (instance != null)
            {
                Log.Debug("Successfully loaded instance from file: {PATH}", file);
                instance.InstanceManager = this;
                Instances.Add(instance.Id, instance);
            }
            else
            {
                Log.Error("Failed to load instance file: {PATH}", file);
            }
        }
    }

    public InstanceModel? Load(string path)
    {
        string instanceFile = Path.Combine(path, "instance.json");
        InstanceModel? instance = JObject.Parse(File.ReadAllText(instanceFile)).ToObject<InstanceModel>();
        if (instance != null)
        {
            instance.InstanceManager = this;
            return Instances[instance.Id] = instance;
        }

        return null;
    }

    public void AddMod(InstanceModel instance, ModModel mod)
    {
        List<ModModel> mods = new();
        mods.AddRange(instance.Mods);
        mods.Add(mod);
        instance.Mods = mods.ToArray();
        Save(instance.Id, instance);
    }

    public InstanceModel[] GetInstancesByName(string name) => Instances.Values.Where(i => i.Name == name).ToArray();

    public InstanceModel GetFirstInstancesByName(string name) => Instances.Values.First(i => i.Name == name);

    public InstanceModel GetInstanceById(Guid id) => Instances[id];

    public bool Exist(string name) => Instances.Values.Any(i => i.Name == name);

    private string GetUniqueInstanceDirectoryName(string name)
    {
        string dirname = name;

        foreach (char illegal in Path.GetInvalidFileNameChars())
        {
            dirname = dirname.Replace(illegal, '-');
        }

        string[] dirs = Directory.GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly);
        int index = 0;
        string originalDirname = dirname;

        while (Array.Exists(dirs, dir => dir.Equals(Path.Combine(path, dirname), StringComparison.OrdinalIgnoreCase)))
        {
            index++;
            dirname = $"{originalDirname} ({index})";
        }

        return dirname;
    }
}