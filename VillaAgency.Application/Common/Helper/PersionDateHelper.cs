using System.Globalization;
using System.Text; // اضافه کردن برای استفاده از StringBuilder

namespace VillaAgency.Application.Common.Helper
{
    public static class PersionDateHelper
    {
        private static readonly PersianCalendar PersianCalendar = new();

        /// <summary>
        /// یک رشته تاریخ شمسی (مثلا "1403/05/07" یا "۱۴۰۳/۰۵/۰۷") را به تاریخ میلادی تبدیل می‌کند
        /// </summary>
        public static DateTime ToGregorian(string persianDate)
        {
            if (string.IsNullOrWhiteSpace(persianDate))
            {
                return default;
            }

            // مرحله ۱: اعداد فارسی/عربی را به انگلیسی تبدیل می‌کنیم
            var englishNumeralsDate = ToEnglishNumerals(persianDate);

            try
            {
                // مرحله ۲: بقیه عملیات روی رشته‌ی اصلاح‌شده انجام می‌شود
                var parts = englishNumeralsDate.Split('/');
                if (parts.Length != 3) return default;

                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);

                return PersianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            catch
            {
                // در صورت بروز خطا، یک تاریخ پیش‌فرض برمی‌گرداند
                return default;
            }
        }

        /// <summary>
        /// یک تاریخ میلادی را به رشته تاریخ شمسی (مثلا "1403/05/07") تبدیل می‌کند
        /// </summary>
        public static string ToPersian(DateTime gregorianDate)
        {
            if (gregorianDate == default)
            {
                return string.Empty;
            }

            int year = PersianCalendar.GetYear(gregorianDate);
            int month = PersianCalendar.GetMonth(gregorianDate);
            int day = PersianCalendar.GetDayOfMonth(gregorianDate);

            return $"{year:D4}/{month:D2}/{day:D2}";
        }

        // <<-- متد کمکی جدید برای تبدیل اعداد -->>
        /// <summary>
        /// یک رشته حاوی اعداد فارسی یا عربی را به اعداد انگلیسی تبدیل می‌کند
        /// </summary>
        private static string ToEnglishNumerals(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var sb = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                switch (c)
                {
                    case '۰': sb.Append('0'); break;
                    case '۱': sb.Append('1'); break;
                    case '۲': sb.Append('2'); break;
                    case '۳': sb.Append('3'); break;
                    case '۴': sb.Append('4'); break;
                    case '۵': sb.Append('5'); break;
                    case '۶': sb.Append('6'); break;
                    case '۷': sb.Append('7'); break;
                    case '۸': sb.Append('8'); break;
                    case '۹': sb.Append('9'); break;
                    // برای پشتیبانی از اعداد عربی
                    case '٠': sb.Append('0'); break;
                    case '١': sb.Append('1'); break;
                    case '٢': sb.Append('2'); break;
                    case '٣': sb.Append('3'); break;
                    case '٤': sb.Append('4'); break;
                    case '٥': sb.Append('5'); break;
                    case '٦': sb.Append('6'); break;
                    case '٧': sb.Append('7'); break;
                    case '٨': sb.Append('8'); break;
                    case '٩': sb.Append('9'); break;
                    default: sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
    }
}