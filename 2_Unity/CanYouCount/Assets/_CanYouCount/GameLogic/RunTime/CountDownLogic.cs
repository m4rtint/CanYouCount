using UnityEngine;
namespace CanYouCount
{
    public class CountDownLogic
    {
        private int _countDownValue;
        private static string[] _canYouCount = GameUIContent.CanYouCount;

        /// <summary>
        /// Gets the count down value.
        /// </summary>
        /// <value>The count down value.</value>
        public int CountDownValue => _countDownValue;

        public CountDownLogic()
        {
            ResetCountDownValue();
        }

        /// <summary>
        /// Gets the count down text with.
        /// </summary>
        /// <returns>The count down text with.</returns>
        /// <param name="value">Value.</param>
        public string GetCountDownText()
        {
             _countDownValue = Mathf.Clamp(_countDownValue, 0, _canYouCount.Length - 1);
            return _canYouCount[_countDownValue];
        }

        /// <summary>
        /// Decrements the count down.
        /// </summary>
        /// <returns>The count down.</returns>
        public int DecrementCountDown()
        {
            _countDownValue--;
            return _countDownValue;
        }

        public void ResetCountDownValue()
        {
            _countDownValue = 2;
        }

    }
}