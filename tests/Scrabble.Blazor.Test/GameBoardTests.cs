using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Charts;
using Scrabble.Domain;
using Scrabble.WASM.Client.Components;
using Scrabble.WASM.Client.Models;
using System;
using Telerik.JustMock;
using MudBlazor.Services;

namespace Scrabble.Blazor.Test
{

    public class GameBoardTests 
    {
        [Fact]
        public void GameBoard_CurrentPlayerParameter_Changed()
        {
            Action<Move> onSomethingHandler = _ => { };
            
            var ctx = new TestContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            ctx.Services.AddMudServices();

            var cut = ctx.RenderComponent<GameBoard>(parameters => parameters               
              .Add(p => p.board, new ((s) => true) )
              .Add(p => p.rack, new Rack())
              .Add(p => p.CurrentPlayer, 1)
              .Add(p => p.CurrentMoveChanged, onSomethingHandler)
            );

            var init = cut.Instance;
            Assert.Equal(1, init.CurrentPlayer);

            cut.SetParametersAndRender<GameBoard>(parameters => parameters
                .Add(p => p.board, new((s) => true))
                .Add(p => p.rack, new Rack())
                .Add(p => p.CurrentPlayer, 2)
                .Add(p => p.CurrentMoveChanged, onSomethingHandler)
            );

            var snd = cut.Instance;

            Assert.Equal(2, snd.CurrentPlayer);


        }
    
    }
}
