using Crdt.Core;
using Crdt.Core.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Crdt.Tests.MemoryStorageTests
{
    public class SaveDocumentTest
    {
        [Fact]
        public void it_saves_empty_document()
        {
            var storage = new MemoryStorage(new FakeReplicasUpdater());
            var jo = new JObject();
            var result = storage.SaveDocument(jo);
            Assert.NotEmpty((string)result["id"]);
        }

        [Fact]
        public void it_saves_not_empty_document()
        {
            var storage = new MemoryStorage(new FakeReplicasUpdater());
            var jo = new JObject();
            jo["name"] = "Alex";
            var result = storage.SaveDocument(jo);
            var name = (string)result["name"];
            Assert.Equal("Alex", name);
        }
    }

    public class FakeReplicasUpdater : IReplicasUpdater
    {
        public void ListenForUpdate(Action<AddToSetCommand> callback)
        {
        }

        public void SendUpdate(UpdateDocumentCommand update)
        {
        }
    }
}
