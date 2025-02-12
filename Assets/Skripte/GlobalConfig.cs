using UnityEngine;
using System.Diagnostics;
using System;
using Unity.VisualScripting;
using System.Text.RegularExpressions;

/*
*  Stores global configuration values and handles various setup tasks:
*  Start NPP-Rest-Server
*  Load scenes
*/
public class GlobalConfig : MonoBehaviour
{
    public static string BASE_URL = "http://localhost:8080/api/";
    public static float CLIENT_UPDATE_INTERVAL = .1f;

    public bool START_REST_SERVER;


    private Process javaRestServerProcess;
    private int restServerPID;

    void Start()
    {
        //Start NPP-Rest-Server
        if (START_REST_SERVER)
        {
            string restServerExecutablePath;
            
        #if UNITY_EDITOR
            restServerExecutablePath = System.IO.Path.Combine(Application.dataPath, "Skripte", "restapi-vr-1.1.jar");
        #else
            restServerExecutablePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "RestServer", "restapi-vr-1.1.jar");
        #endif
            UnityEngine.Debug.Log("Rest-Server Path: " + restServerExecutablePath);
          
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

            javaRestServerProcess.Start();
            javaRestServerProcess.OutputDataReceived += (sender, args) => {
                if (!string.IsNullOrEmpty(args.Data)){
                    //Log all Rest-Server Data
                    //UnityEngine.Debug.Log(args.Data);
                    if (args.Data.Contains("SERVER_PID:")) {
                        restServerPID = int.Parse(Regex.Match(args.Data, @"SERVER_PID: (\d+)").Groups[1].Value);
                        UnityEngine.Debug.Log("Rest-Server PID: " + restServerPID);
                        javaRestServerProcess.CancelOutputRead();
                    }
                }
            }; 
            javaRestServerProcess.BeginOutputReadLine();
            if (javaRestServerProcess.HasExited)
                UnityEngine.Debug.LogError("Failed to start NPP-Rest-Server");
            else
                UnityEngine.Debug.Log("NPP-Rest-Server started");
        }
    }

    void Update()
    {
       
    }

    void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("Shutting down REST-Server after " + Time.time + " seconds");
        if (START_REST_SERVER && javaRestServerProcess != null && !javaRestServerProcess.HasExited)
        {
            try {
                javaRestServerProcess.Kill();
                
                if (restServerPID != 0)
                    Process.GetProcessById(restServerPID).Kill();

                javaRestServerProcess.WaitForExit(5000);
                if (javaRestServerProcess.HasExited)
                    UnityEngine.Debug.Log("NPP-Rest-Server process terminated.");
            } catch (Exception e) {
                UnityEngine.Debug.LogError("Failed to stop NPP-Rest-Server: " + e.Message);
            } finally {
                javaRestServerProcess.Dispose();
            }
        }
    }
}