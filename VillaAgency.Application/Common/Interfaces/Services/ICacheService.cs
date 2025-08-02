namespace VillaAgency.Application.Common.Interfaces.Services
{
    public interface ICacheService
    {
        /// <summary>
        /// دریافت داده از کش
        /// </summary>
        /// <typeparam name="T">نوع داده‌ای که انتظار داریم</typeparam>
        /// <param name="key">کلید منحصربه‌فرد داده</param>
        /// <returns>داده مورد نظر یا مقدار پیش‌فرض آن</returns>
        Task<T?> GetDataAsync<T>(string key);

        /// <summary>
        /// ذخیره داده در کش
        /// </summary>
        /// <typeparam name="T">نوع داده‌ای که ذخیره می‌شود</typeparam>
        /// <param name="key">کلید منحصربه‌فرد</param>
        /// <param name="value">مقداری که باید ذخیره شود</param>
        /// <param name="expirationTime">مدت زمان اعتبار کش</param>
        /// <returns>موفقیت‌آمیز بودن عملیات</returns>
        Task<bool> SetDataAsync<T>(string key, T value, TimeSpan? expirationTime);

        /// <summary>
        /// حذف داده از کش
        /// </summary>
        /// <param name="key">کلید داده‌ای که باید حذف شود</param>
        /// <returns>موفقیت‌آمیز بودن عملیات</returns>
        Task<bool> RemoveDataAsync(string key);
    }
}
