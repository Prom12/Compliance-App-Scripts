using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverAudio : MonoBehaviour
{
    [SerializeField] public GeneralScriptableObj GeneralScriptableObj;
    public List<AudioVoiceOvers> AudioVoiceOvers;
    private AudioSource audioSource;
    private int currentClipIndex = 0;
    private bool playClipEnd = false;
    public bool audioFinished = false;
    // Start is called before the first frame update
    void Start()
    {
        AudioVoiceOvers = GeneralScriptableObj.AudioVoiceOvers;
        audioSource = GetComponent<AudioSource>();
        // videoPlayer.loopPointReached += OnClipEnd;

    }

    public void VoiceOverToPlay(string voiceTitle){

        foreach (AudioVoiceOvers AudioVoiceOver in AudioVoiceOvers){
            if(AudioVoiceOver.voiceTitle == voiceTitle){
                StopAudio();
                audioSource.clip = AudioVoiceOver.clip;
                audioSource.loop = AudioVoiceOver.loopAudio;
                PlayAudio();
                StartCoroutine(WaitForAudioToEnd());
                    if(voiceTitle == VoiceOverTitles.HyeeItsExerciseTime.ToString()){
                        playClipEnd = true;
                        // Invoke("playAnotherAudioAfterFirst", AudioVoiceOver.clip.length);
                    }
            }
        }
    }

    public void VoiceOverAudioPoints(int totalPoints){
        foreach (AudioVoiceOvers AudioVoiceOver in AudioVoiceOvers){
            if(AudioVoiceOver.points == totalPoints && totalPoints != 0){
                StopAudio();
                audioSource.clip = AudioVoiceOver.clip;
                audioSource.loop = AudioVoiceOver.loopAudio;
                PlayAudio();
                StartCoroutine(WaitForAudioToEnd());
                Invoke("playAnotherAudioAfterPoints", AudioVoiceOver.clip.length);
            }
        }
    }

    public void VoiceOverAudioPointsQuit(string voiceTitle){

        foreach (AudioVoiceOvers AudioVoiceOver in AudioVoiceOvers){
            if(AudioVoiceOver.voiceTitle == voiceTitle){
                StopAudio();
                audioSource.clip = AudioVoiceOver.clip;
                audioSource.loop = AudioVoiceOver.loopAudio;
                PlayAudio();
                StartCoroutine(WaitForAudioToEnd());
            }
        }
    }

    private void playAnotherAudioAfterFirst(){
        VoiceOverToPlay(VoiceOverTitles.ChooseBnTOptions.ToString());
        StartCoroutine(WaitForAudioToEnd());

    }
    
    private void playAnotherAudioAfterPoints(){
        VoiceOverToPlay(VoiceOverTitles.RedirectFew.ToString());
        StartCoroutine(WaitForAudioToEnd());
    }

    public void PlayAudio()
    {
        audioFinished = false;
        // Check if the audio clip is not already playing
        if (!audioSource.isPlaying)
        {
            // Start playing the audio clip
            audioSource.Play();
        }
    }

    public void StopAudio()
    {
        // Check if the audio clip is playing
        if (audioSource.isPlaying)
        {
            audioSource.loop = false;
            // Stop playing the audio clip
            audioSource.Stop();
        }
    }

    private IEnumerator WaitForAudioToEnd()
    {
        // Wait until the audio is no longer playing
        yield return new WaitUntil(() => !audioSource.isPlaying);
        audioSource.clip = null;
        // Set the flag to true indicating the audio has finished
        audioFinished = true;
    }

}
