#region Licence
/*
 * Copyright 2018 Artur Wiater
 * 
 * This file is part of Collaborator+.
 * AdvancedPropertyGrid is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or(at your option) any later version.
 * AdvancedPropertyGrid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Collaborator+. 
 * If not, see http://www.gnu.org/licenses/.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DataProvider.Components.PropertyGrid
{
    /// <summary>
    /// Field Helper Class
    /// </summary>
    public class APGFieldHelper
    {
        /// <summary>
        /// Field label text
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Field label position againts edit box
        /// </summary>
        public APGFieldsLabelPosition LabelPosition { get; set; }

        /// <summary>
        ///  Field tootltip Text
        /// </summary>
        public string Tooltip { set; get; }

        /// <summary>
        /// Field name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Field value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Maximum chars could be type in edit box
        /// </summary>
        public Int32 MaxChars { get; set; }

        /// <summary>
        /// Field Type
        /// </summary>
        public APGFieldTypes EditType { get; set; }

        /// <summary>
        /// Field Datasource for IEnumerable edit boxes
        /// </summary>
        public IEnumerable<object> DataSource { get; set; }

        /// <summary>
        /// Panel containing field elements
        /// </summary>
        private Panel _Container { get; set; }

        /// <summary>
        /// Returning DockStyle acording to LabelPosition property
        /// </summary>
        public DockStyle GetDockStyle()
        {
            switch(LabelPosition)
            {
                case APGFieldsLabelPosition.Above:return DockStyle.Top;
                case APGFieldsLabelPosition.Bottom: return DockStyle.Bottom;
                case APGFieldsLabelPosition.Left: return DockStyle.Left;
                case APGFieldsLabelPosition.Right: return DockStyle.Right;
                case APGFieldsLabelPosition.Hide: return DockStyle.None;
                default:return DockStyle.None;
            }
        }

        /// <summary>
        /// Returnig Label object from field
        /// </summary>
        public Label GetLabel()
        {
            Label fldLabel = new Label();
            fldLabel.Text = Label;
            fldLabel.Name = "LabelBox";
            fldLabel.AutoSize = false;
            fldLabel.Height = 25;
            fldLabel.Dock = GetDockStyle();
            fldLabel.Visible = LabelPosition != APGFieldsLabelPosition.Hide;
            fldLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            return fldLabel;
        }

        /// <summary>
        /// Set Panel container for field elements
        /// </summary>
        public void setFieldContainer(Panel Container)
        {
            _Container = Container;
        }

        /// <summary>
        /// Return Panel container for field elements
        /// </summary>
        public Panel getFieldContainer()
        {
           return  _Container;
        }

    }

   
}
