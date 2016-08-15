namespace Crdt.Core
{
    public abstract class DataEntry
    {
        public string Name { get; set; }

        public abstract string Type { get; }
    }
}