using System;
using System.Collections.Generic;
using System.Linq;

namespace CSEvaluator.Controllers
{
    public class RandomUtils
    {
        private string[] alphabet = "abcdefgijklmnopqrstuxyz1234567890".Split("");

        public string getSorterString()
        {
            int currentIndex = alphabet.Length;
            Random random = new Random();

            while (currentIndex != 0)
            {
                int randomIndex = (int)(Math.Floor(random.NextDouble() * currentIndex));
                currentIndex--;
                var tmp = alphabet[currentIndex];
                alphabet[currentIndex] = alphabet[randomIndex];
                alphabet[randomIndex] = tmp;
            }
            return String.Concat(alphabet);
        }

        public string RandomString(int maxLength)
        {
            Random random = new Random();
            String string1 = random.Next().ToString();
            String string2 = random.Next().ToString();
            string1 = string1.Substring(2, Math.Min(maxLength - 1, string1.Length - 3));
            string2 = string2.Substring(2, Math.Min(maxLength - 1, string2.Length - 3));
            return string1 + string2;
        }

        public int RandomInt(int min, int max) 
        {
            Random random = new Random();
            return random.Next(max - min + 1) + min;
        }

        public char RandomChar()
        {
            int upperCase = RandomInt(1, 2);
            char result = (char)(new Random().Next(26) + 'a');
            return (upperCase % 2 == 0) ? result : Char.ToUpper(result);
        }

        public String[] RemoveStringFromArray(string[] array, string toBeRemoved) 
        {
            List<string> asList = new List<string>(array);
            asList.Remove(toBeRemoved);
            return asList.ToArray();
        }
     }
}