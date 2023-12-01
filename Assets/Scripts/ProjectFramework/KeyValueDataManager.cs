using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrameWork
{
    /// <summary>
    /// 用某个实例的属性来指向其他类的静态数据,来间接进行数据持久化
    /// </summary>
    public class PersistentData
    {
        
    }

    public class KeyValueDataManager : SingletonManager<KeyValueDataManager>
    {
        // 数据存储
#region 
        public PersistentData OtherPersistentData;
        protected Dictionary<string, Dictionary<string, string>> _temporaryData;
        public Dictionary<string, Dictionary<string, string>> TemporaryData
        {
            get
            {
                if (_temporaryData == null)
                {
                    _temporaryData = new Dictionary<string, Dictionary<string, string>>();
                }
                return _temporaryData;
            }
        }

        protected Dictionary<string, Dictionary<string, string>> _persistentData;
        public Dictionary<string, Dictionary<string, string>> PersistentData
        {
            get
            {
                if (_persistentData == null)
                {
                    _persistentData = new Dictionary<string, Dictionary<string, string>>();
                }
                return _persistentData;
            }
        }

#endregion

        // 数据读写
#region

        public void SetPersistentData(string key, string value)
        {
            HashSetPersistentData(key, "DragonBornDataBase", value);
        }

        public void HashSetPersistentData(string key, string subKey, string value)
        {
            if (!PersistentData.ContainsKey(key))
            {
                PersistentData.Add(key, new Dictionary<string, string>());
            }
            PersistentData[key][subKey] = value;
        }

        public void SetTemporaryData(string key, string value)
        {
            HashSetTemporaryData(key, "DragonBornDataBase", value);
        }

        public void HashSetTemporaryData(string key, string subKey, string value)
        {
            if (!TemporaryData.ContainsKey(key))
            {
                TemporaryData.Add(key, new Dictionary<string, string>());
            }
            TemporaryData[key][subKey] = value;
        }

        public string GetData(string key)
        {
            return HashGetData(key, "DragonBornDataBase");
        }

        public string HashGetData(string key, string subKey)
        {
            string result = null;
            if (TemporaryData.ContainsKey(key) && TemporaryData[key].ContainsKey(subKey))
            {
                result = TemporaryData[key][subKey];
            }
            else if (PersistentData.ContainsKey(key) && PersistentData[key].ContainsKey(subKey))
            {
                result = PersistentData[key][subKey];
            }
            
            return result;
        }

#endregion
    
        // 数据持久化
#region

        public void SaveData()
        {
#if !UNITY_EDITOR
            return;
#endif
            //TODO,序列化模块和文件读写模块
            File.WriteAllText("Assets/Resources/Configs/PersistentData.json", JsonConvert.SerializeObject(PersistentData, Formatting.Indented));
        }

        protected void LoadData()
        {
            _instance = JsonConvert.DeserializeObject<KeyValueDataManager>(File.ReadAllText("Assets/Resources/Configs/PersistentData.json"));
        }

#endregion
    }
}