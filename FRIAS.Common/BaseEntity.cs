using FRIAS.Common.Entity;

namespace FRIAS.Common
{
    /// <summary>
    /// base entity used for all other entities
    /// </summary>
    public class BaseEntity
    {
        #region private variables
        private User _argUser;                                                              // argument user information
        #endregion

        #region public properties
        public User argUser
        {
            get
            {
                return _argUser;
            }
            set
            {
                _argUser = value;
            }
        }
        #endregion
    }
}
