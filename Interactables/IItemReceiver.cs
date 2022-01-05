/// <summary>
/// Interface that all item receivers inherits from.
/// </summary>
public interface IItemReceiver
{
    /// <summary>
    /// The function that gets called when you input an item into an item receiver.
    /// </summary>
    /// <param name="item"></param>
    void ReceiveItem(InventoryItem item);

}

