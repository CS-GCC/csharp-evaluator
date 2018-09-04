using System;
using System.Collections.Generic;
using System.Linq;

namespace CSEvaluator.Controllers
{
    public class TestService
    {
        private string[] alphabet = "abcdefgijklmnopqrstuxyz1234567890".Split("");

        public RandomUtils utils = new RandomUtils();

        public string ReverseString(String s) {
            Char[] array = s.ToCharArray();
            Array.Reverse(array);
            return String.Concat(array);
        }

        public Dictionary<Char, int> groupBy(List<Char> list) {
            Dictionary<Char, int> result = new Dictionary<char, int>();
            foreach (Char chr in list) {
                if (!result.ContainsKey(chr)) {
                    result.Add(chr, 1);
                } else {
                    result[chr] = result[chr] + 1;
                }
            }
            return result;
        }

        public char GetMaxOccurrence(Dictionary<Char, int> dictionary) {
            int max = 0;
            char result = '-';
            foreach (KeyValuePair<Char, int> dictionaryItem in dictionary) {
                if (dictionaryItem.Value > max) {
                    max = dictionaryItem.Value;
                    result = dictionaryItem.Key;
                }
            }
            return result;
        }

        public string MoveToBeginning(string str, string argv) {
            string tmp = str.Substring(str.Length - int.Parse(argv), int.Parse(argv) - 1);
            str = str.Substring(0, str.Length - int.Parse(argv) - 1);
            return String.Concat(str);
        }

        public string SortString(string sorterString, string str) {
            IEnumerable<Char> characters = str.ToCharArray();
            characters.OrderBy(
                character => sorterString.IndexOf(character)
            );
            return new string(characters.ToArray());
        }

    }
}
