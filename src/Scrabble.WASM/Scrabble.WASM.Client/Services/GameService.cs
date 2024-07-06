namespace Scrabble.WASM.Client.Services
{
    public class GameService
    {
        public event Func<Task>? OnMoveRequest;

        public async Task Move()
        {
            if (OnMoveRequest != null)
            {
                await OnMoveRequest.Invoke();
            }
        }
    }
}
