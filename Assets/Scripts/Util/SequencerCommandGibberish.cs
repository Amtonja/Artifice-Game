using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;


/**
* Most of this is copied from SequencerCommandAudioWait.
**/
namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandGibberish : SequencerCommand
    {
        private float stopTime = 0;
        private AudioSource audioSource = null;
        private int nextClipIndex = 2;
        private AudioClip currentClip = null;
        private AudioClip originalClip = null;

        public IEnumerator Start()
        {
            string audioClipName = "";
            if (Sequencer.entrytag.StartsWith("Evans"))
            {
                audioClipName = "SFX/EvansSpeech";
            }
            else if (Sequencer.entrytag.StartsWith("Hurley"))
            {
                audioClipName = "SFX/HurleySpeech";
            }
            
            Transform subject = GetSubject(1);
            nextClipIndex = 2;
            if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Sequencer: Gibberish({1})", new System.Object[] { DialogueDebug.Prefix, GetParameters() }));
            audioSource = SequencerTools.GetAudioSource(subject);
            if (audioSource == null)
            {
                if (DialogueDebug.LogWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: Gibberish() command: can't find or add AudioSource to {1}.", new System.Object[] { DialogueDebug.Prefix, subject.name }));
                Stop();
            }
            else {
                originalClip = audioSource.clip;
                stopTime = DialogueTime.time + 1; // Give time for yield return null.
                yield return null;
                originalClip = audioSource.clip;
                TryAudioClip(audioClipName);
            }
        }

        private void TryAudioClip(string audioClipName)
        {
            try
            {
                AudioClip audioClip = (!string.IsNullOrEmpty(audioClipName)) ? (DialogueManager.LoadAsset(audioClipName) as AudioClip) : null;
                if (audioClip == null)
                {
                    if (DialogueDebug.LogWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: Gibberish() command: Clip '{1}' wasn't found.", new System.Object[] { DialogueDebug.Prefix, audioClipName }));
                }
                else {
                    if (IsAudioMuted())
                    {
                        if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Sequencer: Gibberish(): waiting but not playing '{1}'; audio is muted.", new System.Object[] { DialogueDebug.Prefix, audioClipName }));
                    }
                    else {
                        if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Sequencer: Gibberish(): playing '{1}'.", new System.Object[] { DialogueDebug.Prefix, audioClipName }));
                        currentClip = audioClip;
                        audioSource.clip = audioClip;
                        audioSource.Play();
                    }
                }
                stopTime = DialogueTime.time + audioClip.length;
            }
            catch (System.Exception)
            {
                stopTime = 0;
            }
        }

        public void Update()
        {
            if (DialogueTime.time >= stopTime)
            {
                if (nextClipIndex < Parameters.Length)
                {
                    TryAudioClip(GetParameter(nextClipIndex));
                    nextClipIndex++;
                }
                else {
                    Stop();
                }
            }
        }

        public void OnDialogueSystemPause()
        {
            if (audioSource == null) return;
            audioSource.Pause();
        }

        public void OnDialogueSystemUnpause()
        {
            if (audioSource == null) return;
            audioSource.Play();
        }

        public void OnDestroy()
        {
            if (audioSource != null)
            {
                if (audioSource.isPlaying && (audioSource.clip == currentClip))
                {
                    audioSource.Stop();
                }
                audioSource.clip = originalClip;
            }
        }


    }

}