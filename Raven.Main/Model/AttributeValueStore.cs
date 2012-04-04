using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RavenDbTest.Model
{
    public class AttributeValueStore
    {
        public AttributeValueStore()
        {
            TextValues = new HashSet<TextValue>();
            DateValues = new HashSet<DateValue>();
            NumericValues = new HashSet<NumericValue>();
            BooleanValues = new HashSet<BooleanValue>();
            DisplayValues = new HashSet<DisplayValue>();
        }

        internal ICollection<TextValue> TextValues { get; set; }
        internal ICollection<DateValue> DateValues { get; set; }
        internal ICollection<NumericValue> NumericValues { get; set; }
        internal ICollection<BooleanValue> BooleanValues { get; set; }
        internal ICollection<DisplayValue> DisplayValues { get; set; }

        public IEnumerable<T> GetValuesByType<T>() where T : IValue
        {
            if (typeof(T) == typeof(TextValue))
                return TextValues.Cast<T>();
            if (typeof(T) == typeof(DateValue))
                return DateValues.Cast<T>();
            if (typeof(T) == typeof(NumericValue))
                return NumericValues.Cast<T>();
            if (typeof(T) == typeof(BooleanValue))
                return BooleanValues.Cast<T>();
            if (typeof(T) == typeof(DisplayValue))
                return DisplayValues.Cast<T>();

            return new Collection<T>();
        }

        public IEnumerable<IValue> GetAllValues()
        {
            var allValues = new List<IValue>();
            allValues.AddRange(TextValues);
            allValues.AddRange(DateValues);
            allValues.AddRange(NumericValues);
            allValues.AddRange(BooleanValues);
            allValues.AddRange(DisplayValues);
            
            return allValues;
        }

        public void AddValue(Guid parentId, Attribute attribute, object data)
        {
            var value = BuildValue(parentId, attribute);
            value.SetData(data);

            switch (attribute.Type)
            {
                case ValueTypes.Text:
                    TextValues.Add((TextValue)value);
                    break;
                case ValueTypes.Date:
                    DateValues.Add((DateValue)value);
                    break;
                case ValueTypes.Boolean:
                    BooleanValues.Add((BooleanValue)value);
                    break;
                case ValueTypes.Numeric:
                    NumericValues.Add((NumericValue)value);
                    break;
                case ValueTypes.Display:
                    DisplayValues.Add((DisplayValue)value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static IValue BuildValue(Guid parentId, Attribute attribute)
        {
            switch (attribute.Type)
            {
                case ValueTypes.Text:
                    return new TextValue { AttributeId = attribute.Id, ParentId = parentId };
                case ValueTypes.Date:
                    return new DateValue { AttributeId = attribute.Id, ParentId = parentId };
                case ValueTypes.Boolean:
                    return new BooleanValue { AttributeId = attribute.Id, ParentId = parentId };
                case ValueTypes.Numeric:
                    return new NumericValue { AttributeId = attribute.Id, ParentId = parentId };
                case ValueTypes.Display:
                    return new DisplayValue { AttributeId = attribute.Id, ParentId = parentId };
                case ValueTypes.Unknown:
                    return new GenericValue { AttributeId = attribute.Id, ParentId = parentId };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RemoveValue()
        {
            
        }
    }
}
