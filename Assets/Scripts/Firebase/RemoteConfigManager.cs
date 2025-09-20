using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.RemoteConfig;
using Firebase.Extensions;

public class RemoteConfigManager : MonoBehaviour
{
    private void Start()
    {
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase dependencies available. Initializing Remote Config...");
                FetchData();
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies: " + task.Result);
            }
        });
    }

    void FetchData()
    {
        // Minimum fetch interval (for testing use 0, in production maybe 3600s)
        var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);

        fetchTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Remote Config fetch completed. Activating...");
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task2 =>
                    {
                        if (task2.IsCompleted)
                        {
                            Debug.Log("Remote Config values activated!");
                            LogAllRemoteConfigValues();
                        }
                    });
            }
            else
            {
                Debug.LogError("Remote Config fetch failed: " + task.Exception);
            }
        });
    }

    void LogAllRemoteConfigValues()
    {
        IDictionary<string, ConfigValue> allValues = FirebaseRemoteConfig.DefaultInstance.AllValues;

        foreach (var item in allValues)
        {
            Debug.Log($"[RemoteConfig] Key: {item.Key}, Value: {item.Value.StringValue}");
        }
    }
}
