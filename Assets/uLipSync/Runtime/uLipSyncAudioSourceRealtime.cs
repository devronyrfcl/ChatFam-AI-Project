/*using UnityEngine;

namespace uLipSync
{
    [RequireComponent(typeof(AudioSource))]
    public class uLipSyncAudioSourceRealtime : MonoBehaviour
    {
        [Tooltip("When ON, AudioSource playback is automatically used for lip sync")]
        public bool isAutoStart = true;

        [Tooltip("AudioSource to play clips from")]
        public AudioSource source;

        private AudioClip _previousClip;

        private void OnEnable()
        {
            if (!source) source = GetComponent<AudioSource>();
            _previousClip = source.clip;

            if (isAutoStart && source.clip != null)
            {
                source.Play();
            }
        }

        private void Update()
        {
            if (!source) return;

            // Check if audio clip changed in inspector or by script
            if (source.clip != _previousClip)
            {
                _previousClip = source.clip;
                PlayClipRealtime(source.clip);
            }

            // If AudioSource stopped and auto-start is true, restart playback
            if (!source.isPlaying && isAutoStart && source.clip != null)
            {
                source.Play();
            }
        }

        /// <summary>
        /// Play a clip and lip sync in real time
        /// </summary>
        /// <param name="clip">AudioClip to play</param>
        public void PlayClipRealtime(AudioClip clip)
        {
            if (!source) return;
            if (clip == null) return;

            if (source.isPlaying) source.Stop();

            source.clip = clip;
            source.Play();

            // uLipSync reads AudioSource automatically, so no microphone needed
        }
    }
}*/








using UnityEngine;

namespace uLipSync
{
    [RequireComponent(typeof(AudioSource))]
    public class uLipSyncAudioSourceRealtime : MonoBehaviour
    {
        [Tooltip("AudioSource to play clips from")]
        public AudioSource source;

        private AudioClip _previousClip;

        private void OnEnable()
        {
            if (!source) source = GetComponent<AudioSource>();
            _previousClip = source.clip;

            // Don’t auto-play, don’t loop
            source.playOnAwake = false;
            source.loop = false;
        }

        private void Update()
        {
            if (!source) return;

            // Detect if clip changed in inspector
            if (source.clip != _previousClip)
            {
                _previousClip = source.clip;
            }
        }

        /// <summary>
        /// Set a clip and play it once (no loop, no auto-restart).
        /// </summary>
        /// <param name="clip">AudioClip to assign and play</param>
        public void SetAndPlayOnce(AudioClip clip)
        {
            if (!source) return;
            if (clip == null) return;

            if (source.isPlaying) source.Stop();

            source.clip = clip;
            source.loop = false; // ensure one-time play
            _previousClip = clip;
            source.Play();

            // uLipSync reads AudioSource automatically
        }
    }
}

