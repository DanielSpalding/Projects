using System.Data;
using System.Collections;

namespace FRIAS.BL
{
    public interface IFactoryManager
    {
        #region public properties
        
        // Old Object is used track hisotry
        object OldObject
        {
            get;
            set;
        }

        // fieldlist is used to determine if a property (field) of an object is used
        // saves a list of control names when binding control to object (see UIManager.BindControlToObject)
        string[] FieldList
        {
            get;
            set;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Fetches an object
        /// </summary>
        /// <param name="obj">Object passed</param>
        /// <returns>Object returned</returns>
        object Fetch(object obj);

        /// <summary>
        /// Fetches a dataset
        /// </summary>
        /// <param name="criteria">criteria to search for</param>
        /// <returns>Dataset</returns>
        DataSet FetchDataSet(string criteria);
        
        // fetches a combolist (array)
        ArrayList FetchComboList(string initMsg, string param);
        
        // deletes a given object
        void Delete(object obj);
        
        // updates a given object
        void Update(object obj);
        
        #endregion
    }
}
