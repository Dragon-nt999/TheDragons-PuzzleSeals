using Godot;
using System;
using System.Threading.Tasks;
namespace TheDragonsPuzzleSeals.Features.Map
{
    public interface ICommand
    {
        Task ExecuteAync();
    }
}

