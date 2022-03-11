using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EMF_Portal_API.Extentions
{
    public class PasswordGenerator
    {
        private int DEFAULT_MIN_PASSWORD_LENGTH = 8;

        private int DEFAULT_MAX_PASSWORD_LENGTH = 10;

        private string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";

        private string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";

        private string PASSWORD_CHARS_NUMERIC = "23456789";

        private string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

        public string Generate()
        {
            return Generate(DEFAULT_MIN_PASSWORD_LENGTH, DEFAULT_MAX_PASSWORD_LENGTH);
        }

        public string Generate(int length)
        {
            return Generate(length, length);
        }

        public string Generate(int minLength, int maxLength)
        {
            if (minLength > 0 && maxLength > 0 && minLength <= maxLength)
            {
                char[][] charGroups = new char[4][]
                {
                PASSWORD_CHARS_LCASE.ToCharArray(),
                PASSWORD_CHARS_UCASE.ToCharArray(),
                PASSWORD_CHARS_NUMERIC.ToCharArray(),
                PASSWORD_CHARS_SPECIAL.ToCharArray()
                };
                int[] charsLeftInGroup = new int[charGroups.Length];
                for (int k = 0; k < charsLeftInGroup.Length; k++)
                {
                    charsLeftInGroup[k] = charGroups[k].Length;
                }
                int[] leftGroupsOrder = new int[charGroups.Length];
                for (int j = 0; j < leftGroupsOrder.Length; j++)
                {
                    leftGroupsOrder[j] = j;
                }
                byte[] randomBytes = new byte[4];
                new RNGCryptoServiceProvider().GetBytes(randomBytes);
                Random random = new Random(BitConverter.ToInt32(randomBytes, 0));
                char[] password2 = null;
                password2 = ((minLength >= maxLength) ? new char[minLength] : new char[random.Next(minLength, maxLength + 1)]);
                int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                for (int i = 0; i < password2.Length; i++)
                {
                    int nextLeftGroupsOrderIdx = (lastLeftGroupsOrderIdx != 0) ? random.Next(0, lastLeftGroupsOrderIdx) : 0;
                    int nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
                    int lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;
                    int nextCharIdx = (lastCharIdx != 0) ? random.Next(0, lastCharIdx + 1) : 0;
                    password2[i] = charGroups[nextGroupIdx][nextCharIdx];
                    if (lastCharIdx == 0)
                    {
                        charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                    }
                    else
                    {
                        if (lastCharIdx != nextCharIdx)
                        {
                            char temp2 = charGroups[nextGroupIdx][lastCharIdx];
                            charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                            charGroups[nextGroupIdx][nextCharIdx] = temp2;
                        }
                        charsLeftInGroup[nextGroupIdx]--;
                    }
                    if (lastLeftGroupsOrderIdx == 0)
                    {
                        lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                    }
                    else
                    {
                        if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                        {
                            int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                            leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                            leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                        }
                        lastLeftGroupsOrderIdx--;
                    }
                }
                return new string(password2);
            }
            return null;
        }
    }
}
