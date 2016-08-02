using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core
{
    public class Configuaration
    {
        private string[] _replicas;

        public Configuaration(params string[] replicas)
        {
            _replicas = replicas;
        }
    }
}
