using System;
using Raven.Client.Document;

namespace RavenDbTest.Data
{
    public static class RavenDbStoreManager
    {
        [ThreadStatic]
        private static DocumentStore _documentStore;

        public static DocumentStore GetDocumentStore()
        {
            return _documentStore ?? (_documentStore = new DocumentStore {Url = "Http://hakridge:8080"});
        }
    }
}
