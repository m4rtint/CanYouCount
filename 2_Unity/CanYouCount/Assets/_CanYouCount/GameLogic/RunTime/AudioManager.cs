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

    public void PlayCountDown()
    {
        PlayOneShot(_model.CountDown);
    }

    public void PlayUIButtonClick()
    {
        PlayOneShot(_model.UIButtonClick);  
    }

    public void PlayWinState()
    {
        PlayOneShot(_model.WinState);
    }

    public void PlayLoseState()
    {
        PlayOneShot(_model.LoseState);
    }


}
