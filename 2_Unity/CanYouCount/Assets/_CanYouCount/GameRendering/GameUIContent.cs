using System;

namespace CanYouCount
{
    public static class GameUIContent
    {
        public static string NextTile => "<size=25%>Next\n\n<size=100%><align=\"center\">{0}";

        public static string TwoDecimalPoint => "<size=25%>Time Elapsed\n\n<size=100%>{0:0.##}";
    }
}
