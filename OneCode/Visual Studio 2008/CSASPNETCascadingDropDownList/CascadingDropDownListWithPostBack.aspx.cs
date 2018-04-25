﻿/****************************** Module Header ******************************\
* Module Name:  CascadingDropDownListWithPostBack.aspx.cs
* Project:      CSASPNETCascadingDropDown
* Copyright (c) Microsoft Corporation.
* 
* This page is used to show the Cascading DropDownList with postback.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* History:
* * 7/24/2009 10:33 AM Thomas Sun Created
\***************************************************************************/

#region Using directives
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
#endregion


namespace CSASPNETCascadingDropDownList
{
    public partial class CascadingDropDownListWithPostBack : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind Country dropdownlist
                BindddlCountry();
                ddlRegion.Enabled = false;
                ddlCity.Enabled = false;

                // Insert one item to dropdownlist top
                ddlRegion.Items.Insert(0, new ListItem("Select Region", "-1"));
                ddlCity.Items.Insert(0, new ListItem("Select City", "-1"));

                // Initialize city dropdownlist selected index
                hdfDdlCitySelectIndex.Value = "0";
            }
        }


        /// <summary>
        /// Bind Country dropdownlist
        /// </summary>
        public void BindddlCountry()
        {
            List<string> list = RetrieveDataFromXml.GetAllCountry();
            ddlCountry.DataSource = list;
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("Select Country", "-1"));
        }


        /// <summary>
        /// Show selected value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Get the selected index of city dropdownlist
            int iCitySelected = Convert.ToInt16(hdfDdlCitySelectIndex.Value);

            // The result will be showing
            string strResult = string.Empty;
            if (ddlCountry.SelectedIndex == 0)
            {
                strResult = "Please select Country firstly.";
            }
            else if (ddlRegion.SelectedIndex == 0 && strResult == string.Empty)
            {
                strResult = "Please select Region firstly.";
            }
            else if (hdfDdlCitySelectIndex.Value == "0" && strResult == string.Empty)
            {
                strResult = "Please select City firstly.";
            }
            else
            {
                strResult = "You select Country: " + ddlCountry.SelectedValue
                    + " ; Region: " + ddlRegion.SelectedValue
                    + " ; City: " + ddlCity.Items[iCitySelected].Value;
            }

            lblResult.Text = strResult;
        }


        /// <summary>
        /// Country dropdownlist SelectedIndexChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Remove region dropdownlist items
            ddlRegion.Items.Clear();
            string strCountry = string.Empty;
            strCountry = ddlCountry.SelectedValue;
            List<string> list = null;

            // Bind Region dropdownlist basing on country value
            if (ddlCountry.SelectedIndex != 0)
            {
                list = RetrieveDataFromXml.GetRegionByCountry(strCountry);
                if (list != null && list.Count != 0)
                {
                    ddlRegion.Enabled = true;
                }

                ddlRegion.DataSource = list;
                ddlRegion.DataBind();
            }
            else
            {
                ddlRegion.Enabled = false;
            }

            ddlRegion.Items.Insert(0, new ListItem("Select Region", "-1"));

            // Clear city dropdownlist
            ddlCity.Enabled = false;
            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("Select City", "-1"));

            // Initialize city dropdownlist selected index
            hdfDdlCitySelectIndex.Value = "0";
        }


        /// <summary>
        /// Region dropdownlist SelectedIndexChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Bind city dropdownlist basing on region value
            string strRegion = string.Empty;
            strRegion = ddlRegion.SelectedValue;
            List<string> list = null;
            list = RetrieveDataFromXml.GetCityByRegion(strRegion);
            ddlCity.Items.Clear();
            ddlCity.DataSource = list;
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("Select City", "-1"));

            // Initialize city dropdownlist selected index
            hdfDdlCitySelectIndex.Value = "0";

            // Enable city dropdownlist when it has items
            if (list.Count > 0)
            {
                ddlCity.Enabled = true;
            }
            else
            {
                ddlCity.Enabled = false;
            }
        }
    }
}
