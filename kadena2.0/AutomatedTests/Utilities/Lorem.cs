using System.Linq;

namespace AutomatedTests.Utilities
{
    /// <summary>
    /// Generates random 'Lorem ipsum' text
    /// </summary>
    public class Lorem
    {
        private const string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean faucibus aliquet dolor. Vivamus condimentum hendrerit ligula non tempus. Duis in risus eu arcu viverra tempor non vitae mauris. Aenean congue accumsan augue, ac suscipit nisl tempor ac. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Phasellus massa nisl, finibus quis interdum eu, mollis pretium lacus. Nunc facilisis odio et nulla feugiat euismod. Etiam tempor varius neque, sed elementum lacus varius vitae. Donec ultrices nunc enim, eu venenatis nunc vehicula non. Nulla facilisi. Vivamus scelerisque libero est, venenatis bibendum nulla volutpat sed. Nullam pellentesque odio mauris, eget eleifend dui congue at. Nulla aliquet metus eget nisi scelerisque vehicula. Cras tempor tempus est et vestibulum";

        /// <summary>
        /// Whole string of lorem ipsum
        /// </summary>
        public static string Ipsum { get { return lorem; } }

        /// <summary>
        /// Gets one word from Lorem ipsum
        /// </summary>
        /// <returns>Random word from Lorem ipsum</returns>
        public static string Word()
        {
            string[] words = lorem.Replace(".", "").Replace(",", "").Split(' ');
            return words[Browser.Random.Next(0, words.Count())];
        }

        /// <summary>
        /// Gets one word from Lorem ipsum with given length
        /// </summary>
        /// <param name="length">Length of the word</param>
        /// <returns>Random word from Lorem ipsum</returns>
        public static string Word(int length)
        {
            string[] words = lorem.Replace(".", "").Replace(",", "").Split(' ').Where(r => r.Length >= length).ToArray();
            return words[Browser.Random.Next(0, words.Count())];
        }

        /// <summary>
        /// Gets one random sentence from Lorem ipsum
        /// </summary>
        /// <returns>Random sentence</returns>
        public static string Sentence()
        {
            string[] sentence = lorem.Replace(". ", ".").Split('.');
            return sentence[Browser.Random.Next(0, sentence.Count())];
        }

        /// <summary>
        /// Gets number of sentences from lorem ipsum 
        /// </summary>
        /// <param name="sentences">Number of sentences in paragraph</param>
        /// <returns>Random paragraph</returns>
        public static string Paragraph(int sentences)
        {
            string[] sentence = lorem.Replace(". ", ".").Split('.');
            string paragraph = string.Empty;

            for (int i = 0; i < sentences; i++)
            {
                paragraph += sentence[Browser.Random.Next(0, sentence.Count())] + ". ";
            }

            return paragraph;
        }

        /// <summary>
        /// Gets random time period
        /// </summary>
        /// <returns>Time period</returns>
        public static string RandomPeriod()
        {
            string[] period = { "denně", "týdně", "měsíčně", "ročně" };
            return period[Browser.Random.Next(0, period.Count() - 1)];
        }

        /// <summary>
        /// Gets random name for person
        /// </summary>
        /// <returns>Random title, first name and last name</returns>
        public static string RandomName()
        {
            string[] title = { "", "MUDr. ", "Ing. ", "Mgr. ", "Bc. ", "MVDr. ", "Phd. " };
            string[] name = { "David ", "Jan ", "Jiří ", "Petr ", "Igor ", "Michal ", "Miroslav " };
            string[] surname = { "Novák", "Svoboda", "Novotný", "Dvořák", "Černý", "Veselý", "Procházka" };

            return title[Browser.Random.Next(0, title.Count() - 1)] + name[Browser.Random.Next(0, name.Count() - 1)] + surname[Browser.Random.Next(0, surname.Count() - 1)];

        }

        /// <summary>
        /// Returns random nuber in given borders
        /// </summary>
        /// <param name="min">Minimum random number</param>
        /// <param name="max">Maximum random number</param>
        /// <returns>Random number</returns>
        public static int RandomNumber(int min, int max)
        {
            return Browser.Random.Next(min, max);
        }

    }
}
