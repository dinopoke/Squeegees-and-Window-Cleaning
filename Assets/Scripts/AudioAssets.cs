using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAssets : MonoBehaviour {

    public static AudioAssets instance;    

    [SerializeField] private GlobalSound[] globalSounds;
    [SerializeField] private SpatialSound[] spatialSounds;

    private void Awake() {

        if (instance != null) {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

        foreach (GlobalSound globalSound in globalSounds) {
            globalSound.audioSource = this.gameObject.AddComponent<AudioSource>();
            globalSound.audioSource.clip = globalSound.audioClip;
            globalSound.audioSource.volume = globalSound.volume;
            globalSound.audioSource.pitch = globalSound.pitch;
        }
    }

    public SoundAudioClip[] SoundAudioClipArray;
    public SoundAudioClip[] GlobalSoundClipArray;

    [System.Serializable]
    public class SoundAudioClip {
        public AudioManager.Sound sound;
        public AudioClip audioClip;
    }

    private void Start() {
        //SceneHandler.OnQuitToMainMenu_Event += PauseMenu_OnQuitToMainMenu_Event;
    }

    private void PauseMenu_OnQuitToMainMenu_Event() {
        Destroy(this.gameObject);
    }

    private void OnDisable() {
        //SceneHandler.OnQuitToMainMenu_Event -= PauseMenu_OnQuitToMainMenu_Event;
    }

}


[System.Serializable]
public class GlobalSound {

    public string name;
    public AudioClip audioClip;

    [Range(0 ,1)] public float volume = 1.0f;
    [Range(.1f, 3f)] public float pitch = 1.0f;

    [HideInInspector] public AudioSource audioSource;
}

[System.Serializable]
public class SpatialSound {

    public string name;
    public AudioClip audioClip;

    [Range(0 ,1)] public float volume = 1.0f;
    [Range(.1f, 3f)] public float pitch = 1.0f;
    [Range(0 ,1)] public float spatial = 1.0f;

}
