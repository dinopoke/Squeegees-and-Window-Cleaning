using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : MonoBehaviour {

    public static SoundPool instance;

    public PlaySound playSoundPrefab;
    private int soundPoolDefaultCapacity = 16;
    private int soundPoolOverflowMax = 32;
    private int overFlowAdded = 0;

    public List<PlaySound> playSoundPrefabPool;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

    }

    private void Start() {
        playSoundPrefabPool = new();
        for (int i = 0; i < soundPoolDefaultCapacity; i++) {
            PlaySound prefab = Instantiate(playSoundPrefab);
            prefab.gameObject.SetActive(false);
            prefab.gameObject.transform.parent = transform;
            playSoundPrefabPool.Add(prefab);
        }
    }

    public PlaySound GetSoundPrefab() {
        for (int i = 0; i < playSoundPrefabPool.Count; i++) {
            if (!playSoundPrefabPool[i].gameObject.activeInHierarchy) {
                return playSoundPrefabPool[i];
            }
        }

        if (overFlowAdded < (soundPoolOverflowMax - soundPoolDefaultCapacity)) {
            PlaySound prefab = Instantiate(playSoundPrefab);
            prefab.gameObject.transform.parent = transform;
            prefab.gameObject.SetActive(false);
            playSoundPrefabPool.Add(prefab);
            overFlowAdded++;
            return prefab;
        }
            return null;
    }

    public void ReturnSoundToPool(PlaySound prefab) {
        if (overFlowAdded > 0) {
            playSoundPrefabPool.Remove(prefab);
            Destroy(prefab);
            overFlowAdded--;
        } else {
            prefab.gameObject.SetActive(false);
        }
    }

}