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

app.MapGet("/api/Games", (IGames games) => games.Count());

app.MapGet("/api/Games/{id}", (int id, IGames games) =>
{
    var game = games.GetGame(id);
    return game == null ? Results.NotFound() : Results.Ok(game);
});

app.MapPost("/api/Games", (string names, IGames games) =>
{
    var gameId = games.CreateGame(names.Split(",").ToList());
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

app.MapPost("/api/Games/{gameId}/{playerId}/Board/Tiles", (int gameId, int playerId, TileLocation tileLocation, IGames games) =>
{
    var game = games.GetGame(gameId);
    var board = game.board;
    var player = game.players[(byte)playerId];

    player.PlaceTile(board, tileLocation);
    return Results.Ok();
});

app.MapGet("/api/Games/{gameId}/Board/Squares", (int gameId, IGames games) =>
{
    var game = games.GetGame(gameId);
    var board = game.board;
    return Results.Ok(board.GetCoordSquares());
});

app.MapGet("/api/Games/{gameId}/Board/Tiles", (int gameId, IGames games) =>
{
    var game = games.GetGame(gameId);
    var board = game.board;
    return Results.Ok(board.GetCoordSquares(filterForOccupied: true));
});

app.Run();

