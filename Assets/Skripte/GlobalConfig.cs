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

/// <summary>
/// This class contains global configuration values and handles various initialisation and setup tasks.
/// </summary>
public class GlobalConfig : MonoBehaviour
{

    /// <param name="BASE_URL">base URL of the rest server running the simulation</param>
    public static string BASE_URL = "http://localhost:8080/api/";
    /// <param name="CLIENT_UPDATE_INTERVAL">Interval in seconds for between two update requests</param>
    public static float CLIENT_UPDATE_INTERVAL = .1f;
    /// <param name="START_REST_SERVER">boolean flag to start the REST server at the start of the VR application</param>
    public bool START_REST_SERVER;
    /// <param name="javaRestServerProcess">Reference to the REST server process </param>
    private Process javaRestServerProcess;
    /// <param name="restServerPID">PID of the REST server process</param>
    private int restServerPID;

    /// <summary>
    /// This method starts the REST server if specified by the player.
    /// </summary>
    void Start()
    {
        //Start NPP-Rest-Server
        if (START_REST_SERVER)
        {
            string restServerExecutablePath = System.IO.Path.Combine(Application.dataPath, "Skripte", "restapi-vr-1.0.jar");
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

// this method is empty, hence excluded from the documentation

    void Update()
    {
        // pass
    }

    /// <summary>
    /// This method stops the REST Server when the application is closed, if the server is running.
    /// </summary>
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