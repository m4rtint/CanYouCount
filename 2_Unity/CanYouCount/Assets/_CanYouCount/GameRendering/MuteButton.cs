using UnityEngine.UI;
using UnityEngine;

namespace CanYouCount
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class MuteButton : MonoBehaviour
    {
        private AudioManager _manager = null;

        [SerializeField]
        private Sprite _muted = null;

        [SerializeField]
        private Sprite _unmuted = null;

        private Button _muteButton;
        private Image _muteImage;

        public void Initialize(AudioManager manager)
        {
            SetupButton();
            SetupImage();
            _manager = manager;
            UpdateMuteImage();
            _muteButton.onClick.RemoveAllListeners();
            _muteButton.onClick.AddListener(() => switchMuteState());
        }

        private void SetupButton()
        {
            _muteButton = GetComponent<Button>();
            if (_muteButton == null)
            {
                _muteButton = gameObject.AddComponent<Button>();
            }
        }

        private void SetupImage()
        {
            _muteImage = GetComponent<Image>();
            if (_muteImage == null)
            {
                _muteImage = gameObject.AddComponent<Image>();
            }
        }

        private void UpdateMuteImage()
        {
            Sprite sprite = _unmuted;
            if (_manager.IsMuted)
            {
                sprite = _muted;
            }

            _muteImage.sprite = sprite;
        }

        private void switchMuteState()
        {
            _manager.IsMuted = !_manager.IsMuted;
            UpdateMuteImage();
        }
    }
}