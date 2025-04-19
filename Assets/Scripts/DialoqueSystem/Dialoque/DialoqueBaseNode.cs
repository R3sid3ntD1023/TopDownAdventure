
using NodeSystem;

namespace DialoqueSystem
{
    [OutputPin(typeof(DialoqueBaseNode))]
    [InputPin(typeof(DialoqueBaseNode))]
    public abstract class DialoqueBaseNode : RootNode
    {

        public DialoqueBaseNode GetNext()
        {
            return Child as DialoqueBaseNode;
        }
    }
}