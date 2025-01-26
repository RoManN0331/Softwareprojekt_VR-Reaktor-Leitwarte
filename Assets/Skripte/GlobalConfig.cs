using UnityEngine;
using System.Diagnostics;
using System;

/*
*  Stores global configuration values and handles various setup tasks:
*  Start NPP-Rest-Server
*  Load scenes
*/
public class GlobalConfig : MonoBehaviour
{
    public static string BASE_URL = "http://localhost:8080/api/";
    public static float CLIENT_UPDATE_INTERVAL = .5f;

    public const bool START_REST_SERVER = false;


    private Process javaRestServerProcess;

    void Start()
    {
        //Start NPP-Rest-Server
        
        string restServerExecutablePath = System.IO.Path.Combine(Application.dataPath, "Skripte", "restapi-vr-1.0.jar");
        UnityEngine.Debug.Log("Rest-Server Path: " + restServerExecutablePath);

        //Rest-Server Path: E:/Unity/Projects/Softwareprojekt_VR-Reaktor-Leitwarte/Assets\Skripte\restapi-vr-1.0.jar
        
        ProcessStartInfo javaRestServerStartInfo = new ProcessStartInfo {
            FileName = "java",
            Arguments = $"-jar {restServerExecutablePath}",
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        javaRestServerProcess = new Process {
            StartInfo = javaRestServerStartInfo
        };
        if (START_REST_SERVER)
        {
            javaRestServerProcess.Start();
            if (javaRestServerProcess.HasExited)
                UnityEngine.Debug.LogError("Failed to start NPP-Rest-Server");
            else
                UnityEngine.Debug.Log("NPP-Rest-Server started");
        }
    
    }

    void Update()
    {
       // Server output logs
       //while (!javaRestServerProcess.HasExited && !javaRestServerProcess.StandardOutput.EndOfStream)
       //{
       //     UnityEngine.Debug.Log(javaRestServerProcess.StandardOutput.ReadLine());
        //}
    }

    void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("Shutting down REST-Server after " + Time.time + " seconds");
        if (START_REST_SERVER && javaRestServerProcess != null && !javaRestServerProcess.HasExited)
        {
            javaRestServerProcess.Kill();
            javaRestServerProcess.WaitForExit();
            UnityEngine.Debug.Log("NPP-Rest-Server process terminated.");
            javaRestServerProcess.Dispose();
        }
    }
}