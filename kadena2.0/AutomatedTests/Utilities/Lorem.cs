using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomatedTests.Utilities
{
    /// <summary>
    /// Generates random 'Lorem ipsum' text
    /// </summary>
    public static class Lorem
    {

        public static Random Random { get; set; }
        private const string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean faucibus aliquet dolor. Vivamus condimentum hendrerit ligula non tempus. Duis in risus eu arcu viverra tempor non vitae mauris. Aenean congue accumsan augue, ac suscipit nisl tempor ac. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Phasellus massa nisl, finibus quis interdum eu, mollis pretium lacus. Nunc facilisis odio et nulla feugiat euismod. Etiam tempor varius neque, sed elementum lacus varius vitae. Donec ultrices nunc enim, eu venenatis nunc vehicula non. Nulla facilisi. Vivamus scelerisque libero est, venenatis bibendum nulla volutpat sed. Nullam pellentesque odio mauris, eget eleifend dui congue at. Nulla aliquet metus eget nisi scelerisque vehicula. Cras tempor tempus est et vestibulum";

        static Lorem()
        {
            Random = new Random();
        }

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
            return words[Random.Next(0, words.Count())];
        }

        /// <summary>
        /// Gets one word from Lorem ipsum with given length
        /// </summary>
        /// <param name="length">Length of the word</param>
        /// <returns>Random word from Lorem ipsum</returns>
        public static string Word(int length)
        {
            string[] words = lorem.Replace(".", "").Replace(",", "").Split(' ').Where(r => r.Length >= length).ToArray();
            return words[Random.Next(0, words.Count())];
        }

        /// <summary>
        /// Gets one random sentence from Lorem ipsum
        /// </summary>
        /// <returns>Random sentence</returns>
        public static string Sentence()
        {
            string[] sentence = lorem.Replace(". ", ".").Split('.');
            return sentence[Random.Next(0, sentence.Count())];
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
                paragraph += sentence[Random.Next(0, sentence.Count())] + ". ";
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
            return period[Random.Next(0, period.Count() - 1)];
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

            return title[Random.Next(0, title.Count() - 1)] + name[Random.Next(0, name.Count() - 1)] + surname[Random.Next(0, surname.Count() - 1)];

        }

        /// <summary>
        /// Returns random nuber in given borders
        /// </summary>
        /// <param name="min">Minimum random number</param>
        /// <param name="max">Maximum random number</param>
        /// <returns>Random number</returns>
        public static int RandomNumber(int min, int max)
        {
            return Random.Next(min, max);
        }

        /// <summary>
        /// Returns message saying it is test
        /// </summary>
        /// <returns></returns>
        public static string ThisIsTestMessage()
        {
            return "This is test. Do not reply";
        }

        /// <summary>
        /// Returns random US address
        /// </summary>
        /// <returns></returns>
        public static Address RandomUSAddress()
        {
            List<Address> addresses = new List<Address>()
            {
                new Address() {ID = 1, AddressLine1 =  "9708 Vermont St.", City = "Kansas City", State = "MO", Zip = "64151"},
                new Address() {ID = 2, AddressLine1 =  "7 Glenwood Street", City = "Glendale", State = "AZ", Zip = "85302"},
                new Address() {ID = 3, AddressLine1 =  "570 Albany Ave.", City = "Bronx", State = "NY", Zip = "10451"},
                new Address() {ID = 4, AddressLine1 =  "8245 George Ave.", City = "Raleigh", State = "NC", Zip = "27603"},
                new Address() {ID = 5, AddressLine1 =  "9802 W. Durham St.", City = "Danvers", State = "MA", Zip = "01923"},
            };

            int random = Random.Next(0, addresses.Count);
            return addresses[random];
        }
    }
}
