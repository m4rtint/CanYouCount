using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Audio/AudioClips", order = 1)]
public class AudioModel : ScriptableObject
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _tileClick;

    [SerializeField]
    private AudioClip _userInterfaceButtonClick;

    [SerializeField]
    private AudioClip _countDown;

    [SerializeField]
    private AudioClip _winState;

    [SerializeField]
    private AudioClip _loseState;
}
