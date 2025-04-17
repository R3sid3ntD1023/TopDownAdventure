public abstract class DialoqueBaseNode : RootNode
{

    public DialoqueBaseNode GetNext()
    {
        return Child as DialoqueBaseNode;
    }
}
