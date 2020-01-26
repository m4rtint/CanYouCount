using System;
using UnityEngine;
using TMPro;

namespace CanYouCount
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CountDownRenderer : MonoBehaviour
    {
        public Action OnComplete;

        [SerializeField]
        private float _timeTakenToAnimate = 1f;
        [SerializeField]
        private TMP_Text _countDownText = null;
        private CanvasGroup _canvasGroup = null;

        /// <summary>
        /// Shows the count down.
        /// </summary>
        public void StartCountDownFrom(int value)
        {
            AnimateValue(value);
        }

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 1;
        }

        private void AnimateValue(int value)
        {
            Reset();
            var seq = LeanTween.sequence();

            var scaleDescr = transform.LeanScale(Vector3.one, _timeTakenToAnimate).setEaseOutBack();
            seq.append(scaleDescr);
            if (value == 0)
            {
                seq.append(LeanTween.alphaCanvas(_canvasGroup, 0, _timeTakenToAnimate / 2));
            }

            seq.append(() => OnAnimationComplete(value));
        }

        private void OnAnimationComplete(int value)
        {
            value--;
            string textToDisplay = value.ToString();

            if (value >= 0)
            {
                if (value == 0)
                {
                    textToDisplay = "GO";
                }

                SetCountDownText(textToDisplay);
                AnimateValue(value);
            }
            else
            {
                OnComplete?.Invoke();
                Reset();
            }
        }

        private void SetCountDownText(string text)
        {
            _countDownText.text = text;
        }

        private void Awake()
        {
            SetupText();
            SetupCanvasGroup();
        }

        private void SetupCanvasGroup()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        private void SetupText()
        {
            if (_countDownText == null)
            {
                _countDownText = GetComponentInChildren<TMP_Text>();
            }
        }
    }
}
