/*
/*
	newwer
#1#
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LitJson;
using System.IO;


	public class MenuTool  {

	
		//[MenuItem("设置/设置开机启动",false,5000)]
        public static void StartUp()
        {
            string jsonText = GetConfigContent();
            JsonData jsonData= JsonMapper.ToObject(jsonText);
            Debug.Log(jsonData["path"]);
            string path = jsonData["path"].ToString();
            path= string.Format("\"{0}\"", path);
            //string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            RegistryKey rgkRun = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rgkRun == null)
            {
                rgkRun = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            }
            rgkRun.SetValue("DesktopPet", path);

            Debug.Log("设置成功");
        }

        //[MenuItem("设置/取消开机启动", false, 5010)]
        public static void CancelStartUp()
        {
            string jsonText = GetConfigContent();
            JsonData jsonData = JsonMapper.ToObject(jsonText);
            Debug.Log(jsonData["path"]);
            string path = jsonData["path"].ToString();

            RegistryKey rgkRun = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rgkRun == null)
            {
                rgkRun = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            }

            rgkRun.DeleteValue("DesktopPet", false);

            Debug.Log("取消设置成功");
        }

        public static string GetConfigContent()
        {
            //print(Application.streamingAssetsPath);

            string jsonPath = string.Format("{0}{1}", Application.streamingAssetsPath, "/config.json");
            //print(jsonPath);
            string jsonText = File.ReadAllText(jsonPath);
            //print(jsonText);

            return jsonText;
        }

    }
    */

