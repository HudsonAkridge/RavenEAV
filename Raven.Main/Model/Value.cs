using System;

namespace RavenDbTest.Model
{
    public interface IValue
    {
        Guid ParentId { get; }
        Guid AttributeId { get; }
        object GetData();
        void SetData(object data);
    }

    public abstract class Value<T> : IValue
    {
        public Guid ParentId { get; protected internal set; }
        public Guid AttributeId { get; protected internal set; }

        public object GetData()
        {
            return Data;
        }

        public void SetData(object data)
        {
            SetData((T)data);
        }

        public abstract ValueTypes GetValueType();
        public T Data { get; protected set; }

        public void SetData(T value)
        {
            Data = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals((Value<T>)obj);
        }

        public bool Equals(Value<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.ParentId.Equals(ParentId) && other.AttributeId.Equals(AttributeId) && Equals(other.Data, Data);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = ParentId.GetHashCode();
                result = (result * 397) ^ AttributeId.GetHashCode();
                result = (result * 397) ^ Data.GetHashCode();
                return result;
            }
        }
    }

    public enum ValueTypes
    {
        Unknown,
        Text,
        Date,
        Boolean,
        Numeric,
        Display,
    }

    public class GenericValue : Value<object>
    {
        public override ValueTypes GetValueType()
        {
            return ValueTypes.Unknown;
        }
    }

    public class TextValue : Value<string>
    {
        public override ValueTypes GetValueType()
        {
            return ValueTypes.Text;
        }
    }

    public class DateValue : Value<DateTime?>
    {
        public override ValueTypes GetValueType()
        {
            return ValueTypes.Date;
        }
    }

    public class BooleanValue : Value<bool>
    {
        public override ValueTypes GetValueType()
        {
            return ValueTypes.Boolean;
        }
    }

    public class NumericValue : Value<Double>
    {
        public override ValueTypes GetValueType()
        {
            return ValueTypes.Numeric;
        }
    }

    public class DisplayValue : Value<int>
    {
        public override ValueTypes GetValueType()
        {
            return ValueTypes.Display;
        }
    }
}
