using Yarhl.FileFormat;
using Yarhl.FileSystem;

namespace AdolTranslator.Containers.Dat
{
    public class DatContainer2NodeContainer : IConverter<DatContainer, NodeContainerFormat>
    {
        public NodeContainerFormat Convert(DatContainer source)
        {
            var count = source.Blocks.Count;
            var container = new NodeContainerFormat();

            for (int i = 0; i < count; i++)
            {
                container.Root.Add(GenerateSingleNode($"{i}.bin", source.Blocks[i]));
            }

            return container;
        }

        private Node GenerateSingleNode(string name, byte[] block)
        {
            Node child = NodeFactory.FromMemory(name);
            child.Stream.Write(block, 0, block.Length);
            return child;
        }
    }
}
