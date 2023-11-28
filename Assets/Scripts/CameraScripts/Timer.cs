using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText;

    private float _elapsedTime;
    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int second = Mathf.FloorToInt(_elapsedTime % 60);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, second);
    }
}
