using UnityEngine.Events;

//Interface used by an interactable item.
public interface IInteractable
{

    //Unity actions store a reference to a method that can be called at a later time.
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactSuccessful);

    //Not always used by the implement
    public void EndInteraction();
}
