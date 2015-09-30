namespace FRIAS.Common
{
    /// <summary>
    /// constants used throughout the application
    /// </summary>
    public class Constant
    {
        public static string plantDBStr;
        
        public const int type_FZ = 1;
        public const int type_FA = 2;
        public const int type_FR = 3;

        /// <summary>
        /// Site urls for specific pages
        /// </summary>
        public struct site
        {
            // component pages
            public const string compListPage = "~/pages/compListPage.aspx";
            public const string compPage = "~/pages/compPage.aspx";
            public const string cableBlockPage = "~/pages/cbdPage.aspx";

            // power pages
            public const string powerListPage = "~/pages/powerListPage.aspx";
            public const string powerPage = "~/pages/powerPage.aspx";
            
            // cable pages
            public const string cabListPage = "~/pages/cabListPage.aspx";
            public const string cabPage = "~/pages/cabPage.aspx";
            
            // reference pages
            public const string refPage = "~/pages/refPage.aspx";
            
            // routing pages
            public const string routeListPage = "~/pages/routeListPage.aspx";
            public const string routePage = "~/pages/routePage.aspx";
            
            // fire area pages
            public const string fareaListPage = "~/pages/faListPage.aspx";
            public const string fareaPage = "~/pages/faPage.aspx";
            
            // fire zone pages
            public const string fzoneListPage = "~/pages/fzListPage.aspx";
            public const string fzonePage = "~/pages/fzPage.aspx";

            // fire room pages
            public const string froomListPage = "~/pages/frListPage.aspx";
            public const string froomPage = "~/pages/frPage.aspx";

            // disposition pages
            public const string dispListPage = "~/pages/dispListPage.aspx";
            public const string dispPage = "~/pages/dispPage.aspx";
            
            // nsca pages
            public const string nscaListPage = "~/pages/nscaListPage.aspx";
            public const string nscaPage = "~/pages/nscaPage.aspx";
            
            // pra pages
            public const string praListPage = "~/pages/praListPage.aspx";
            public const string praPage = "~/pages/praPage.aspx";

            // npo pages
            public const string npoListPage = "~/pages/npoListPage.aspx";
            public const string npoPage = "~/pages/npoPage.aspx";

            // maf page
            public const string mafPage = "~/pages/mafPage.aspx";

            // b3Table page
            public const string b3TablePage = "~/pages/b3TablePage.aspx";

            // report pages
            public const string reportPage = "~/pages/reportPage.aspx";

            // other pages
            public const string loginPage = "~/loginPage.aspx";
            public const string mainPage = "~/pages/mainPage.aspx";
            
        }
        
        /// <summary>
        /// Session constants for various items
        /// </summary>
        public struct session
        {
            // session variables
            public const string User = "sessionUser";
            public const string Component = "sessionComp";
            public const string Power = "sessionPower";
            public const string Cable = "sessionCab";
            public const string Drawing = "sessionDwg";
            public const string Route = "sessionRoute";
            public const string FireArea = "sessionFA";
            public const string FACompDisp = "SessionFACompDisp";
            public const string FireZone = "sessionFZ";
            public const string FireRoom = "sessionFR";
            public const string Disposition = "sessionDisp";
            public const string ComboType = "sessionComboType";
            public const string Table = "sessionTable";
            public const string Reportlist = "sessionReportlist";
            public const string Userlist = "sessionUserlist";
            public const string Document = "sessionDoc";

            public const string WhatIfComponent = "sessionWhatIfComponent";
            public const string WhatIfCable = "sessionWhatIfCable";
            public const string WhatIfFireArea = "sessionWhatIfFA";
        }
        
        /// <summary>
        /// Session threads
        /// </summary>
        public struct thread
        {
            public const string Component = "threadComponent";
            public const string Power = "threadPower";
            public const string Cable = "threadCable";
            public const string Drawing = "threadDrawing";
            public const string Route = "threadRoute";
            public const string FireArea = "threadFA";
            public const string FireZone = "threadFZ";
            public const string FireRoom = "threadFR";
            public const string Disposition = "threadDisp";
            public const string NSCA = "threadNSCA";
            public const string PRA = "threadPRA";
            public const string NPO = "threadNPO";
            public const string MAF = "threadMAF";
            public const string Document = "threadDocument";
        }

        /// <summary>
        /// Common variable values used
        /// </summary>
        public struct var
        {
            public const string nullDate = "1/1/0001 12:00:00 AM";                          // null value for date/time
        }
    }
}
