using System.Text.RegularExpressions;

namespace Kadena.Helpers
{
    public static class MailValidator
    {
        private static readonly string  RegexText = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                            + "@"
                            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

        private static Regex regex = new Regex(RegexText);

        public static bool IsValid(string email)
        {
            Match match = regex.Match(email);
            return match.Success;
        }
    }
}
