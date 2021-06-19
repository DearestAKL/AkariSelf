using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    public class GameEvent
    {
        public EventType Type;
        public object[] Params = null;

        public GameEvent()
        {

        }

        public GameEvent(EventType type, params object[] InParams)
        {
            Type = type;
            Params = InParams;
        }
    }
}
