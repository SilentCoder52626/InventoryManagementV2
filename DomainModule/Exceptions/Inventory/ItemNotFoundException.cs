
namespace InventoryLibrary.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message = "Item Has not been Found!") : base(message)
        {

        }
    }
}
