using System.Text;
using Yarhl.FileFormat;
using Yarhl.FileSystem;

namespace AdolTranslator.Containers.SE.DAT
{
    public class SeDatContainer2NodeContainer : IConverter<SeDatContainer, NodeContainerFormat>
    {
        public NodeContainerFormat Convert(SeDatContainer source)
        {
            var count = source.Count;
            var container = new NodeContainerFormat();

            for (int i = 0; i < count; i++)
            {
                container.Root.Add(GenerateSingleNode($"{i}.ogg", source.Blocks[i]));
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
