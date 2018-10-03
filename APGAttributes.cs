#region Licence
/*
 * Copyright 2018 Artur Wiater
 * 
 * This file is part of Collaborator+.
 *AdvancedPropertyGrid is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or(at your option) any later version.
 *AdvancedPropertyGrid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Collaborator+. 
 * If not, see http://www.gnu.org/licenses/.
*/
#endregion
using System;

/// <summary> 
/// Fields Attributes File
/// </summary>
namespace DataProvider.Components.PropertyGrid
{
    /// <summary>
    /// Field label Text
    /// </summary>
    public class LabelAttribute : Attribute
    {
        public string value;

        public LabelAttribute(string v)
        {
            this.value = v;
        }
    }

    /// <summary>
    /// Field tootltip Text
    /// </summary>
    public class TooltipAttribute : Attribute
    {
        public string value;

        public TooltipAttribute(string v)
        {
            this.value = v;
        }
    }

    /// <summary>
    /// Field label position againts edit box
    /// </summary>
    public class LabelPositionAttribute : Attribute
    {
        public APGFieldsLabelPosition value;

        public LabelPositionAttribute(APGFieldsLabelPosition v)
        {
            this.value = v;
        }
    }

    /// <summary>
    /// Maximum chars could be type in edit box
    /// </summary>
    public class MaxCharsAttribute : Attribute
    {
        public int value;

        public MaxCharsAttribute(int v)
        {
            this.value = v;
        }
    }

    /// <summary>
    /// Field Type
    /// </summary>
    public class FieldEditTypeAttribute : Attribute
    {
        public APGFieldTypes value;

        public FieldEditTypeAttribute(APGFieldTypes v)
        {
            this.value = v;
        }
    }
}
