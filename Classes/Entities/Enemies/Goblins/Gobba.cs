using Pathfinding;

namespace Classes.Entities.Enemies.Goblins
{
    public class Gobba : Enemy
    {
        protected override void Start()
        {
            base.Start();
            var random = RandomPath.Construct(transform.position, 50000);
            seeker.StartPath(random);
        }
    }
}