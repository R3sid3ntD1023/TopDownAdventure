using CustomAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Interactee)), RequireComponent(typeof(Inventory))]
public class QuestPlayer : MonoBehaviour, IAcceptQuest
{
    public float Speed = 100f;

    public QuestManager QuestManagerTemplate;

    private Rigidbody2D _RigidBody;

    [ReadOnlyProperty]
    public QuestManager _QuestManager;

    private Inventory _Inventory;

    private Interactee _Interactee;

    void Awake()
    {
        if (!QuestManagerTemplate)
            return;

        _QuestManager = Instantiate(QuestManagerTemplate);
        _QuestManager.OnReceiveReward.AddListener(OnReceiveReward);

        _Inventory = GetComponent<Inventory>();
        _Interactee = GetComponent<Interactee>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
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

    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.canceled)
            return;

        _Interactee.Interact();
    }

    public QuestManager GetQuestManager()
    {
        return _QuestManager;
    }
}
