using Crdt.Core.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Crdt.Tests.MemoryStorageTests
{
    public class GetDocument
    {
        [Fact]
        public void it_gets_document()
        {
            var storage = new MemoryStorage(new FakeReplicasUpdater());
            var jo = new JObject();
            jo["name"] = "Alex";
            var result = storage.SaveDocument(jo);
            var doc = storage.GetDocument((string)result["id"]);
            var name = (string)doc["name"];
            Assert.Equal("Alex", name);
        }

        [Fact]
        public void it_gets_document_with_embeded_array()
        {
            var storage = new MemoryStorage(new FakeReplicasUpdater());
            var jo = JObject.Parse("{items: [{id: 1, name: \"Alex\"}, {id: 2, name: \"Alex\"}] }");
            var result = storage.SaveDocument(jo);
            var doc = storage.GetDocument((string)result["id"]);
            Assert.NotNull(doc["items"]);
        }
    }
}
