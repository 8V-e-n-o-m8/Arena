using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerScript : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicTracks; // ћассив с вашими музыкальными треками

    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // ѕровер€ем, есть ли музыкальные треки в массиве
        if (musicTracks.Length > 0)
        {
            // Ќачинаем воспроизведение первого трека
            PlayNextTrack();
        }
    }

    void Update()
    {
        // ѕровер€ем, закончилс€ ли текущий трек
        if (!audioSource.isPlaying)
        {
            // ¬оспроизводим следующий трек
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        // ¬ыбираем следующий трек из массива
        audioSource.clip = musicTracks[currentTrackIndex];
        // ¬оспроизводим выбранный трек
        audioSource.Play();

        // ”величиваем индекс дл€ следующего трека
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
    }
}
