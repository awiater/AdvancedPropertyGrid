using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace DataProvider.Components.PropertyGrid
{
    public delegate void DrawCustomField(APGFieldHelper e);

    public partial class AdvancedPropertyGrid : UserControl
    {
        private Dictionary<string, APGFieldHelper> _Fields = new Dictionary<string, APGFieldHelper>();

        public APGFieldHelper[] Fields
        {
            get { return _Fields.Values.ToArray(); }
            set
            {
                _Fields = value.ToDictionary(x => x.Name, x => x);
                DrawFields();
            }
        }

        public APGFieldsLabelPosition DefaultFieldLabelPosition { get; set; } = APGFieldsLabelPosition.Left;

        public bool UseDefaultSettings { get; set; } = false;

        public Int32 FieldsSeparatorHeight { get; set; } = 5;

        public event DrawCustomField DrawCustomField;


        public AdvancedPropertyGrid()
        {
            InitializeComponent();
        }

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

        public void DrawFields()
        {
            for (Int32 id= _Fields.Count-1; id>=0;id--)
            {
                flowBody.Controls.Add(CreateField(_Fields.Values.ToArray()[id]));
                flowBody.Controls.Add(new Panel() { Height = FieldsSeparatorHeight, Dock=DockStyle.Top });
            }
            Invalidate(true);
        }

        public object GetFieldValue(string Name)
        {

            if (_Fields.ContainsKey(Name))
            {
                object result = _Fields[Name].Value;
                return _Fields[Name].Value;
            }

            return null;
        }

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

        public void setFieldByName(string Name, APGFieldHelper Field)
        {
            if (_Fields.ContainsKey(Name))
            {
                _Fields[Name] = Field;
            }
        }

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
    }
}
