using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class QuestPlayer : MonoBehaviour, IAcceptQuest
{
    public float Speed = 100f;

    public QuestManager QuestManager;

    private Rigidbody2D _RigidBody;

    public QuestManager _QuestManager;

    QuestManager IAcceptQuest.QuestManager => _QuestManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _RigidBody = GetComponent<Rigidbody2D>();

        if (!QuestManager)
            return;

        _QuestManager = Instantiate(QuestManager);
        _QuestManager.OnReceiveReward.AddListener(OnReceiveReward);

        UIDocument document = null;
        if (TryGetComponent<UIDocument>(out document))
        {
            if (document.rootVisualElement.Q<ScrollView>("ActiveQuests") is var active && active != null)
            {
                active.Bind(new SerializedObject(_QuestManager));
            }

            if (document.rootVisualElement.Q<ScrollView>("CompletedQuests") is var completed && completed != null)
            {
                completed.Bind(new SerializedObject(_QuestManager));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnReceiveReward(QuestReward reward)
    {

    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        var value = callbackContext.ReadValue<Vector2>();

        _RigidBody.linearVelocity = new Vector3(value.x, value.y, 0) * Time.deltaTime * Speed;
    }
}
