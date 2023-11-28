using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerScript : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicTracks; // ������ � ������ ������������ �������

    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // ���������, ���� �� ����������� ����� � �������
        if (musicTracks.Length > 0)
        {
            // �������� ��������������� ������� �����
            PlayNextTrack();
        }
    }

    void Update()
    {
        // ���������, ���������� �� ������� ����
        if (!audioSource.isPlaying)
        {
            // ������������� ��������� ����
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        // �������� ��������� ���� �� �������
        audioSource.clip = musicTracks[currentTrackIndex];
        // ������������� ��������� ����
        audioSource.Play();

        // ����������� ������ ��� ���������� �����
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
    }
}
