using System;
using System.Text.RegularExpressions;
using Archer.Extension.Models;

namespace Archer.Extension
{
    /// <summary>
    /// 身分識別工具類，用於判斷自然人與法人
    /// </summary>
    public static class IdentityHelper
    {
        /// <summary>
        /// 判斷給定的識別號碼是屬於自然人還是法人
        /// </summary>
        /// <param name="identityNumber">識別號碼（身分證號或統一編號）</param>
        /// <returns>實體類型</returns>
        public static EntityType DetermineEntityType(string identityNumber)
        {
            if (string.IsNullOrWhiteSpace(identityNumber))
            {
                return EntityType.Unknown;
            }

            identityNumber = identityNumber.Trim().ToUpper();

            // 檢查是否為身分證號格式（自然人）
            if (IsValidNationalId(identityNumber))
            {
                return EntityType.NaturalPerson;
            }

            // 檢查是否為統一編號格式（法人）
            if (IsValidBusinessRegistrationNumber(identityNumber))
            {
                return EntityType.LegalEntity;
            }

            return EntityType.Unknown;
        }

        /// <summary>
        /// 驗證台灣身分證號格式
        /// </summary>
        /// <param name="nationalId">身分證號</param>
        /// <returns>是否為有效的身分證號格式</returns>
        public static bool IsValidNationalId(string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
            {
                return false;
            }

            nationalId = nationalId.Trim().ToUpper();

            // 身分證號格式：1個英文字母 + 9個數字
            var regex = new Regex(@"^[A-Z][0-9]{9}$");
            if (!regex.IsMatch(nationalId))
            {
                return false;
            }

            // 身分證號檢查碼驗證
            return ValidateNationalIdChecksum(nationalId);
        }

        /// <summary>
        /// 驗證台灣統一編號格式
        /// </summary>
        /// <param name="businessNumber">統一編號</param>
        /// <returns>是否為有效的統一編號格式</returns>
        public static bool IsValidBusinessRegistrationNumber(string businessNumber)
        {
            if (string.IsNullOrWhiteSpace(businessNumber))
            {
                return false;
            }

            businessNumber = businessNumber.Trim();

            // 統一編號格式：8個數字
            var regex = new Regex(@"^[0-9]{8}$");
            if (!regex.IsMatch(businessNumber))
            {
                return false;
            }

            // 統一編號檢查碼驗證
            return ValidateBusinessNumberChecksum(businessNumber);
        }

        /// <summary>
        /// 驗證身分證號檢查碼
        /// </summary>
        /// <param name="nationalId">身分證號</param>
        /// <returns>檢查碼是否正確</returns>
        private static bool ValidateNationalIdChecksum(string nationalId)
        {
            // 身分證字母對應數值表
            var letterValues = new int[]
            {
                10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21, 22, 35, 23, 24, 25, 26, 27, 28, 29, 32, 30, 31, 33
            };

            char firstChar = nationalId[0];
            int letterValue = letterValues[firstChar - 'A'];

            // 計算檢查碼
            int sum = (letterValue / 10) + (letterValue % 10) * 9;

            for (int i = 1; i < 9; i++)
            {
                sum += (nationalId[i] - '0') * (9 - i);
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (nationalId[9] - '0');
        }

        /// <summary>
        /// 驗證統一編號檢查碼
        /// </summary>
        /// <param name="businessNumber">統一編號</param>
        /// <returns>檢查碼是否正確</returns>
        private static bool ValidateBusinessNumberChecksum(string businessNumber)
        {
            var weights = new int[] { 1, 2, 1, 2, 1, 2, 4, 1 };
            int sum = 0;

            for (int i = 0; i < 8; i++)
            {
                int digit = businessNumber[i] - '0';
                int product = digit * weights[i];
                sum += (product / 10) + (product % 10);
            }

            // 特殊情況處理：第7位數為7時的例外規則
            if ((businessNumber[6] - '0') == 7)
            {
                return (sum % 10) == 0 || ((sum + 1) % 10) == 0;
            }

            return (sum % 10) == 0;
        }
    }
}