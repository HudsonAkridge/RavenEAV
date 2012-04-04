using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Linq;
using RavenDbTest.Data;
using RavenDbTest.Model;
using Attribute = RavenDbTest.Model.Attribute;

namespace RavenDbTest.Tests
{
    [TestFixture]
    public class EntityTests
    {
        private DocumentStore _documentStore;

        [TestFixtureSetUp]
        public void Setup()
        {
            _documentStore = RavenDbStoreManager.GetDocumentStore();
            _documentStore.Initialize();
            using (var session = _documentStore.OpenSession())
            {
                var entities = session.Query<Entity>();
                var attributes = session.Query<Attribute>();

                foreach (var entity in entities) session.Delete(entity);
                foreach (var attribute in attributes) session.Delete(attribute);
                session.SaveChanges();
            }
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _documentStore.Dispose();
        }

        [Test]
        public void CanSaveAndRetrieveBasicEntity()
        {
            var entity = new Entity { Name = "TestEntity" };
            using (var session = _documentStore.OpenSession())
            {
                var attribute = new Attribute { Name = "TestAttribute", Type = ValueTypes.Text };
                session.Store(attribute);
                session.Store(entity);

                entity.AddValue(attribute, "Test");

                session.SaveChanges();
            }

            Entity retrievedEntity;
            using (var session = _documentStore.OpenSession())
            {
                retrievedEntity = session.Load<Entity>("entities/" + entity.Id);
            }

            Assert.AreEqual(entity.Name, retrievedEntity.Name);
            CollectionAssert.AreEquivalent(entity.GetAllValues(), retrievedEntity.GetAllValues());
        }

        [Test]
        public void CanSaveAndRetrieveEntityWithEveryValueType()
        {
            var entity = new Entity { Name = "TestEntity" };
            using (var session = _documentStore.OpenSession())
            {
                var textAtt = new Attribute { Name = "TextAttribute", Type = ValueTypes.Text };
                var numericAtt = new Attribute { Name = "NumericAttribute", Type = ValueTypes.Numeric };
                var dateAtt = new Attribute { Name = "DateAttribute", Type = ValueTypes.Date };
                var boolAtt = new Attribute { Name = "BooleanAttribute", Type = ValueTypes.Boolean };
                var displayAtt = new Attribute { Name = "DisplayAttribute", Type = ValueTypes.Display };

                session.Store(textAtt);
                session.Store(numericAtt);
                session.Store(dateAtt);
                session.Store(boolAtt);
                session.Store(displayAtt);
                session.Store(entity);

                entity.AddValue(textAtt, "Test");
                entity.AddValue(numericAtt, 32891.28);
                entity.AddValue(dateAtt, DateTime.Now);
                entity.AddValue(boolAtt, false);
                entity.AddValue(displayAtt, 8);

                session.SaveChanges();
            }

            Entity retrievedEntity;
            using (var session = _documentStore.OpenSession())
            {
                retrievedEntity = session.Load<Entity>("entities/" + entity.Id);
            }

            Assert.AreEqual(entity.Name, retrievedEntity.Name);
            CollectionAssert.AreEquivalent(entity.GetAllValues(), retrievedEntity.GetAllValues());
        }


        [Test]
        public void CanSearchEntityByTextValue()
        {
            var entity = new Entity { Name = "TestEntity" };
            var entity2 = new Entity { Name = "TestEntity2" };
            var textAtt = new Attribute { Name = "TextAttribute", Type = ValueTypes.Text };

            using (var session = _documentStore.OpenSession())
            {

                session.Store(textAtt);
                session.Store(entity);
                session.Store(entity2);

                entity.AddValue(textAtt, "Test");

                session.SaveChanges();
            }

            IEnumerable<Entity> retrievedEntities;
            using (var session = _documentStore.OpenSession())
            {
                retrievedEntities = from e in session.Query<Entity>()
                                    where e.AttributeValueStore.TextValues.Any(x => x.Data.Equals("Test", StringComparison.InvariantCultureIgnoreCase) 
                                        && x.AttributeId == textAtt.Id)
                                    select e;

            }

            Assert.AreEqual(1, retrievedEntities.Count());
            Assert.AreEqual(entity.Name, retrievedEntities.First().Name);
        }
    }
}
