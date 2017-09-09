using System;
using UnityEngine;
using System.IO;
using System.Text;
using Extensions;
using AJSON;

namespace AppCore {
    public class DataController : IDataController, IOnApplicationQuitable {
        #region Members
        /// <summary>
        /// Player data file name.
        /// </summary>
        public string playerDataFileName { get; private set; }
        /// <summary>
        /// Path to data folder inside assets folder (witout persistentDataPath).
        /// </summary>
        public string dataAssetFolderPath { get; private set; }
        /// <summary>
        /// Current data loaded from path.
        /// </summary>
        public PlayerData currentData { get; private set; }
        #endregion

        #region Events
        /// <summary>
        /// Event called when currentData is loaded.
        /// </summary>
        public event Action<PlayerData> OnLoaded;
        /// <summary>
        /// Event called when the currentData is saved.
        /// </summary>
        public event Action<PlayerData> OnSaved;
        /// <summary>
        /// Event called when the data is applied.
        /// </summary>
        public event Action<PlayerData> OnReplaced;
        #endregion

        #region Mono implementation
        public void OnApplicationQuit() {
            Save();
        }
        #endregion

        public DataController() {
            playerDataFileName = "playerData.json";
            dataAssetFolderPath = "Data";
        }

        public void Init() {
            Load();
        }

        #region Public methods
        /// <summary>
        /// Replaces currentData with given data.
        /// </summary>
        /// <param name="data">Data that will replace currentData.</param>
        public void Replace(PlayerData data) {
            //currentData = data;
            //if (OnReplaced != null) {
            //    OnReplaced(data);
            //}
        }
        /// <summary>
        /// Loads currentData from file (automatically called when class instance is initialized).
        /// </summary>
        public void Load() {
            //Debug.Log(JSON.Serialize(new PlayerData()));

            //currentData = JsonUtility.FromJson<PlayerData>(Encoding.UTF8.GetString(EData.Load(Path.Combine(dataAssetFolderPath, playerDataFileName))));
            //if (currentData == null) {
            //    currentData = new PlayerData();
            //}
            //if (OnLoaded != null) {
            //    OnLoaded(currentData);
            //}
        }
        /// <summary>
        /// Saves currentData (automatically called in OnApplicationQuit).
        /// </summary>
        public void Save() {
            //EData.Save(Path.Combine(dataAssetFolderPath, playerDataFileName), Encoding.UTF8.GetBytes(JSONObject.Se .ToJson(currentData)));
            //if (OnSaved != null) {
            //    OnSaved(currentData);
            //}
        }
        #endregion
    }
}
