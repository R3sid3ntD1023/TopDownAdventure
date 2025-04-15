using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class QuestPlayer : MonoBehaviour, IAcceptQuest
{
    public float Speed = 100f;

    public QuestManager QuestManagerTemplate;

    private Rigidbody2D _RigidBody;

    public QuestManager _QuestManager;

    void Awake()
    {
        if (!QuestManagerTemplate)
            return;

        _QuestManager = Instantiate(QuestManagerTemplate);
        _QuestManager.OnReceiveReward.AddListener(OnReceiveReward);
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

    public QuestManager GetQuestManager()
    {
        return _QuestManager;
    }
}
