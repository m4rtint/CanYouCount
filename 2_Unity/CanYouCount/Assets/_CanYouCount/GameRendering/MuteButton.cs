using UnityEngine.UI;
using UnityEngine;

namespace CanYouCount
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class MuteButton : MonoBehaviour
    {
        // TODO : THIS SHOULD NOT HAVE DIRECT ACCES TO AUDIO MANAGER
        [SerializeField]
        private AudioManager _manager = null;

        [SerializeField]
        private Sprite _muted;

        [SerializeField]
        private Sprite _unmuted;

        private Button _muteButton;
        private Image _muteImage;

        private void Awake()
        {
            SetupButton();
            SetupImage();
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

        private void switchMuteState()
        {
            _manager.IsMuted = !_manager.IsMuted;
            Sprite sprite = _unmuted;
            if (_manager.IsMuted)
            {
                sprite = _muted;
            }

            _muteImage.sprite = sprite;
        }
    }
}