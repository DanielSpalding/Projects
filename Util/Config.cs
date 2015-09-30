using System.Configuration;

namespace Util
{
    public class Config
    {
        /// <summary>
        /// Gets value in 'appsettings' tag in web.config.
        /// </summary>
        /// <param name="key">appstring parameter</param>
        /// <param name="defval">default value</param>
        /// <returns>Connection string</returns>
        public static string getValue(string key, string defval)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (value==null)
            {
                value = defval;
            }
            return value;
        }

    }
}
