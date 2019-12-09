using System;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace Rules
{
    public class CodigoRandom
    {

        string keyLetters;
        string keyNumbers;
        int keyChars;
        char[] lettersArray;

        char[] numbersArray;

        public string KeyLetters
        {
            set { keyLetters = value; }
        }

        public string KeyNumbers
        {
            set { keyNumbers = value; }
        }

        public int KeyChars
        {
            set { keyChars = value; }
        }


        public string Generate()
        {
            int iKey = 0;
            float random1 = 0;
            Int16 arrIndex = default(Int16);
            StringBuilder sb = new StringBuilder();
            string randomLetter = null;


            lettersArray = keyLetters.ToCharArray();
            numbersArray = keyNumbers.ToCharArray();

            for (iKey = 1; iKey <= keyChars; iKey++)
            {

                VBMath.Randomize();
                random1 = VBMath.Rnd();
                arrIndex = -1;

                if ((Convert.ToInt32(random1 * 111)) % 2 == 0)
                {

                    while (arrIndex < 0)
                    {
                        arrIndex = Convert.ToInt16(lettersArray.GetUpperBound(0) * random1);
                    }
                    randomLetter = lettersArray[arrIndex].ToString();

                    if ((Convert.ToInt32(arrIndex * random1 * 99)) % 2 != 0)
                    {
                        randomLetter = lettersArray[arrIndex].ToString();
                        randomLetter = randomLetter.ToUpper();
                    }
                    sb.Append(randomLetter);
                }
                else
                {

                    while (arrIndex < 0)
                    {
                        arrIndex = Convert.ToInt16(numbersArray.GetUpperBound(0) * random1);
                    }
                    sb.Append(numbersArray[arrIndex]);
                }
            }
            return sb.ToString();
        }


    }
}
