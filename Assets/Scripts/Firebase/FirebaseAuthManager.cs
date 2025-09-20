using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseAuthManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text statusText; // assign in Inspector

    private FirebaseAuth auth;

    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(dependencyTask =>
        {
            if (dependencyTask.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                statusText.text = "Firebase initialized.";
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyTask.Result);
                statusText.text = "Firebase init failed.";
            }
        });
    }

    /// <summary>
    /// Call this via a button OnClick
    /// </summary>
    public void LoginAsGuest()
    {
        if (auth == null)
        {
            statusText.text = "Auth not ready!";
            return;
        }

        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Anonymous sign-in was canceled.");
                statusText.text = "Login canceled.";
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("Anonymous sign-in encountered an error: " + task.Exception);
                statusText.text = "Login error.";
                return;
            }

            // Here task.Result is of type AuthResult
            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;  // extract FirebaseUser
            string userId = newUser.UserId;

            Debug.Log($"✅ Login Successful! UserID: {userId}");
            statusText.text = $"Login Successful!\nUserID: {userId}";
        });
    }
}
