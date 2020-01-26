﻿using System;
using UnityEngine;
using TMPro;

namespace CanYouCount
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MainScreenTextRenderer : MonoBehaviour
    {
        /// <summary>
        /// The on count down complete.
        /// </summary>
        public Action OnCountDownComplete;

        /// <summary>
        /// The on game over complete.
        /// </summary>
        public Action OnGameOverComplete;

        [SerializeField]
        private float _timeTakenToAnimate = 1f;
        [SerializeField]
        private TMP_Text _mainScreenText = null;
        private CanvasGroup _canvasGroup = null;
        private MainScreenTextLogic _mainScreenTextLogic = null;


        /// <summary>
        /// Shows the count down.
        /// </summary>
        public void StartCountDown()
        {
            AnimateCountDown();
        }

        /// <summary>
        /// Starts the game over.
        /// </summary>
        public void StartGameOver()
        {
            AnimateGameOver();
        }

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 1;
        }

        private void AnimateGameOver()
        {
            Reset();
            SetMainScreenText(GameUIContent.GameOver);
            transform.LeanScale(Vector3.one, _timeTakenToAnimate)
                .setEase(LeanTweenType.easeOutSine)
                .setOnComplete(() =>
            {
                OnGameOverComplete?.Invoke();
            });
        }


        private void AnimateCountDown()
        {
            Reset();
            SetMainScreenText(_mainScreenTextLogic.GetCountDownText());
            var seq = LeanTween.sequence();

            var scaleDescr = transform.LeanScale(Vector3.one, _timeTakenToAnimate).setEaseOutBack();
            seq.append(scaleDescr);
            if (_mainScreenTextLogic.CountDownValue == 0)
            {
                seq.append(LeanTween.alphaCanvas(_canvasGroup, 0, _timeTakenToAnimate / 2));
            }

            seq.append(() => OnAnimationComplete());
        }

        private void OnAnimationComplete()
        {
            if (_mainScreenTextLogic.DecrementCountDown() >= 0)
            {
                AnimateCountDown();
            }
            else
            {
                OnCountDownComplete?.Invoke();
                Reset();
            }
        }

        private void SetMainScreenText(string text)
        {
            _mainScreenText.text = text;
        }

        private void Awake()
        {
            SetupText();
            SetupCanvasGroup();
            _mainScreenTextLogic = new MainScreenTextLogic();
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
            if (_mainScreenText == null)
            {
                _mainScreenText = GetComponentInChildren<TMP_Text>();
            }
        }
    }
}