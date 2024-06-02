using Scrabble.Domain;

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
    var Id = GameManager.CreateGame([.. names.Split(",")]);
    return Results.Ok(Id);
});

app.MapPut("/api/Games/{Id}", (Guid Id, string value, IGames games) =>
{
    // Implementation for updating the game with the given id
    return Results.Ok();
});

app.MapDelete("/api/Games/{Id}", (Guid Id, IGames games) =>
{
    // Implementation for deleting the game with the given id
    return Results.Ok();
});

app.MapPost("/api/Games/{Id}/{playerId}/Board/Tiles", (Guid Id, int playerId, Coord coord, Tile tile, IGameManager GameManager) =>
{
    var game = GameManager.GetGame(Id);
    var board = game.Board;
    var player = game.Players[(byte)playerId];

    player.PlaceTile(board, coord, tile);
    return Results.Ok();
});

app.MapGet("/api/Games/{Id}/Board/Squares", (Guid Id, IGameManager GameManager) =>
{
    var game = GameManager.GetGame(Id);
    var board = game.Board;
    return Results.Ok(board.GetLocationSquares());
});

app.MapGet("/api/Games/{Id}/Board/Tiles", (Guid Id, IGameManager GameManager) =>
{
    var game = GameManager.GetGame(Id);
    var board = game.Board;
    return Results.Ok(board.GetLocationSquares(filterForOccupied: true));
});

app.Run();
