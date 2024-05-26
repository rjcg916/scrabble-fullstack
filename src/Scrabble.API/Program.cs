using Scrabble.Domain.Model;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/Games", (IGameManager GameManager) => GameManager.NumberOfGames());

app.MapGet("/api/Games/{Id}", (Guid Id, IGameManager GameManager) =>
{
    var game = GameManager.GetGame(Id);
    return game == null ? Results.NotFound() : Results.Ok(game);
});

app.MapPost("/api/Games", (string names, IGameManager GameManager) =>
{
    var gameId = GameManager.CreateGame([.. names.Split(",")]);
    return Results.Ok(gameId);
});

app.MapPut("/api/Games/{id}", (int id, string value, IGames games) =>
{
    // Implementation for updating the game with the given id
    return Results.Ok();
});

app.MapDelete("/api/Games/{id}", (int id, IGames games) =>
{
    // Implementation for deleting the game with the given id
    return Results.Ok();
});

app.MapPost("/api/Games/{GameId}/{playerId}/Board/Tiles", (Guid GameId, int playerId, Coord coord, Tile tile, IGameManager GameManager) =>
{
    var game = GameManager.GetGame(GameId);
    var board = game.Board;
    var player = game.Players[(byte)playerId];

    Player.PlaceTile(board, coord, tile);
    return Results.Ok();
});

app.MapGet("/api/Games/{GameId}/Board/Squares", (Guid GameId, IGameManager GameManager) =>
{
    var game = GameManager.GetGame(GameId);
    var board = game.Board;
    return Results.Ok(board.GetCoordSquares());
});

app.MapGet("/api/Games/{GameId}/Board/Tiles", (Guid GameId, IGameManager GameManager) =>
{
    var game = GameManager.GetGame(GameId);
    var board = game.Board;
    return Results.Ok(board.GetCoordSquares(filterForOccupied: true));
});

app.Run();

