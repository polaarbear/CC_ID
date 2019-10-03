using System;

namespace CC_ID
{
    class Program
    {
        private static bool checkMoreNumbers = true;
        private static string ccNumber;
        private static int checkDigit = 0;
        private static string nonCheckFlipped;
        private static int[] preLuhnValues;
        private static int luhnSum = 0;
        private static bool numberValid = true;
        
        static void Main(string[] args)
        {
            do
            {
                CheckCardNumber();
                PromptTryAgain();
            }
            while (checkMoreNumbers);

            Console.WriteLine("Ok, see you!\nPress the any key to exit!");
            Console.Read();
        }

        private static void CheckCardNumber()
        {
            Console.WriteLine("Enter a credit card number to check.");
            ccNumber = Console.ReadLine();
            String resultString = "";

            ParseCheckAndFlip();
            if (numberValid)
            {
                ParseNonCheck();
                if (numberValid)
                {
                    SumNonCheck();

                    if ((checkDigit + luhnSum) % 10 == 0)
                    {
                        resultString = "VALID, ";
                    }
                    else
                    {
                        numberValid = false;
                        resultString = "INVALID, CHECKSUM FAILURE!";
                    }
                }
                else
                {
                    resultString = "INVALID, NON-NUMERIC CHECK DIGIT";
                }
            }
            else
            {
                resultString = "INVALID, NON-NUMERIC MAIN DIGIT";
            }

            if (numberValid)
            {
                resultString += GetCardIssuer();
            }

            Console.WriteLine(resultString);
        }

        private static void ParseCheckAndFlip()
        {
            bool firstDigit = true;
            for (int value = ccNumber.Length - 1; value >= 0; value--)
            {
                if (firstDigit)
                {
                    numberValid = int.TryParse(ccNumber[value].ToString(), out checkDigit);
                    firstDigit = false;
                }
                else
                {
                    nonCheckFlipped += ccNumber[value];
                }

            }
        }

        private static void ParseNonCheck()
        {
            int nonCheckDigits = nonCheckFlipped.Length;
            preLuhnValues = new int[nonCheckDigits];

            for (int i = 0; i < nonCheckDigits; i++)
            {
                numberValid = int.TryParse(nonCheckFlipped[i].ToString(), out preLuhnValues[i]);
            }
        }

        private static int SumNonCheck()
        {
            for (int preLuhn = 0; preLuhn < preLuhnValues.Length; preLuhn++)
            {
                if (preLuhn % 2 == 0)
                {
                    luhnSum += CalculateLuhn(preLuhnValues[preLuhn]);
                }
                else
                {
                    luhnSum += preLuhnValues[preLuhn];
                }
            }
            return luhnSum;
        }

        private static int CalculateLuhn(int value)
        {
            value *= 2;
            if (value >= 10)
            {
                value = 1 + (value % 10);
            }
            return value;
        }

        private static string GetCardIssuer()
        {
            if(ccNumber[0] == '3')
            {
                switch(ccNumber[1])
                {
                    case '4':
                    case '7':
                        return "AMEX";
                    case '5':
                        return "JCB";
                    case '6':
                    case '8':
                        return "DINER'S CLUB";
                    default:
                        return "ISSUER UNKNOWN";
                }
            }
            else if (ccNumber[0] == '4')
            {
                return "VISA";
            }
            else if (ccNumber[0] == '5')
            {
                switch (ccNumber[1])
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                        return "MASTERCARD";
                    default:
                        return "ISSUER UNKNOWN";
                }
            }
            else if (ccNumber[0] == '6')
            {
                return "DISCOVER";
            }
            return "ISSUER UNKNOWN";
        }

        private static void PromptTryAgain()
        {
            Console.WriteLine("Check another card? Y/N");
            char response = Console.ReadKey(true).KeyChar;

            if (response == 'Y' || response == 'y')
            {
                ResetForm();
            }
            else if (response == 'N' || response == 'n')
            {
                checkMoreNumbers = false;
            }
            else
            {
                Console.WriteLine("Invalid response, try again.");
                PromptTryAgain();
            }
        }

        private static void ResetForm()
        {
            ccNumber = "";
            checkDigit = 0;
            nonCheckFlipped = "";
            preLuhnValues = null;
            luhnSum = 0;
            numberValid = true;
            Console.Clear();
        }
    }
}
