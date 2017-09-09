using System;
namespace AppCore {
    public interface IDataController :  IController {
        string playerDataFileName { get; }
        string dataAssetFolderPath { get; }
        PlayerData currentData { get; }
        event Action<PlayerData> OnLoaded;
        event Action<PlayerData> OnSaved;
        event Action<PlayerData> OnReplaced;
        void Load();
        void Replace(PlayerData data);
        void Save();
    }
}