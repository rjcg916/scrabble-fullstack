using AngleSharp.Dom;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using Scrabble.Domain;
using Scrabble.WASM.Client.Components;
using Scrabble.WASM.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrabble.Blazor.Test
{

    public class GameBoardTests 
    {
        [Fact]
        public void GameBoard_CurrentPlayerParameter_Changed()
        {
            Action<Move> onSomethingHandler = _ => { };
            
            var ctx = new BunitContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            ctx.Services.AddMudServices();

            var cut = ctx.Render<GameBoard>(parameters => parameters               
              .Add(p => p.board, new ((s) => true) )
              .Add(p => p.rack, new Rack())
              .Add(p => p.CurrentPlayer, 1)
              .Add(p => p.CurrentMoveChanged, onSomethingHandler)
            );

            var init = cut.Instance;
            Assert.Equal(1, init.CurrentPlayer);
           
            cut.Render<GameBoard>(parameters => parameters
                .Add(p => p.CurrentPlayer, 2)
            );

            var snd = cut.Instance;
           
            Assert.Equal(2, snd.CurrentPlayer);

        }

        [Fact]
        public void GameBoardDropZone_CorrectDropZones()
        {
            Action<Move> onSomethingHandler = _ => { };

            var ctx = new BunitContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            ctx.Services.AddMudServices();
   
            var rack = new Rack();
            var tilesToAdd = new List<Tile> { new('A'), new('B') };
            rack = rack.AddTiles(tilesToAdd);

            var cut = ctx.Render<GameBoard>(parameters => parameters
              .Add(p => p.board, new((s) => true))
              .Add(p => p.rack, rack)
              .Add(p => p.CurrentPlayer, 1)
              .Add(p => p.CurrentMoveChanged, onSomethingHandler)
            );

            var container = cut.Find(".mud-drop-container");
            container.Children.Should().HaveCount(2);

            cut.FindAll(".mud-drop-zone").Should().HaveCount(232);

            var rackDropZones = container.Children[0];
            rackDropZones.Children.Should().HaveCount(2);

            var firstRackDropZoneA = rackDropZones.Children[0];
            firstRackDropZoneA.TextContent.Should().Be("Tile Rack");

            var boardRowDropZones = container.Children[1];
            boardRowDropZones.Children.Should().HaveCount(16);

        }
    }
}
