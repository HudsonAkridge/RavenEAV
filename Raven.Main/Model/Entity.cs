using System;
using System.Collections.Generic;

namespace RavenDbTest.Model
{
    public class Entity
    {
        public Entity()
        {
            AttributeValueStore = new AttributeValueStore();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public AttributeValueStore AttributeValueStore { get; set; }

        public void AddValue(Attribute attribute, object data)
        {
            AttributeValueStore.AddValue(Id, attribute, data);
        }

        public IEnumerable<IValue> GetAllValues()
        {
            return AttributeValueStore.GetAllValues();
        }
    }
}
