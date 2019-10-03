using System;

namespace CC_ID
{
    class Program
    {
        private static string ccNumber;
        private static int checkDigit = 0;
        private static string nonCheckFlipped;
        private static int[] preLuhnValues;
        private static int luhnSum = 0;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a credit card number to check.");
           ccNumber = Console.ReadLine();
            
            if (ParseCheckAndFlip())
            {
                if (ParseNonCheck())
                {
                    luhnSum = SumNonCheck();
                }
                else
                {
                    Console.WriteLine("Failed to parse non-check digits");
                }
            }
            else
            {
                Console.WriteLine("Failed to parse check digit");
            }


            if ((luhnSum + checkDigit) % 10 == 0)
            {
                string validationString = "VALID, " + GetCardIssuer() + ".";

                Console.WriteLine(validationString);
            }
            else
            {
                Console.WriteLine("INVALID CARD!");
            }

            Console.ReadKey();            
        }

        private static bool ParseCheckAndFlip()
        {
            bool firstDigit = true;
            bool parseSuccess = true;
            for (int value = ccNumber.Length - 1; value >= 0; value--)
            {
                if (firstDigit)
                {
                    parseSuccess = int.TryParse(ccNumber[value].ToString(), out checkDigit);
                    firstDigit = false;
                }
                else
                {
                    nonCheckFlipped += ccNumber[value];
                }

            }
            return parseSuccess;
        }

        private static bool ParseNonCheck()
        {
            int nonCheckDigits = nonCheckFlipped.Length;
            preLuhnValues = new int[nonCheckDigits];
            bool parseSuccess = true;

            for (int i = 0; i < nonCheckDigits; i++)
            {
                parseSuccess = int.TryParse(nonCheckFlipped[i].ToString(), out preLuhnValues[i]);
                if (!parseSuccess)
                {
                    return parseSuccess;
                }
            }
            return parseSuccess;
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
    }
}
