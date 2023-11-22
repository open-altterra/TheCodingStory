using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinksStorage : MonoBehaviour
{
    public static LinksStorage Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [field: SerializeField]
    public CharacterController CharacterController { get; private set; }

    [field: SerializeField]
    public SceneController SceneController { get; private set; }

    [field: SerializeField]
    public EventsController EventsController { get; private set; }

    [field: SerializeField]
    public UIController UIController { get; private set; }

    [field: SerializeField]
    public DialogsController DialogController { get; private set; }

    [field: SerializeField]
    public DialogsStorage DialogsStorage { get; private set; }
}
