using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War3Net.Build.Object;

namespace Launcher.MapObject
{
    public static class FieldFactory
    {

        public static SimpleObjectDataModification CreateIntField(int fieldId, int fieldValue)
        {
            return new()
            {
                Id = fieldId,
                Type = ObjectDataType.Int,
                Value = fieldValue,
            };
        }

        public static SimpleObjectDataModification CreateRealField(int fieldId, float fieldValue)
        {
            return new()
            {
                Id = fieldId,
                Type = ObjectDataType.Real,
                Value = fieldValue,
            };
        }

        public static SimpleObjectDataModification CreateStringField(int fieldId, string fieldValue)
        {
            return new()
            {
                Id = fieldId,
                Type = ObjectDataType.String,
                Value = fieldValue,
            };
        }

        public static SimpleObjectDataModification CreateUnrealField(int fieldId, float fieldValue)
        {
            return new()
            {
                Id = fieldId,
                Type = ObjectDataType.Unreal,
                Value = fieldValue,
            };
        }

        public static SimpleObjectDataModification CreateCustomField(int fieldID, ObjectDataType fieldType,  object fieldValue)
        {
            return new()
            {
                Id = fieldID,
                Type = fieldType,
                Value = fieldValue,
            };
        }
    }
}
