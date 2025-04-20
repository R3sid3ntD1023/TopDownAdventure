using NodeSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialoqueSystem
{
    [UxmlElement]
    public partial class DialoqueGraphView : BaseGraphView<DialoqueBaseNode, DialoqueNodeView>
    {
        public DialoqueGraphView()
            : base(Resources.Load<DialoqueSettings>("DialoqueSettings").StyleSheet)
        {



        }

    }


    [UxmlElement]
    public partial class DialoqueNodeList : NodeListView<DialoqueBaseNode>
    {

    }

    public partial class DialoqueNodeView : NodeView
    {

        public DialoqueNodeView(DialoqueBaseNode node, string title)
            : base(node, title, AssetDatabase.GetAssetPath(DialoqueSettings.GetOrCreateSettings().NodeElementUxml))
        {
        }
    }
}