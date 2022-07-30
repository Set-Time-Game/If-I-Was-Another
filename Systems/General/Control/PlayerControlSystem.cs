using Leopotam.Ecs;
using Types.Classes;
using Chunk = Types.Structs.Chunk;

namespace Systems.General.Control
{
    public class PlayerControlSystem : IEcsRunSystem, IEcsPreInitSystem
    {
        public MapData MapData = null;
        public Player Player = null;
        
        private EcsComponentRef<Chunk>[] _currentMap;

        public void PreInit() => _currentMap = new EcsComponentRef<Chunk>[9];

        public void Run()
        {
            /*foreach (var i in _currentMap[5].Unref().Grounds)
            {
                
            }*/
        }
    }
}