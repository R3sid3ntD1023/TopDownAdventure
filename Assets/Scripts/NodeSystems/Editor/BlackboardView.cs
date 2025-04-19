using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeSystem
{
    internal partial class BlackboardElementView : VisualElement
    {
        private Label _label = new Label { name = "label" };

        public string Title { get => _label.text; set => _label.text = value; }

        public BlackboardElementView()
        {
            Add(_label);
        }
    }

    internal partial class PopupElement : ListView
    {

        public PopupElement(Vector2 position)
        {

            this.style.position = Position.Absolute;
            this.style.top = position.y;
            this.style.left = position.x;
            this.style.width = 300;
            this.style.height = 600;
            this.style.backgroundColor = new StyleColor(new Color32(25, 25, 25, 255));
        }


    }

    [UxmlElement]
    public partial class BlackboardView : VisualElement
    {
        private Blackboard m_Blackboard;

        public IList GetKeyTypes { get => TypeCache.GetTypesDerivedFrom<BlackboardKeyBase>(); }

        public BlackboardView()
        {

            hierarchy.Add(CreateHeader());
            hierarchy.Add(CreateContent());
        }

        private VisualElement CreateContent()
        {
            var content = new VisualElement { name = "content" };
            return content;
        }

        private VisualElement CreateHeader()
        {
            var header = new VisualElement { name = "header" };
            header.style.flexDirection = FlexDirection.Row;
            header.style.justifyContent = Justify.SpaceBetween;
            header.style.alignContent = Align.Center;

            header.Add(new Label { name = "title", style = { fontSize = 20 } });
            header.Add(CreateAddButton());

            return header;
        }

        private VisualElement CreateAddButton()
        {
            var button = new Button { name = "add_button", text = "+" };
            button.clicked += () =>
            {
                PopupElement popupElement = new PopupElement(button.contentRect.position);
                popupElement.makeItem = MakePopupItem;
                popupElement.bindItem = BindPopupItem;
                popupElement.itemsSource = GetKeyTypes;
                this.parent.Insert(0, popupElement);
                popupElement.BringToFront();
            };

            return button;
        }

        private void BindPopupItem(VisualElement element, int arg2)
        {
            var elm = element as BlackboardElementView;
            elm.Title = TypeCache.GetTypesDerivedFrom<BlackboardKeyBase>()[arg2].Name;
        }

        private VisualElement MakePopupItem()
        {
            return new BlackboardElementView();
        }

        public void Initialize(NodeSystem.Blackboard blackboard)
        {
            m_Blackboard = blackboard;

            foreach (var key in m_Blackboard.Keys)
            {
                AddBlackboardKey(key.Name);
            }

            this.Q<Label>("title").text = blackboard.name;
        }


        private void AddBlackboardKey(string name)
        {

        }
    }
}