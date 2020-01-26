using System;

namespace CanYouCount
{
    public static class GameUIContent
    {
        /// <summary>
        /// Gets the two decimal point.
        /// </summary>
        /// <value>The two decimal point.</value>
        public static string TwoDecimalPoint => "{0:0.##}";

        /// <summary>
        /// Gets the go text.
        /// </summary>
        /// <value>The go.</value>
        public static string Go => "GO";

        public static string GameOver => "<size=50%>GAME OVER";
    }
}
