namespace Scrabble.WASM.Client.Services
{
    public class GameService
    {
        public event Func<Task> OnMoveButtonClicked;

        public async Task MoveButtonClicked()
        {
            if (OnMoveButtonClicked != null)
            {
                await OnMoveButtonClicked.Invoke();
            }
        }
    }
}
