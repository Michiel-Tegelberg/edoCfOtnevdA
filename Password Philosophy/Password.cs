using System;

namespace Password_Philosophy
{
    class Password
    {
        public int Low { get; set; }
        public int High { get; set; }
        public char Letter { get; private set; }
        public string Pass { get; private set; }

        public Password(int low, int high, char letter, string pass)
        {
            Low = low;
            High = high;
            Letter = letter;
            Pass = pass;
        }

        public bool IsValid()
        {
            int[] bucket = new int[26];
            foreach (char letter in Pass)
            {
                bucket[letter - 97]++;
            }

            int index = Letter - 97;
            if (bucket[index] < Low || bucket[index] > High)
            {
                return false;
            }
            return true;
        }

        public bool TobogganCorporateValid()
        {
            bool low = false;
            bool high = false;
            bool LowInRange = Low - 1 < Pass.Length;
            bool highInRange = High - 1 < Pass.Length;
            
            if (LowInRange)
            {
                low = (Pass[Low - 1] == Letter);
            }
            if (highInRange)
            {
                high = (Pass[High - 1] == Letter);
            }
            return low ^ high;
        }

        public override string ToString()
        {
            return $"{IsValid(),5}|{Low}-{High} {Letter}: {Pass}";
        }
    }
}
