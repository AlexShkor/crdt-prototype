using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crdt.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Crdt.Core.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var nodeCount = 3;
            var startPort = 5001;

            var nodes = RunNodes(startPort, nodeCount);

            Task.WaitAll(nodes.ToArray());
        }

        private static List<Task> RunNodes(int startPort, int nodeCount)
        {
            var currentPort = startPort;

            var nodeTasks = new List<Task>();
            var nodes = new List<string>();

            for (var i = 0; i < nodeCount; i++)
            {
                nodes.Add($"Node_{i + 1}");
            }

            foreach (var node in nodes)
            {
                var config = CreateNodeConfiguration(node, nodes);
                nodeTasks.Add(RunHost(config, currentPort));
                currentPort++;
            }
            return nodeTasks;
        }


        private static QueueConfiguration CreateNodeConfiguration(string node, IEnumerable<string> nodes)
        {
            return new QueueConfiguration
            {
                ConsumingQueue = node,
                SendingQueues = nodes.Except(new[] { node })
            };
        }

        private static Task RunHost(QueueConfiguration config, int port)
        {
            return Task.Factory.StartNew(() =>
            {
                var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://*:{port}")
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.Add(new ServiceDescriptor(typeof(QueueConfiguration), config));
                })
                .UseStartup<Startup>()
                .Build();

                host.Run();
            });
        }
    }
}
