using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Audio/AudioClips", order = 1)]
public class AudioModel : ScriptableObject
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _tileClickCorrect;

    [SerializeField]
    private AudioClip _tileClickWrong;

    [SerializeField]
    private AudioClip _userInterfaceButtonClick;

    [SerializeField]
    private AudioClip _countDown;

    [SerializeField]
    private AudioClip _winState;

    [SerializeField]
    private AudioClip _loseState;

    /// <summary>
    /// Gets the tile click.
    /// </summary>
    /// <value>The tile click.</value>
    public AudioClip TileClickCorrect => _tileClickCorrect;

    /// <summary>
    /// Gets the tile click wrong.
    /// </summary>
    /// <value>The tile click wrong.</value>
    public AudioClip TileClickWrong => _tileClickWrong;

    /// <summary>
    /// Gets the UIB utton click.
    /// </summary>
    /// <value>The UIB utton click.</value>
    public AudioClip UIButtonClick => _userInterfaceButtonClick;

    /// <summary>
    /// Gets the count down.
    /// </summary>
    /// <value>The count down.</value>
    public AudioClip CountDown => _countDown;

    /// <summary>
    /// Gets the state of the window.
    /// </summary>
    /// <value>The state of the window.</value>
    public AudioClip WinState => _winState;

    /// <summary>
    /// Gets the state of the lose.
    /// </summary>
    /// <value>The state of the lose.</value>
    public AudioClip LoseState => _loseState;
}
