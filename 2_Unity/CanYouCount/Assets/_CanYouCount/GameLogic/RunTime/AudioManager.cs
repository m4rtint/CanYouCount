using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioModel _model;
    private AudioSource _sfxAudioSource;

    // Start is called before the first frame update
    private void Awake()
    {
        _sfxAudioSource = GetComponent<AudioSource>();
    }

    private void SetupAudioSource()
    {
        _sfxAudioSource = GetComponent<AudioSource>();
        if (_sfxAudioSource == null)
        {
            _sfxAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void PlayOneShot(AudioClip clip)
    {
        _sfxAudioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Plaies the tile click correct.
    /// </summary>
    public void PlayTileClickCorrect()
    {
        PlayOneShot(_model.TileClickCorrect);
    }

    /// <summary>
    /// Plaies the tile click wrong.
    /// </summary>
    public void PlayTileClickWrong()
    {
        PlayOneShot(_model.TileClickWrong);
    }

    /// <summary>
    /// Plaies the count down.
    /// </summary>
    public void PlayCountDown()
    {
        PlayOneShot(_model.CountDown);
    }

    /// <summary>
    /// Plaies the UIB utton click.
    /// </summary>
    public void PlayUIButtonClick()
    {
        PlayOneShot(_model.UIButtonClick);  
    }

    /// <summary>
    /// Plaies the state of the window.
    /// </summary>
    public void PlayWinState()
    {
        PlayOneShot(_model.WinState);
    }

    /// <summary>
    /// Plaies the state of the lose.
    /// </summary>
    public void PlayLoseState()
    {
        PlayOneShot(_model.LoseState);
    }


}
