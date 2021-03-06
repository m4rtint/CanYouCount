﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace CanYouCount 
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioModel _model = null;
        private AudioSource _sfxAudioSource;
        private bool _isMuted;
        private const string MuteKey = "MUTE_KEY";

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:CanYouCount.AudioManager"/> is mute.
        /// </summary>
        /// <value><c>true</c> if is mute; otherwise, <c>false</c>.</value>
        [ShowInInspector]
        public bool IsMuted
        {
            get
            {
                return _isMuted;
            }

            set
            {
                OnSetMute(value);
            }
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            SetupAudioSource();
            IsMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;
        }

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

        private void OnSetMute(bool isMute)
        {
            _isMuted = isMute;
            _sfxAudioSource.mute = _isMuted;
            PlayerPrefs.SetInt(MuteKey, _isMuted ? 1 : 0);
        }

        private void PlayOneShot(AudioClip clip)
        { 
            _sfxAudioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Plaies the tile click correct.
        /// </summary>
        [Button("Play Tile Correct")]
        public void PlayTileClickCorrect()
        {
            PlayOneShot(_model.TileClickCorrect);
        }

        /// <summary>
        /// Plaies the tile click wrong.
        /// </summary>
        [Button("Play Tile Wrong")]
        public void PlayTileClickWrong()
        {
            PlayOneShot(_model.TileClickWrong);
        }

        /// <summary>
        /// Plaies the count down.
        /// </summary>
        [Button("Play Count Down")]
        public void PlayCountDown()
        {
            PlayOneShot(_model.CountDown);
        }

        /// <summary>
        /// Plaies the UIB utton click.
        /// </summary>
        [Button("Play UI Button Click")]
        public void PlayUIButtonClick()
        {
            PlayOneShot(_model.UIButtonClick);
        }

        /// <summary>
        /// Plaies the state of the window.
        /// </summary>
        [Button("Play Win State")]
        public void PlayWinState()
        {
            PlayOneShot(_model.WinState);
        }

        /// <summary>
        /// Plaies the state of the lose.
        /// </summary>
        [Button("Play Lose State")]
        public void PlayLoseState()
        {
            PlayOneShot(_model.LoseState);
        }
    }
}
