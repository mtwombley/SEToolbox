﻿namespace SEToolbox.Interop
{
    using System.Collections.Generic;
    using System.Linq;
    using Sandbox.Definitions;
    using VRage.Game;
    using VRage.ObjectBuilders;
    using VRage.ObjectBuilders.Private;

    /// <summary>
    /// Encapsulates the game definitions, either stock or loaded for a specific save game world.
    /// </summary>
    public class SpaceEngineersResources
    {
        public Dictionary<string, byte> MaterialIndex { get; private set; }

        /// <summary>
        /// Loads Stock definitions from default path, useful for tests.
        /// </summary>
        public void LoadDefinitions()
        {
            // Call PreloadDefinitions(), to load DefinitionsToPreload.sbc file first.
            // otherwise LoadData() may throw an InvalidOperationException due to a modified collection.
            MyDefinitionManager.Static.PreloadDefinitions();
            MyDefinitionManager.Static.LoadData(new List<MyObjectBuilder_Checkpoint.ModItem>());
            MaterialIndex = new Dictionary<string, byte>();
        }

        public void LoadDefinitionsAndMods(string userModspath, List<MyObjectBuilder_Checkpoint.ModItem> mods)
        {
            // Call PreloadDefinitions(), to load DefinitionsToPreload.sbc file first.
            // otherwise LoadData() may throw an InvalidOperationException due to a modified collection.
            MyDefinitionManager.Static.PreloadDefinitions();
            MyDefinitionManager.Static.LoadData(mods);
            MaterialIndex = new Dictionary<string, byte>();
        }

        public string GetDataPathOrDefault(string key, string defaultValue)
        {
            // TODO: this code is obsolete and needs to be cleaned up.
            // #31 https://github.com/midspace/SEToolbox/commit/354fd4cba31d1d8accac4c8188189dd1b114209b#diff-816c9c8868fbb3625db0cc45485797ef

            return defaultValue;
        }

        private static readonly object MatindexLock = new object();

        public byte GetMaterialIndex(string materialName)
        {
            lock (MatindexLock)
            {
                return MyDefinitionManager.Static.GetVoxelMaterialDefinition(materialName).Index;
            }
        }

        public IList<MyBlueprintDefinitionBase> BlueprintDefinitions
        {
            get { return MyDefinitionManager.Static.GetBlueprintDefinitions().ToList(); }
        }

        public IEnumerable<MyCubeBlockDefinition> CubeBlockDefinitions
        {
            get { return MyDefinitionManager.Static.GetAllDefinitions().Where(e => e is MyCubeBlockDefinition).Cast<MyCubeBlockDefinition>(); }
        }

        public IList<MyComponentDefinition> ComponentDefinitions
        {
            get { return MyDefinitionManager.Static.GetPhysicalItemDefinitions().Where(e => e is MyComponentDefinition).Cast<MyComponentDefinition>().ToList(); }
        }

        public IList<MyPhysicalItemDefinition> PhysicalItemDefinitions
        {
            get { return MyDefinitionManager.Static.GetPhysicalItemDefinitions().Where(e => !(e is MyComponentDefinition)).ToList(); }
        }

        public IList<MyAmmoMagazineDefinition> AmmoMagazineDefinitions
        {
            get { return MyDefinitionManager.Static.GetAllDefinitions().Where(e => e is MyAmmoMagazineDefinition).Cast<MyAmmoMagazineDefinition>().ToList(); }
        }

        public IList<MyVoxelMaterialDefinition> VoxelMaterialDefinitions
        {
            get { return MyDefinitionManager.Static.GetVoxelMaterialDefinitions().ToList(); }
        }

        public IList<MyVoxelMapStorageDefinition> VoxelMapStorageDefinitions
        {
            get { return MyDefinitionManager.Static.GetVoxelMapStorageDefinitions().ToList(); }
        }

        public IList<MyCharacterDefinition> CharacterDefinitions
        {
            get { return MyDefinitionManager.Static.Characters.ToList(); }
        }

        public string GetMaterialName(byte materialIndex, byte defaultMaterialIndex)
        {
            if (materialIndex <= MyDefinitionManager.Static.GetVoxelMaterialDefinitions().Count)
                return MyDefinitionManager.Static.GetVoxelMaterialDefinition(materialIndex).Id.SubtypeName;

            return MyDefinitionManager.Static.GetVoxelMaterialDefinition(defaultMaterialIndex).Id.SubtypeName;

            //if (materialIndex <= _definitions.VoxelMaterials.Length)
            //    return _definitions.VoxelMaterials[materialIndex].Id.SubtypeId;

            //return _definitions.VoxelMaterials[defaultMaterialIndex].Id.SubtypeId;
        }

        public string GetMaterialName(byte materialIndex)
        {
            return MyDefinitionManager.Static.GetVoxelMaterialDefinition(materialIndex).Id.SubtypeName;
            //return _definitions.VoxelMaterials[materialIndex].Id.SubtypeId;
        }

        public string GetDefaultMaterialName()
        {
            return MyDefinitionManager.Static.GetDefaultVoxelMaterialDefinition().Id.SubtypeName;
        }

        public byte GetDefaultMaterialIndex()
        {
            return MyDefinitionManager.Static.GetDefaultVoxelMaterialDefinition().Index;
        }

        public T CreateNewObject<T>()
            where T : MyObjectBuilder_Base
        {
            return (T)MyObjectBuilderSerializerKeen.CreateNewObject(typeof(T));
        }

        public T CreateNewObject<T>(MyObjectBuilderType typeId, string subtypeId)
           where T : MyObjectBuilder_Base
        {
            return (T)MyObjectBuilderSerializerKeen.CreateNewObject(typeId, subtypeId);
        }
    }
}
