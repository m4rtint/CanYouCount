using System;
using UnityEngine;
using TMPro;

namespace CanYouCount
{
    public class CountDownTimer : MonoBehaviour
    {
        public Action OnComplete;

        [SerializeField]
        private float _timeTakenToAnimate = 1f;
        [SerializeField]
        private TMP_Text _countDownText = null;

        /// <summary>
        /// Shows the count down.
        /// </summary>
        public void StartCountDownFrom(int value)
        {
            AnimateValue(value);
        }

        private void AnimateValue(int value)
        { 
            transform.localScale = Vector3.zero;
            var seq = LeanTween.sequence();

            var scaleDescr = transform.LeanScale(Vector3.one, _timeTakenToAnimate).setEaseOutBack();
            seq.append(scaleDescr);
            seq.append(() =>
            {
                value--;
                SetCountDownText(value.ToString());
                if (value > 0) 
                {
                    AnimateValue(value);
                }
                else
                {
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
            });

        }

        private void SetCountDownText(string text)
        {
            _countDownText.text = text;
        }

        private void Awake()
        {
            SetupText();
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
