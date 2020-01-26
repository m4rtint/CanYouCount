namespace CanYouCount
{
    public static class GameUIContent
    {
        /// <summary>
        /// Gets the two decimal point.
        /// </summary>
        /// <value>The two decimal point.</value>
        public static string TwoDecimalPoint => "{0:0.##}";

        private static string Can => "CAN";

        private static string You => "YOU";

        private static string Count => "COUNT";

        public static string[] CanYouCount = new string[] { GameUIContent.Count, GameUIContent.You, GameUIContent.Can };

        public static string GameOver => "<size=50%>GAME OVER";
    }
}
