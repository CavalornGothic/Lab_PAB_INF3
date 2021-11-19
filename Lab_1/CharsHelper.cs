namespace Lab_PAB_INF3
{
    static public class CharsHelper
    {
        public static string SpecialChars(string text)
        {
            return text.Replace('"', '\"').Replace("'", "''");
        }
    }
}
