#region Licence
/*
 * Copyright 2018 Artur Wiater
 * 
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
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace DataProvider.Components.PropertyGrid
{
    /// <summary> 
    /// DrawCustomField Delegate for DrawCustomField event
    /// </summary>
    public delegate void DrawCustomField(APGFieldHelper e);

    /// <summary> 
    /// AdvancedPropertyGrid Control Class 
    /// </summary>
    public partial class AdvancedPropertyGrid : UserControl
    {
        #region Private Properties
        /// <summary> 
        /// Conrtainer of fields objects 
        /// </summary>
        private Dictionary<string, APGFieldHelper> _Fields = new Dictionary<string, APGFieldHelper>();
        #endregion

        #region Public Properties
        /// <summary> 
        /// Array with fields objects 
        /// </summary>
        [Description("Array with fields objects")]
        public APGFieldHelper[] Fields
        {
            get { return _Fields.Values.ToArray(); }
            set
            {
                _Fields = value.ToDictionary(x => x.Name, x => x);
                DrawFields();
            }
        }

        /// <summary> 
        /// Default position of field labels 
        /// </summary>
        [Description("Default position of field labels")]
        public APGFieldsLabelPosition DefaultFieldLabelPosition { get; set; } = APGFieldsLabelPosition.Left;

        /// <summary> 
        /// When true default settings will be used 
        /// </summary>
        [Description("Default Settings usage")]
        public bool UseDefaultSettings { get; set; } = false;

        /// <summary> 
        /// Size of fields separator
        /// </summary>
        [Description("Fields separator height")]
        public Int32 FieldsSeparatorHeight { get; set; } = 5;

        /// <summary> 
        /// Hide / Show field tooltip panel
        /// </summary>
        [Description("Tooltip panel visibility")]
        public bool TooltipPanelVisible
        {
            get { return lblInfo.Visible; }
            set { lblInfo.Visible = value; Invalidate(true); }
        }
        #endregion

        #region Events
        /// <summary> 
        /// Event executed when draw custom field 
        /// </summary>
        [Description("Holds custom fields execution")]
        public event DrawCustomField DrawCustomField;
        #endregion

        #region Private Methods
        /// <summary> 
        /// Create field control
        /// </summary>
        /// <param name="Field">Field object</param>
        private Panel CreateField(APGFieldHelper Field)
        {
            Panel fldPanel = new Panel();
            fldPanel.Dock = DockStyle.Top;
            fldPanel.BorderStyle = BorderStyle.Fixed3D;
            Field.LabelPosition = UseDefaultSettings ? DefaultFieldLabelPosition : Field.LabelPosition;
            fldPanel.Margin = new Padding(0, 0, 0, 5);
            

            fldPanel.ControlAdded += (s, e) =>
              {
                  if (e.Control.Name != "LabelBox")
                  {
                      e.Control.Name = "EditBox";
                      if (Field.LabelPosition == APGFieldsLabelPosition.Left || Field.LabelPosition == APGFieldsLabelPosition.Right || Field.LabelPosition == APGFieldsLabelPosition.Hide)
                      {
                          fldPanel.Height = e.Control.Height+10;
                      }
                      else
                      if (Field.LabelPosition == APGFieldsLabelPosition.Above || Field.LabelPosition == APGFieldsLabelPosition.Bottom)
                      {
                          fldPanel.Height =( e.Control.Height * 2)+10;
                      }
                  }
                  e.Control.MouseClick += (ss, ee) =>
                  {
                      lblInfo.Text = Field.Tooltip;
                  };
                  
              };
            Field.setFieldContainer(fldPanel);
            fldPanel.Name = Field.Name;
            CreateEditControl(Field);
            fldPanel.Controls.Add(Field.GetLabel());
            return fldPanel;
        }

        /// <summary> 
        /// Create edit box using FieldType property 
        /// </summary>
        /// <param name="Field">Field object</param>
        private void CreateEditControl(APGFieldHelper Field)
        {
            switch (Field.EditType)
            {
                case APGFieldTypes.BasicEditField:
                    TextBox txt = new TextBox();
                    txt.Dock = DockStyle.Fill;
                    txt.Text = string.Format("{0}", Field.Value);
                    txt.Name = "EditBox";
                    txt.MaxLength = Field.MaxChars;
                    Field.getFieldContainer().Controls.Add(txt);
                    break;
                case APGFieldTypes.CustomField:
                    DrawCustomField(Field);
                    break;
            }
        }
        #endregion

        #region Public Methods
        /// <summary> 
        /// InitializeComponent
        /// </summary>
        public AdvancedPropertyGrid()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Draw fields in control body 
        /// </summary>
        public void DrawFields()
        {
            for (Int32 id= _Fields.Count-1; id>=0;id--)
            {
                flowBody.Controls.Add(CreateField(_Fields.Values.ToArray()[id]));
                flowBody.Controls.Add(new Panel() { Height = FieldsSeparatorHeight, Dock=DockStyle.Top });
            }
            Invalidate(true);
        }

        /// <summary> 
        /// Return field value by given name 
        /// </summary>
        /// <param name="Name">Name of field</param>
        public object GetFieldValue(string Name)
        {

            if (_Fields.ContainsKey(Name))
            {
                object result = _Fields[Name].Value;
                return _Fields[Name].Value;
            }

            return null;
        }

        /// <summary> 
        /// Set field value by given name 
        /// </summary>
        /// <param name="Name">Name of field</param>
        /// <param name="Value">Field value to set</param>
        /// <param name="isFilling">Optional field not in use now</param>
        public void SetFieldValue(string Name, object Value, bool isFilling = false)
        {
            if (_Fields.ContainsKey(Name))
            {
                _Fields[Name].Value = Value;
                if (!isFilling)
                {
                    flowBody.Controls[Name].Controls["EditBox"].Text = string.Format("{0}", Value);
                }
            }
        }

        /// <summary> 
        /// Change field object by given name
        /// </summary>
        /// <param name="Name">Name of field</param>
        /// <param name="Field">New field object</param>
        public void setFieldByName(string Name, APGFieldHelper Field)
        {
            if (_Fields.ContainsKey(Name))
            {
                _Fields[Name] = Field;
            }
        }

        /// <summary> 
        /// Create fields list from properties of given object 
        /// </summary>
        /// <param name="ObjectWithFields">Any object with Fields Attributes setup against properties</param>
        public void CreateFieldsFromObject(object ObjectWithFields)
        {
            List<APGFieldHelper> EditFieldsList = new List<APGFieldHelper>();
            foreach (PropertyInfo prop in ObjectWithFields.GetType().GetProperties())
            {
                LabelAttribute LabelAttribute = DataProvider.Parsers.VariableParser.GetPropertyAttribute(prop.Name, "LabelAttribute", ObjectWithFields) as LabelAttribute;
                TooltipAttribute TooltipAttribute = DataProvider.Parsers.VariableParser.GetPropertyAttribute(prop.Name, "TooltipAttribute", ObjectWithFields) as TooltipAttribute;
                LabelPositionAttribute LabelPositionAttribute = DataProvider.Parsers.VariableParser.GetPropertyAttribute(prop.Name, "LabelPositionAttribute", ObjectWithFields) as LabelPositionAttribute;
                MaxCharsAttribute MaxCharsAttribute = DataProvider.Parsers.VariableParser.GetPropertyAttribute(prop.Name, "MaxCharsAttribute", ObjectWithFields) as MaxCharsAttribute;
                FieldEditTypeAttribute FieldEditTypeAttribute = DataProvider.Parsers.VariableParser.GetPropertyAttribute(prop.Name, "FieldEditTypeAttribute", ObjectWithFields) as FieldEditTypeAttribute;

                EditFieldsList.Add(new APGFieldHelper()
                {
                    Name = prop.Name,
                    Label = LabelAttribute!=null? LabelAttribute.value:"",
                    LabelPosition= LabelPositionAttribute != null ? LabelPositionAttribute.value :APGFieldsLabelPosition.Left,
                    Tooltip= TooltipAttribute != null ? TooltipAttribute.value : "",
                    Value=  prop.GetValue(ObjectWithFields),
                    MaxChars= MaxCharsAttribute != null ? MaxCharsAttribute.value:9999,
                    EditType= FieldEditTypeAttribute != null ? FieldEditTypeAttribute.value:APGFieldTypes.TextField,
                });

            }
            _Fields = EditFieldsList.ToDictionary(x=>x.Name,x=>x);
            DrawFields();
        }
        #endregion
    }
}
