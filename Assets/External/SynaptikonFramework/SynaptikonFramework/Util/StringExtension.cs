using System;
using System.Diagnostics;
using System.Text;

namespace SynaptikonFramework.Util
{
    public static class StringExtension
    {
        public static string EncryptZeroOne(this string bits)
        {
            // check if the string contains any other character
            foreach (char c in bits)
            {
                if (c < '0' || c > '1')
                    throw new FormatException("Bits are not in the correct format, only 0 and 1 are allowed");
            }

            var builder = new StringBuilder();
            for (var i = 0; i < bits.Length; i += 30)
            {
                string bitStream;
                if (bits.Length > i + 30)
                    bitStream = "1" + bits.Substring(i, 30);
                else
                    bitStream = "1" + bits.Substring(i);
                var step = ConvertToBase(Convert.ToInt32(bitStream, 2),36);
                while (step.Length < 6)
                    step = "0" + step;
                builder.Append(step);
            }
            return builder.ToString();
        }

        public static string DecryptZeroOne(this string code)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < code.Length; i += 6)
            {
                string bitstream;
                if (code.Length > i + 6)
                    bitstream = code.Substring(i, 6);
                else
                    bitstream = code.Substring(i);
                var step = ConvertToBase((int)Decode(bitstream),2);
                builder.Append(step.Substring(1));
            }
            return builder.ToString();
        }

        static string ConvertToBase(int num, int nbase)
        {
            string chars = "0123456789abcdefghijklmnopqrstuvwxyz";

            // check if we can convert to another base
            if (nbase < 2 || nbase > chars.Length)
                return "";

            int r;
            string newNumber = "";

            // in r we have the offset of the char that was converted to the new base
            while (num >= nbase)
            {
                r = num % nbase;
                newNumber = chars[r] + newNumber;
                num = num / nbase;
            }
            // the last number to convert
            newNumber = chars[num] + newNumber;

            return newNumber;
        }

        static Int64 Decode(string input)
        {
            string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            var reversed = input.Reverse();
            long result = 0;
            int pos = 0;
            foreach (char c in reversed)
            {
                result += chars.IndexOf(c) * (long)System.Math.Pow(36, pos);
                pos++;
            }
            return result;
        }

        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
