using static Classes.Utils.Flags;

namespace Interfaces
{
    public interface IState
    {
        public States State { get; }

        public void ChangeState(States state);
        
        public delegate void OnStateChangeDelegate(States from, States to);
        public event OnStateChangeDelegate onStateChangeEvent;
    }
}