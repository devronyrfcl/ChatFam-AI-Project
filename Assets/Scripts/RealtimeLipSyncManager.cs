using System.Collections.Generic;
using UnityEngine;
using uLipSync;

[System.Serializable]
public class PhonemeBlendShapeInfo
{
    public string phoneme;
    public string blendShape;
}

public class RealtimeLipSyncManager : MonoBehaviour
{
    public GameObject target;
    public uLipSync.Profile profile;
    public string skinnedMeshRendererName = "MTH_DEF";
    public List<PhonemeBlendShapeInfo> phonemeBlendShapeTable = new List<PhonemeBlendShapeInfo>();

    private uLipSync.uLipSync _lipSync;
    private uLipSyncBlendShape _blendShape;
    private AudioSource _audioSource;

    void Start()
    {
        // Setup SkinnedMeshRenderer
        var targetTform = uLipSync.Util.FindChildRecursively(target.transform, skinnedMeshRendererName);
        var smr = targetTform.GetComponent<SkinnedMeshRenderer>();

        // Add uLipSyncBlendShape
        _blendShape = target.AddComponent<uLipSyncBlendShape>();
        _blendShape.skinnedMeshRenderer = smr;

        foreach (var info in phonemeBlendShapeTable)
        {
            _blendShape.AddBlendShape(info.phoneme, info.blendShape);
        }

        // Add AudioSource
        _audioSource = target.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        // Add uLipSync and connect
        _lipSync = target.AddComponent<uLipSync.uLipSync>();
        _lipSync.profile = profile;
        _lipSync.onLipSyncUpdate.AddListener(_blendShape.OnLipSyncUpdate);
    }

    // Call this to play a new audio clip and sync lips
    public void PlayClip(AudioClip clip)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
