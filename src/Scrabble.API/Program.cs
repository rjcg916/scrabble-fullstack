using Scrabble.Domain;
using Scrabble.Domain.Interface;

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
    var game = Game.GameFactory.CreateGame(new Lexicon(), new List<Player>( names.Split(",", StringSplitOptions.RemoveEmptyEntries).Select( name => new Player(name) ).ToList()  ));
    var Id = GameManager.AddGame(game);
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

app.MapPost("/api/Games/{Id}/{playerName}/Board/Tiles", (Guid Id, string playerName, Coord coord, Tile tile, IGameManager GameManager) =>
{
    var game = GameManager.GetGame(Id);
    var board = game.Board;
    var player = game.Players.GetByName(playerName);

   // board.PlaceTile(coord, tile);

    return Results.Ok();
});

//app.MapGet("/api/Games/{Id}/Board/Squares", (Guid Id, IGameManager GameManager) =>
//{
//    var game = GameManager.GetGame(Id);
//    var board = game.Board;
//    return Results.Ok(board.GetLocationSquares());
//});

//app.MapGet("/api/Games/{Id}/Board/Tiles", (Guid Id, IGameManager GameManager) =>
//{
//    var game = GameManager.GetGame(Id);
//    var board = game.Board;
//    return Results.Ok(board.GetLocationSquares(filterForOccupied: true));
//});

app.Run();
