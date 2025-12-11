using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Manager;
using Newtonsoft.Json;
using UnityEngine;

namespace Generic
{
    public static class SaveLoad
    {
        private const string FileName = "Save.json";
        private static string DirectoryPath => Path.Combine(Application.persistentDataPath, "Save");

        public static void SaveUserData(UserData userData) => SaveUserDataAsync(userData).Forget();
        public static void LoadUserData() => LoadUserDataAsync().Forget();
        
        private static async UniTaskVoid SaveUserDataAsync(UserData userData)
        {
            string path = Path.Combine(DirectoryPath, FileName);
            CheckDirectory();

            try
            {
                string json = JsonConvert.SerializeObject(userData);

                await File.WriteAllTextAsync(path, json, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        private static async UniTaskVoid LoadUserDataAsync()
        {
            string path = Path.Combine(DirectoryPath, FileName);
            
            try
            {
                UserData userData;
                
                if (File.Exists(path))
                {
                    string json = await File.ReadAllTextAsync(path, Encoding.UTF8);
                    userData = JsonConvert.DeserializeObject<UserData>(json);
                    
                }
                else
                {
                    userData = UserData.CreateNewUserData();
                    SaveUserDataAsync(userData).Forget();
                }
                
                EventManager.Instance.OnLoadUserData?.Invoke(userData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
        
        
        private static void CheckDirectory()
        {
            if (Directory.Exists(DirectoryPath)) return;
            
            Directory.CreateDirectory(DirectoryPath);
        }
    }
}