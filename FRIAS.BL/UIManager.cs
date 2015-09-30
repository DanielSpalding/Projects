using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL
{
    /// <summary>
    /// Common UI Methods
    /// </summary>
    public sealed class UIManager : TotalManager
    {
        private UIManager() { }

        #region common methods (bind, toggle, (un)lock)

        /// <summary>
        /// Creates a DataTable, casting null values and empty datetime values.
        /// </summary>
        /// <param name="itemlist">ArrayList</param>
        /// <returns></returns>
        public static DataTable CreateDataTable(ArrayList itemlist)
        {
            // create new data table and row
            DataTable dt = new DataTable();
            DataRow dr;
            
            // check to see if list is empty
            if (itemlist.Count > 0)
            {
                // get type of object passed and its properties
                Type type = itemlist[0].GetType();
                PropertyInfo[] properties = type.GetProperties();
                
                // create new column for each property
                foreach (PropertyInfo p in properties)
                {
                    string[] col = p.ToString().Split(new char[] { ' ' });
                    dt.Columns.Add(col[1]);
                }

                // create new row for each each record
                for(int i=0; i<itemlist.Count; i++)
                {
                    dr = dt.NewRow();
                    foreach (PropertyInfo p in properties)
                    {
                        // populate values
                        string[] col = p.ToString().Split(new char[] { ' ' });
                        // if item is a date
                        if (p.Name.Contains("date"))
                        {
                            // if date is null (ie 1/1/0001 12:00:00 AM)
                            if (Convert.ToString(p.GetValue(itemlist[i], null)) == Constant.var.nullDate)
                                // set date to blank (can also be N/A
                                dr[col[1]] = "";
                            else
                                dr[col[1]] = Convert.ToDateTime(p.GetValue(itemlist[i], null)).Date.ToShortDateString();
                        }
                        // otherwise get value
                        else
                            dr[col[1]] = p.GetValue(itemlist[i], null);
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        
        /// <summary>
        /// Binds object to web controls within specified container.
        /// </summary>
        /// <param name="obj">Object entity</param>
        /// <param name="container">Web container (eg. panel)</param>
        public static void BindObjectToControl(object obj, Control container)
        {
            // if no object is sent then stop
            if (obj == null) return;
            
            // get type of object passed and its properties
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();
            
            foreach (PropertyInfo p in properties)
            {
                // find the control in the web form
                Control control = container.FindControl(p.Name);

                // check to see if value is null
                if (p.GetValue(obj, null) != null)
                {
                    // determine type of control and bind object to control
                    if (control is DropDownList)
                    {
                        if (p.PropertyType.Name.Contains("String"))
                            // item is a text only
                            ((DropDownList)control).Text = p.GetValue(obj, null).ToString();
                        else
                            // item has value and text
                            ((DropDownList)control).SelectedValue = p.GetValue(obj, null).ToString();
                    }
                    else if (control is CheckBox)
                    {
                        if (p.PropertyType == typeof(bool))
                            ((CheckBox)control).Checked = (bool)p.GetValue(obj, null);
                    }
                    else if (control is TextBox)
                    {
                        string value = p.GetValue(obj, null).ToString();
                        // check to see if textbox contains date
                        if (p.Name.Contains("date"))
                        {
                            if (value == Constant.var.nullDate)
                                // set to empty string if date is null
                                ((TextBox)control).Text = "";
                            else
                                // get the date portion from date/time
                                ((TextBox)control).Text = value.Substring(0, value.IndexOf(" "));
                        }
                        else
                            ((TextBox)control).Text = p.GetValue(obj, null).ToString();
                    }
                    else if (control is Label)
                    {
                        ((Label)control).Text = p.GetValue(obj, null).ToString();
                    }
                    else if (control is HiddenField)
                        ((HiddenField)control).Value = p.GetValue(obj, null).ToString();
                    else if (control is LinkButton)
                        ((LinkButton)control).Text = p.GetValue(obj, null).ToString();
                    else
                    {
                        // any other controls
                    }
                }
            }
        }

        /// <summary>
        /// Binds object to a specific web control within specified container.
        /// </summary>
        /// <param name="obj">Object entity</param>
        /// <param name="container">Web container (eg. panel)</param>
        /// <param name="prefix">Prefix of the web control inside the container</param>
        public static void BindControlToObject(object obj, Control container, string prefix)
        {
            // get type of object passed and its properties
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();
            
            Control control;

            foreach (PropertyInfo p in properties)
            {
                if (prefix == null)
                    // if no prefix was passsed then control name is property name
                    control = container.FindControl(p.Name);
                else
                    // otherwise control name is prefix + property name
                    control = container.FindControl(prefix + p.Name);

                // determine type of control and bind control to object
                if (control is DropDownList)
                {
                    DropDownList list = (DropDownList)control;
                    if (list.SelectedItem != null)
                        p.SetValue(obj, Convert.ChangeType(list.SelectedItem.Value, p.PropertyType), null);
                }
                else if (control is CheckBox)
                {
                    p.SetValue(obj, (bool)((CheckBox)control).Checked, null);
                }
                else if (control is TextBox)
                {
                    if ((((TextBox)control).Text).Length > 0)
                        p.SetValue(obj, Convert.ChangeType(((TextBox)control).Text, p.PropertyType), null);
                    else
                    {
                        if (p.Name.Contains("date"))
                            p.SetValue(obj, null, null);
                        else
                            p.SetValue(obj, "", null);
                    }
                }
                else if (control is Label)
                {
                    if ((((Label)control).Text).Length > 0)
                        p.SetValue(obj, Convert.ChangeType(((Label)control).Text, p.PropertyType), null);
                    else
                        p.SetValue(obj, "", null);
                }
                else if (control is HiddenField)
                {
                    if (((HiddenField)control).Value == "")
                        p.SetValue(obj, 0, null);
                    else
                        p.SetValue(obj, Convert.ToInt32(((HiddenField)control).Value), null);
                }
                else if (control is LinkButton)
                {
                    if ((((LinkButton)control).Text).Length > 0)
                        p.SetValue(obj, Convert.ChangeType(((LinkButton)control).Text, p.PropertyType), null);
                    else
                        p.SetValue(obj, "", null);
                }
                else
                {
                    // any other controls
                }
            }
        }

        /// <summary>
        /// Binds value of web controls to object with given prefix for control name AND tracks fields populated
        /// </summary>
        public static void BindControlToObject(object obj, Control container, string prefix, ref string[] fields)
        {
            Type type = obj.GetType();                                                      // get type of object
            Control control;
            int i = 0;                                                                      // initialize counter to count fields
            PropertyInfo[] properties = type.GetProperties();                               // get properties
            
            foreach (PropertyInfo p in properties)
            {
                if (prefix == null)
                    // if no prefix was passed then control name is property name
                    control = container.FindControl(p.Name);
                else
                    // otherwise control name is prefix + property name
                    control = container.FindControl(prefix + p.Name);

                // determine type of control and bind control to object
                if (control is DropDownList)
                {
                    DropDownList list = (DropDownList)control;
                    if (list.SelectedItem != null)
                        p.SetValue(obj, Convert.ChangeType(list.SelectedItem.Value, p.PropertyType), null);
                    fields[i] = p.Name;
                    i++;
                }
                else if (control is CheckBox)
                {
                    p.SetValue(obj, (bool)((CheckBox)control).Checked, null);
                    fields[i] = p.Name;
                    i++;
                }
                else if (control is TextBox)
                {
                    if ((((TextBox)control).Text).Length > 0)
                        p.SetValue(obj, Convert.ChangeType(((TextBox)control).Text, p.PropertyType), null);
                    else
                    {
                        if (p.Name.Contains("date"))
                            p.SetValue(obj, null, null);
                        else
                            p.SetValue(obj, "", null);
                    }
                    fields[i] = p.Name;
                    i++;
                }
                else if (control is Label)
                {
                    if ((((Label)control).Text).Length > 0)
                        p.SetValue(obj, Convert.ChangeType(((Label)control).Text, p.PropertyType), null);
                    else
                        p.SetValue(obj, "", null);
                    fields[i] = p.Name;
                    i++;
                }
                else if (control is HiddenField)
                {
                    if (((HiddenField)control).Value == "")
                        p.SetValue(obj, 0, null);
                    else
                        p.SetValue(obj, Convert.ToInt32(((HiddenField)control).Value), null);
                    fields[i] = p.Name;
                    i++;
                }
                else if (control is LinkButton)
                {
                    if ((((LinkButton)control).Text).Length > 0)
                        p.SetValue(obj, Convert.ChangeType(((LinkButton)control).Text, p.PropertyType), null);
                    else
                        p.SetValue(obj, "", null);
                }
                else
                {
                    // any other controls
                }
            }
        }

        /// <summary>
        /// Toggles css styles and readonly attributes for given repeater
        /// </summary>
        /// <param name="list">Repeater</param>
        /// <param name="e">RepeaterCommandEventArgs</param>
        public static void ToggleRepeaterControl(Repeater list, RepeaterCommandEventArgs e)
        {
            string cname = e.CommandName.ToString();                                        // command name
            string listid = list.ID;                                                        // id of the repeater
            string controltype;                                                             // control type
            string controlname;                                                             // name of the control
            int i = e.Item.ItemIndex;                                                       // row index
            ImageButton btn = new ImageButton();

            if (cname == "edit")
            {
                // lock all rows except selected index
                for (int j = 0; j < list.Items.Count; j++)
                {
                    // hide edit button
                    btn = (ImageButton)list.Items[j].FindControl("btnE" + listid);
                    btn.Visible = false;

                    // hide delete button
                    btn = (ImageButton)list.Items[j].FindControl("btnD" + listid);
                    btn.Visible = false;
                }

                // toggle TextBox and CheckBox
                for (int j = 0; j < list.Items[i].Controls.Count; j++)
                {
                    // get control type and name
                    controltype = list.Items[i].Controls[j].GetType().ToString();
                    controlname = list.Items[i].Controls[j].ClientID.ToString();

                    // check for textboxes that are not tagged as "readonly"
                    if (controltype.Contains("TextBox") && !controlname.Contains("readonly"))
                    {
                        ((TextBox)list.Items[i].Controls[j]).CssClass = "editable";
                        ((TextBox)list.Items[i].Controls[j]).ReadOnly = false;
                        ((TextBox)list.Items[i].Controls[j]).Focus();
                    }
                    // check for checkboxes that are not tagged as "readonly"
                    else if (controltype.Contains("CheckBox") && !controlname.Contains("readonly"))
                        ((CheckBox)list.Items[i].Controls[j]).Enabled = true;
                    // check for dropdownlists (dropdownlists cannot be tagged as "readonly"
                    else if (controltype.Contains("DropDownList"))
                    {
                        ((DropDownList)list.Items[i].Controls[j]).CssClass = "editable";
                        ((DropDownList)list.Items[i].Controls[j]).Enabled = true;
                    }
                }

                // change edit --> update button
                btn = (ImageButton)list.Items[i].FindControl("btnE" + listid);
                btn.ImageUrl = "~/images/update.png";
                btn.Visible = true;
                btn.CommandName = "update";

                // change delete --> cancel button
                btn = (ImageButton)list.Items[i].FindControl("btnD" + listid);
                btn.ImageUrl = "~/images/cancel.png";
                // show button, as button is hidden in reportListPage
                btn.Visible = true;
                btn.CommandName = "cancel";
            
            }
            else if ((cname == "cancel") || (cname == "update"))
            {
                // toggle TextBox and CheckBox
                for (int j = 0; j < list.Items[i].Controls.Count; j++)
                {
                    controltype = list.Items[i].Controls[j].GetType().ToString();
                    if (controltype.Contains("TextBox"))
                    {
                        ((TextBox)list.Items[i].Controls[j]).CssClass = "readonly";
                        ((TextBox)list.Items[i].Controls[j]).ReadOnly = true;
                    }
                    else if (controltype.Contains("CheckBox"))
                        ((CheckBox)list.Items[i].Controls[j]).Enabled = false;
                    //else if (controltype.Contains("DropDownList"))
                    //{
                    //    ((DropDownList)list.Items[i].Controls[j]).CssClass = "readonly";
                    //    ((DropDownList)list.Items[i].Controls[j]).Enabled = false;
                    //}
                }
            }
            else if (cname == "delete")
            {
                // lock all rows except selected index
                for (int j = 0; j < list.Items.Count; j++)
                {
                    // hide edit button
                    btn = (ImageButton)list.Items[j].FindControl("btnE" + listid);
                    btn.Visible = false;

                    // hide delete button
                    btn = (ImageButton)list.Items[j].FindControl("btnD" + listid);
                    btn.Visible = false;
                }

                // change edit --> delete button
                btn = (ImageButton)list.Items[i].FindControl("btnE" + listid);
                btn.ImageUrl = "~/images/cross.png";
                btn.Visible = true;
                btn.CommandName = "deletenow";
                btn.Focus();

                // change delete --> cancel button
                btn = (ImageButton)list.Items[i].FindControl("btnD" + listid);
                btn.ImageUrl = "~/images/cancel.png";
                // show button, as button is hidden in reportListPage
                btn.Visible = true;
                btn.CommandName = "cancel";
            }
            else if (cname == "donothing")
            {
                // does nothing
            }
        }

        /// <summary>
        /// Sets focus to edit button of the selected repeater row
        /// </summary>
        public static void SetRowFocus(Repeater list, RepeaterCommandEventArgs e)
        {
            // index of the repeater row
            int i = e.Item.ItemIndex;

            if (i < list.Items.Count)
                ((ImageButton)list.Items[i].FindControl("btnE" + list.ID)).Focus();
        }

        // locks sign-off controls based on prep_by, chkd_by and user intitials
        public static void LockControls(Control control, string prep_by, string chkd_by, User user)
        {
            if (user.level != User.admin)
            {
                // if there is no prep_by user cannot enter chkd_by or chkd_date
                // NOTE: if user enter prep_date w/ prep_by=N/A error message is given in the page
                if (prep_by == "N/A")
                {
                    ((DropDownList)control.FindControl("chkd_by")).Enabled = false;
                    ((TextBox)control.FindControl("chkd_date")).Enabled = false;
                }

                // if user is preparer, he cannot also be the checker
                if (user.initial == prep_by)
                {
                    ((DropDownList)control.FindControl("chkd_by")).Enabled = false;
                    ((TextBox)control.FindControl("chkd_date")).Enabled = false;
                }
                else
                {
                    // otherwise if user is not preparer, and therefore should not be able to change prep_by or prep_date
                    if (prep_by != "N/A")
                    {
                        ((DropDownList)control.FindControl("prep_by")).Enabled = false;
                        ((TextBox)control.FindControl("prep_date")).Enabled = false;
                    }
                }

                // if user is not checker and there is already a checker, then he cannot change chkd_date
                if ((user.initial != chkd_by) && (chkd_by != "N/A"))
                    ((TextBox)control.FindControl("chkd_date")).Enabled = false;
            }
        }

        // locks a given colleciton of controls
        public static void LockControls(ControlCollection controls)
        {
            // NOTE: must use GetType and .Contains because if we use "c is Control" directly Repeater Controls are hidden inside RepeaterItems
            string controltype;

            // loop through controls
            foreach (Control c in controls)
            {
                controltype = c.GetType().ToString();

                // if control is a button (and it is not a button that is used to SHOW other item then hide button
                if ((controltype.Contains("Button")) && !(c.ClientID.ToString().Contains("Show")))
                {
                    if (controltype.Contains("ImageButton"))
                        ((ImageButton)c).Visible = false;
                    else if (!controltype.Contains("LinkButton"))
                        ((Button)c).Visible = false;
                }
                // if control is a checkbox and id contains "new" then disable
                else if (controltype.Contains("CheckBox") && c.ClientID.ToString().Contains("new"))
                {
                    ((CheckBox)c).Visible = false;
                }
                // if control is a textbox and id contains "new" then make readonly
                else if (controltype.Contains("TextBox") && c.ClientID.ToString().Contains("new"))
                {
                    ((TextBox)c).CssClass = "readonly";
                    ((TextBox)c).ReadOnly = true;
                }
                // if control is a dropdownlist then hide
                else if (controltype.Contains("DropDownList"))
                {
                    ((DropDownList)c).BackColor = System.Drawing.ColorTranslator.FromHtml("#ededed");
                    ((DropDownList)c).Enabled = false;
                }
                // if it is repeater then call LockConrols recursively
                else if (controltype.Contains("Repeater"))
                    LockControls(c.Controls);
                else if (controltype.Contains("Accordion"))
                    LockControls(c.Controls);
            }
        }

        // unlocks given collection of controls
        public static void UnLockControls(ControlCollection controls)
        {
            string controltype;

            // loop through controls
            foreach (Control c in controls)
            {
                controltype = c.GetType().ToString();

                // if control is a button (and it is not a button that is used to SHOW other item then unhide button
                if ((controltype.Contains("Button")) && !(c.ClientID.ToString().Contains("Show")))
                {
                    if (controltype.Contains("ImageButton"))
                        ((ImageButton)c).Visible = true;
                    else if (!controltype.Contains("LinkButton"))
                        ((Button)c).Visible = true;
                }
                // if control is a checkbox and id contains "new" then enable
                else if (controltype.Contains("CheckBox") && c.ClientID.ToString().Contains("new"))
                {
                    ((CheckBox)c).Visible = true;
                }
                // if control is a textbox and id contains "new" then make editable
                else if (controltype.Contains("TextBox") && c.ClientID.ToString().Contains("new"))
                {
                    ((TextBox)c).CssClass = "editable";
                    ((TextBox)c).ReadOnly = false;
                }
                // if control is a dropdownlist then unhide
                else if (controltype.Contains("DropDownList"))
                    ((DropDownList)c).Visible = true;
                // if it is repeater then call LockConrols recursively
                else if (controltype.Contains("Repeater"))
                    UnLockControls(c.Controls);
            }
        }

        public static void LockPrimaryObjectControls(ControlCollection controls)
        {
            // NOTE: cannot be used to lock controls for all updatepanels as it does not call itself recursively to handle repeaters
            string controltype;

            foreach (Control c in controls)
            {
                controltype = c.GetType().ToString();

                // NOTE: buttons should not be hidden or disabled as they trigger lock/unlock events themselves

                // if control is a checkbox then enable
                if (controltype.Contains("CheckBox"))
                {
                    ((CheckBox)c).Enabled = false;
                }
                // if control is a textbox then make editable
                else if (controltype.Contains("TextBox"))
                {
                    ((TextBox)c).CssClass = "readonly";
                    ((TextBox)c).ReadOnly = true;
                }
                // if control is a dropdownlist then enable and change color
                else if (controltype.Contains("DropDownList"))
                {
                    ((DropDownList)c).CssClass = "readonly";
                    ((DropDownList)c).BackColor = System.Drawing.ColorTranslator.FromHtml("#ededed");
                    ((DropDownList)c).Enabled = false;
                }
                // if calendar extender then enable it
                else if (controltype.Contains("CalendarExtender"))
                    ((AjaxControlToolkit.CalendarExtender)c).Enabled = true;
                else if (controltype.Contains("Panel"))
                    LockPrimaryObjectControls(c.Controls);

            }
        }

        // unlocks a given collection of controls for an object (Component, Cable etc).
        public static void UnlockPrimaryObjectControls(ControlCollection controls)
        {
            // NOTE: cannot be used to unlock controls for all updatepanels as it does not call itself recursively to handle repeaters
            string controltype;
            
            foreach (Control c in controls)
            {
                controltype = c.GetType().ToString();

                // NOTE: buttons should not be hidden or disabled as they trigger lock/unlock events themselves

                // if control is a checkbox then enable
                if (controltype.Contains("CheckBox"))
                {
                    ((CheckBox)c).Enabled = true;
                }
                // if control is a textbox then make editable
                else if (controltype.Contains("TextBox"))
                {
                    ((TextBox)c).CssClass = "editable";
                    ((TextBox)c).ReadOnly = false;
                }
                // if control is a dropdownlist then enable and change color
                else if (controltype.Contains("DropDownList"))
                {
                    ((DropDownList)c).BackColor = Color.White;
                    ((DropDownList)c).Enabled = true;
                }
                // if calendar extender then enable it
                else if (controltype.Contains("CalendarExtender"))
                    ((AjaxControlToolkit.CalendarExtender)c).Enabled = true;
                else if (controltype.Contains("Panel"))
                    UnlockPrimaryObjectControls(c.Controls);
                 
            }
        }

        #endregion

        #region sign-off error check

        // checks for errors in sign-offs
        public static string SignOffError(string prep_by, string prep_date, string chkd_by, string chkd_date)
        {
            string error = "";

            // preparer and checker cannot be the same
            if ((prep_by != "N/A") && (chkd_by != "N/A"))
                if (prep_by == chkd_by)
                    error = "Prep_By and Chkd_By cannot be the same. " + error;

            // check if date entered without initials
            if ((prep_by == "N/A") && (prep_date != ""))
                error = "Prep_Date entered w/o Prep_By. " + error;
            if ((chkd_by == "N/A") && (chkd_date != ""))
                error = "Chkd_Date entered w/o Chkd_By. " + error;

            // prep_date needs to be netered befroe chkd_date
            if ((prep_date == "") && (chkd_date != ""))
                error = "Chkd_Date cannot be entered without a Prep_Date. " + error;

            // chkd_date cannot be earlier than prep_date
            if ((prep_date != "") && (chkd_date != ""))
            {
                if (Convert.ToDateTime(chkd_date) < Convert.ToDateTime(prep_date))
                    error = "Chkd_Date must be after Prep_Date." + error;
            }

            return error;
        }

        #endregion

        #region autocomplete methods

        // types of autocompletes
        public enum AutoCompleteType
        {
            Component, Drawing, Subcomp, Power, Interlock, Cable, Route, FRoom, FZone, FArea, Disposition, 
            NSCA, PRA, NPO, BDEndpt, BE, KSF, Mode, ALLDocument, EEDocument, LICDocument, GENDocument
        }

        // fetches autocomplete list
        public static string[] FetchAutoCompleteList(string prefix, string ContextKey, AutoCompleteType type)
        {
            // NOTE: autocomplete lists are utilized as web service instead of dropdownlist to minimize view state
            
            string qryString = "";
            List<string> items = new List<string>();
            
            switch (type)
            {
                case AutoCompleteType.Component:
                    qryString = "SELECT COMP_ID, COMP FROM COMPLIST WHERE COMP LIKE '" + prefix + "%'  AND PRI_SUB <> 'D' ORDER BY COMP";
                    break;
                case AutoCompleteType.Drawing:
                    qryString = "SELECT DWG_ID, DWG_REF FROM DWGLIST WHERE DWG_REF LIKE '" + prefix + "%' ORDER BY DWG_REF, DWG_REV DESC";
                    break;
                case AutoCompleteType.Power:
                    qryString = "SELECT POWER_ID, POWER FROM viewPOWERLIST WHERE POWER LIKE '" + prefix + "%' ORDER BY POWER";
                    break;
                case AutoCompleteType.Subcomp:
                    // qryString = "SELECT COMP_ID, COMP FROM COMPLIST WHERE COMP LIKE '" + prefix + "%' AND PRI_SUB='S' ORDER BY COMP";
                    qryString = "SELECT COMP_ID, COMP FROM COMPLIST WHERE COMP LIKE '" + prefix + "%'  AND PRI_SUB <> 'D' ORDER BY COMP";
                    break;
                case AutoCompleteType.Interlock:
                    qryString = "SELECT COMP_ID AS INTLK_ID, COMP AS INTLK FROM COMPLIST WHERE COMP LIKE '" + prefix + "%' ORDER BY COMP";
                    break;
                case AutoCompleteType.Cable:
                    qryString = "SELECT CABLE_ID, CABLE FROM CABLIST WHERE CABLE LIKE '" + prefix + "%' ORDER BY CABLE";
                    break;
                case AutoCompleteType.Route:
                    qryString = "SELECT NODE_ID, NODE FROM ROUTELIST WHERE NODE LIKE '" + prefix + "%' ORDER BY NODE";
                    break;
                case AutoCompleteType.FArea:
                    qryString = "SELECT FA_ID, FA FROM FALIST WHERE FA LIKE '" + prefix + "%' ORDER BY FA";
                    break;
                case AutoCompleteType.FZone:
                    qryString = "SELECT FZ_ID, FZ FROM FZLIST WHERE FZ LIKE '" + prefix + "%' ORDER BY FZ";
                    break;
                case AutoCompleteType.FRoom:
                    qryString = "SELECT RM_ID, RM FROM FRLIST WHERE RM LIKE '" + prefix + "%' ORDER BY RM";
                    break;
                case AutoCompleteType.Disposition:
                    qryString = "SELECT DISP_ID, DISP FROM DISPLIST WHERE DISP LIKE '" + prefix + "%' ORDER BY DISP";
                    break;
                case AutoCompleteType.NSCA:
                    qryString = "SELECT COMP_ID, COMP FROM COMPLIST WHERE COMP LIKE '" + prefix + "%' AND PRI_SUB <> 'D' AND ((SSD_REQ=1 AND PRI_SUB='P') OR (NSCA_REQ=1)) ORDER BY COMP";
                    break;
                case AutoCompleteType.PRA:
                    qryString = "SELECT COMP_ID, COMP FROM COMPLIST WHERE COMP LIKE '" + prefix + "%' AND PRI_SUB <> 'D' AND PRA_REQ=1 ORDER BY COMP";
                    break;
                case AutoCompleteType.NPO:
                    qryString = "SELECT COMP_ID, COMP FROM COMPLIST WHERE COMP LIKE '" + prefix + "%' AND PRI_SUB <> 'D' AND NPO_REQ=1 ORDER BY COMP";
                    break;
                case AutoCompleteType.BDEndpt:
                    qryString = "SELECT DISTINCT 0 AS BD_ENDPT_ID, BD_ENDPT FROM viewBDENDPOINTS WHERE BD_ENDPT LIKE '" + prefix + "%' ORDER BY BD_ENDPT";
                    break;
                case AutoCompleteType.BE:
                    qryString = "SELECT DISTINCT BE_ID, BE FROM BELIST WHERE BE LIKE '" + prefix + "%' ORDER BY BE";
                    break;
                case AutoCompleteType.KSF:
                    qryString = "SELECT DISTINCT KSF_ID, KSF FROM KSFLIST WHERE KSF LIKE '" + prefix + "%' ORDER BY KSF";
                    break;
                case AutoCompleteType.Mode:
                    qryString = "SELECT DISTINCT MODE_ID, MODE FROM MODELIST WHERE MODE LIKE '" + prefix + "%' ORDER BY MODE";
                    break;
                case AutoCompleteType.ALLDocument:
                    qryString = "SELECT DISTINCT DOC_ID, DOC FROM DOCLIST WHERE DOC LIKE '" + prefix + "%' ORDER BY DOC";
                    break;
                case AutoCompleteType.EEDocument:
                    qryString = "SELECT DISTINCT DOC_ID, DOC FROM DOCLIST WHERE DOC LIKE '" + prefix + "%' AND DOC_TYPE='EE' ORDER BY DOC";
                    break;
                case AutoCompleteType.LICDocument:
                    qryString = "SELECT DISTINCT DOC_ID, DOC FROM DOCLIST WHERE DOC LIKE '" + prefix + "%' AND DOC_TYPE='LIC' ORDER BY DOC";
                    break;
                case AutoCompleteType.GENDocument:
                    qryString = "SELECT DISTINCT DOC_ID, DOC FROM DOCLIST WHERE DOC LIKE '" + prefix + "%' AND DOC_TYPE='GEN' ORDER BY DOC";
                    break;
            }

            String dbString = Util.Crypto.Decrypt(Util.Crypto.CryptoType.TripleDES, Util.Config.getValue("dbKey", "FRIAS"), ContextKey);
            IDBManager dbmgr = new DBManager(dbString);
            dbmgr.ConnectionString = dbString;
            
            try
            {
                dbmgr.Open();                                                               // open database
                dbmgr.ExecuteReader(CommandType.Text, qryString);                           // execute query
                while (dbmgr.DataReader.Read())                                             // get text and id for each item
                {
                    items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dbmgr.DataReader.GetString(1), dbmgr.DataReader.GetInt32(0).ToString()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }

            return items.ToArray();                                                         //return the string array
        }

        // determine id/index of selected value from autocomplete extender (returns 0 if not found)
        public static int SelectedItemId(AutoCompleteType item_type, string selected_value)
        {
            // initialize
            int index = 0;
            string qryString="";

            // determine type of item
            switch (item_type)
            {
                case AutoCompleteType.Drawing:
                    qryString = "SELECT DWG_ID AS id FROM DWGLIST WHERE DWG_REF='" + selected_value + "'";
                    break;
                case AutoCompleteType.Cable:
                    qryString = "SELECT CABLE_ID AS id FROM CABLIST WHERE CABLE='" + selected_value + "'";
                    break;
                case AutoCompleteType.Component:
                case AutoCompleteType.Interlock:
                case AutoCompleteType.Power:
                    qryString = "SELECT COMP_ID AS id FROM COMPLIST WHERE COMP='" + selected_value + "'";
                    break;
                case AutoCompleteType.Route:
                    qryString = "SELECT NODE_ID AS id FROM ROUTELIST WHERE NODE='" + selected_value + "'";
                    break;
                case AutoCompleteType.FArea:
                    qryString = "SELECT FA_ID AS id FROM FALIST WHERE FA='" + selected_value + "'";
                    break;
                case AutoCompleteType.FZone:
                    qryString = "SELECT FZ_ID AS id FROM FZLIST WHERE FZ='" + selected_value + "'";
                    break;
                case AutoCompleteType.FRoom:
                    qryString = "SELECT RM_ID AS id FROM FRLIST WHERE RM='" + selected_value + "'";
                    break;
                case AutoCompleteType.Disposition:
                    qryString = "SELECT DISP_ID AS id FROM DISPLIST WHERE DISP='" + selected_value + "'";
                    break;
                case AutoCompleteType.BE:
                    qryString = "SELECT BE_ID AS id FROM BELIST WHERE BE='" + selected_value + "'";
                    break;
                case AutoCompleteType.KSF:
                    qryString = "SELECT KSF_ID AS id FROM KSFLIST WHERE KSF='" + selected_value + "'";
                    break;
                case AutoCompleteType.Mode:
                    qryString = "SELECT MODE_ID AS id FROM MODELIST WHERE MODE='" + selected_value + "'";
                    break;
                case AutoCompleteType.NSCA:
                    qryString = "SELECT COMP_ID AS id FROM COMPLIST WHERE COMP='" + selected_value + "' AND ((SSD_REQ=1 AND PRI_SUB='P') OR (NSCA_REQ=1))";
                    break;
                case AutoCompleteType.NPO:
                    qryString = "SELECT COMP_ID AS id FROM COMPLIST WHERE COMP='" + selected_value + "' AND NPO_REQ=1";
                    break;
                case AutoCompleteType.PRA:
                    qryString = "SELECT COMP_ID AS id FROM COMPLIST WHERE COMP='" + selected_value + "' AND PRA_REQ=1";
                    break;
                case AutoCompleteType.ALLDocument:
                    qryString = "SELECT DOC_ID AS id FROM DOCLIST WHERE DOC='" + selected_value + "'";
                    break;
                case AutoCompleteType.EEDocument:
                    qryString = "SELECT DOC_ID AS id FROM DOCLIST WHERE DOC='" + selected_value + "' AND DOC_TYPE='EE'";
                    break;
                case AutoCompleteType.LICDocument:
                    qryString = "SELECT DOC_ID AS id FROM DOCLIST WHERE DOC='" + selected_value + "' AND DOC_TYPE='LIC'";
                    break;
                case AutoCompleteType.GENDocument:
                    qryString = "SELECT DOC_ID AS id FROM DOCLIST WHERE DOC='" + selected_value + "' AND DOC_TYPE='GEN'";
                    break;
            }

            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);                              // get provider
            dbmgr.ConnectionString = user.plantDBStr;                                       // set connection string

            try
            {
                dbmgr.Open();                                                               // open database
                dbmgr.ExecuteReader(CommandType.Text, qryString);                           // execute query
                while (dbmgr.DataReader.Read())                                             // get text and id for each item
                {
                    index = Convert.ToInt32(dbmgr.DataReader["id"]);
                    return index;
                }
            }
            catch
            {
                return index;
            }
            finally
            {
                dbmgr.Dispose();
            }
            
            return index;
        }

        #endregion

        #region combobox methods

        public static int SelectedItemId(ComboBoxItem.Type type, string selected_text)
        {
            // initialize
            int value = 0;
            string query;

            switch (type)
            {
                case ComboBoxItem.Type.Unit:
                    query = "SELECT UNIT_ID AS ID FROM UNITLIST WHERE UNIT ='" + selected_text + "'";
                    break;
                case ComboBoxItem.Type.Sys:
                    query = "SELECT SYS_ID AS ID FROM SYSLIST WHERE SYS ='" + selected_text + "'";
                    break;
                case ComboBoxItem.Type.Comp_Type:
                default:
                    query = "SELECT COMP_TYPE_ID AS ID FROM COMPTYPES WHERE COMP_TYPE ='" + selected_text + "'";
                    break;
            }

            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();
                dbmgr.ExecuteReader(CommandType.Text, query);
                while (dbmgr.DataReader.Read())
                {
                    value = Convert.ToInt32(dbmgr.DataReader["id"]);
                    return value;
                }
            }
            catch
            {
                return value;
            }
            finally
            {
                dbmgr.Dispose();
            }

            return value;
        }

        // creates arraylist for ComboBox, given initial message & type
        public static ArrayList FetchComboBoxList(string initMsg, ComboBoxItem.Type type)
        {
            // initialize
            ArrayList list = new ArrayList();
            string query;

            // determine table based on type passed
            switch (type)
            {
                case ComboBoxItem.Type.Box:
                    query = "SELECT * FROM BOXLIST ORDER BY BOX";
                    break;
                case ComboBoxItem.Type.Unit:
                    query = "SELECT * FROM UNITLIST ORDER BY UNIT";
                    break;
                case ComboBoxItem.Type.Sys:
                    query = "SELECT * FROM SYSLIST ORDER BY SYS";
                    break;
                case ComboBoxItem.Type.Position:
                    query = "SELECT * FROM POSLIST ORDER BY POS";
                    break;
                case ComboBoxItem.Type.Train:
                    query = "SELECT * FROM TRAINLIST ORDER BY TRAIN";
                    break;
                case ComboBoxItem.Type.Method:
                    query = "SELECT * FROM SSD_METHODLIST ORDER BY METHOD";
                    break;
                case ComboBoxItem.Type.Comp_Type:
                    query = "SELECT * FROM COMPTYPES ORDER BY COMP_TYPE";
                    break;
                case ComboBoxItem.Type.DrawingType:
                    query = "SELECT * FROM DWGTYPES WHERE DWGTYPE_ID <= 4";
                    break;
                case ComboBoxItem.Type.FZone:
                    query = "SELECT * FROM FZLIST ORDER BY FZ";
                    break;
                case ComboBoxItem.Type.FArea:
                    query = "SELECT * FROM FALIST ORDER BY FA";
                    break;
                case ComboBoxItem.Type.KSF:
                    query = "SELECT * FROM KSFLIST ORDER BY KSF";
                    break;
                case ComboBoxItem.Type.Bin:
                    query = "SELECT BIN_ID, BIN, BIN_DESC FROM BINLIST ORDER BY BIN_ID";
                    break;
                case ComboBoxItem.Type.ExportTables:
                    query = "SELECT COLTYPE_ID, COLTYPE_NAME, QUERY, EDIT FROM TABLECOLUMNTYPES WHERE EXPORT = 1";
                    break;
                case ComboBoxItem.Type.LookupTables:
                default:
                    query = "SELECT COLTYPE_ID, COLTYPE_NAME, QUERY, EDIT FROM TABLECOLUMNTYPES WHERE LEVEL = 1 ORDER BY COLTYPE_NAME";
                    break;
            }

            // connect to database
            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();
                dbmgr.ExecuteReader(CommandType.Text, query);
                
                // add the default item to list
                if (initMsg.Length > 0)
                    list.Add(new ComboBoxItem(initMsg, "0"));

                // add subsequent items to list
                while (dbmgr.DataReader.Read())
                {
                    switch (type)
                    {
                        case ComboBoxItem.Type.ExportTables:
                            // set item's name(COLTYPE_NAME), desc(""), value(ID), query(QUERY)
                            list.Add(new ComboBoxItem(dbmgr.DataReader.GetString(1), "", dbmgr.DataReader.GetInt32(0).ToString(), dbmgr.DataReader.GetString(2)));
                            break;
                        case ComboBoxItem.Type.LookupTables:
                            // set item's name(COLTYPE_NAME), desc(""), value(ID|EDIT), query(QUERY)
                            list.Add(new ComboBoxItem(dbmgr.DataReader.GetString(1), "", dbmgr.DataReader.GetInt32(0).ToString() + "|" + dbmgr.DataReader.GetBoolean(3).ToString(), dbmgr.DataReader.GetString(2)));
                            break;
                        case ComboBoxItem.Type.Bin:
                            list.Add(new ComboBoxItem(dbmgr.DataReader.GetString(1) + ": " + dbmgr.DataReader.GetString(2), dbmgr.DataReader.GetInt32(0).ToString()));
                            break;
                        default:
                            list.Add(new ComboBoxItem(dbmgr.DataReader.GetString(1), dbmgr.DataReader.GetInt32(0).ToString()));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        // creates arraylist for ComboBox, given initial message & type
        public static ArrayList FetchComboBoxList(string initMsg, ComboBoxItem.Type type, int id)
        {
            // initialize
            ArrayList list = new ArrayList();
            string query;

            // determine table based on type passed
            switch (type)
            {
                case ComboBoxItem.Type.FAFZ:
                default:
                    query = "SELECT FZ_ID, FZ FROM FZLIST WHERE FA_ID = @fa_id ORDER BY FZ";
                    break;
            }

            // connect to database
            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open(); 
                dbmgr.CreateParameters(1);
                dbmgr.AddParameters(0, "@fa_id", id);
                dbmgr.ExecuteReader(CommandType.Text, query);

                // add the default item to list
                if (initMsg.Length > 0)
                    list.Add(new ComboBoxItem(initMsg, "0"));

                // add subsequent items to list
                while (dbmgr.DataReader.Read())
                    list.Add(new ComboBoxItem(dbmgr.DataReader.GetString(1), dbmgr.DataReader.GetInt32(0).ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        // fetches list of users that can sign-off
        public static ArrayList FetchUserList(string existingUserInitials, string currentUserIntials)
        {
            // NOTE: existingUserInitials is current prep_by or chkd_by, currentUserInitials (curently logged in user)
            ArrayList list = new ArrayList();

            list.Add(new ComboBoxItem("N/A", "N/A"));
            if (existingUserInitials != "N/A")
                list.Add(new ComboBoxItem(existingUserInitials, existingUserInitials));
            if (existingUserInitials != currentUserIntials)
                list.Add(new ComboBoxItem(currentUserIntials, currentUserIntials));

            return list;
        }

        // update lists (1) insert, (2) update, (3) delete
        public static void SaveComboBoxList(ComboBoxItem item, IList list, ref bool in_use)
        {
            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();

                switch (item.update_type)
                {
                    // insert item to list
                    case 1:
                        dbmgr.CreateParameters(2);
                        dbmgr.AddParameters(0, "@name", item.name);
                        dbmgr.AddParameters(1, "@desc", item.desc);
                        dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, item.query + "_u");
                        break;
                    // update item in list
                    case 2:
                        dbmgr.CreateParameters(3);
                        dbmgr.AddParameters(0, "@value", item.value);
                        dbmgr.AddParameters(1, "@name", item.name);
                        dbmgr.AddParameters(2, "@desc", item.desc);
                        dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, item.query + "_u");
                        break;
                    // delete item from list
                    case 3:
                        dbmgr.CreateParameters(2);
                        dbmgr.AddParameters(0, "@value", item.value);
                        dbmgr.AddParameters(1, "@in_use", in_use, true);
                        dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, item.query + "_d");
                        in_use = Convert.ToBoolean(((System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(1)).Value);
                        break;
                }

                dbmgr.CommitTransaction();
            }
            catch (Exception ex)
            {
                dbmgr.RollbackTransaction();
                throw (ex);
            }
            finally
            {
                dbmgr.Dispose();
            }
        }

        #endregion

        #region lookup table methods
        
        // fetches an IList of the selected lookup table. This method supports the lookupTable.aspx page.
        public static IList FetchLookupTable(string type, ref string tableName)
        {
            ArrayList list = new ArrayList();
            string query = "SELECT * FROM %TABLE% ORDER BY %ITEM%";
            string table = "";
            string item = "";
            //ComboBoxItem item;

            // get table name from type
            // NOTE: type must match TABLECOLUMNTYPES.COLTYPE_NAME
            switch (type)
            {
                case "Units":
                    table = "UNITLIST";
                    item = "UNIT";
                    break;
                case "Systems":
                    table = "SYSLIST";
                    item = "SYS";
                    break;
                case "Positions":
                    table = "POSLIST";
                    item = "POS";
                    break;
                case "Trains":
                    table = "TRAINLIST";
                    item = "TRAIN";
                    break;
                case "Method":
                    table = "SSD_METHODLIST";
                    item = "METHOD";
                    break;
                case "Component Types":
                    table = "COMPTYPES";
                    item = "COMP_TYPE";
                    break;
                case "Snapshots":
                    table = "SNAPSHOTTYPES";
                    item = "SNAPSHOT_NAME";
                    break;
                case "Paths":
                    table = "PATHLIST";
                    item = "SNAPSHOT_NAME";
                    break;
                case "Modes":
                    table = "MODELIST";
                    item = "MODE";
                    break;
                case "Box":
                    table = "BOXLIST";
                    item = "BOX";
                    break;
                case "KSF":
                    table = "KSFLIST";
                    item = "KSF";
                    break;
            }

            tableName = table;
            query = query.Replace("%TABLE%", table);
            query = query.Replace("%ITEM%", item);

            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();
                dbmgr.ExecuteReader(CommandType.Text, query);

                while (dbmgr.DataReader.Read())
                    // set item's name(NAME), desc(DESC), value(ID), query(TABLE)
                    list.Add(new ComboBoxItem(dbmgr.DataReader.GetString(1), dbmgr.DataReader.GetString(2), dbmgr.DataReader.GetInt32(0).ToString(), table));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                dbmgr.Dispose();
            }
            return list;
        }

        #endregion

        #region history methods

        public static IList FetchHistory()
        {
            // initialize
            History item = new History();
            PropertyInfo[] p = item.GetType().GetProperties();
            List<History> list = new List<History>();

            string qryString = "SELECT * FROM HISTORY_LOG";

            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();
                dbmgr.ExecuteReader(CommandType.Text, qryString);

                while (dbmgr.DataReader.Read())
                {
                    item = new History();
                    item = (History)FetchObject(item, p, dbmgr);
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                dbmgr.Dispose();
            }
            return list;
        }

        #endregion

        #region export dataset method

        public static DataSet GenerateExport(int id, string query, string name)
        {
            DataSet ds = new DataSet(name);

            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();

                ds = dbmgr.ExecuteDataSet(CommandType.Text, query);
                ds.DataSetName = name;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                dbmgr.Dispose();
            }
            return ds;
        }

        #endregion
    }
}
