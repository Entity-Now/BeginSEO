using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeginSEO.Utils
{
    /// <summary>
    /// 节流
    /// </summary>
    public class Throttle
    {
        Timer timer;
        public Throttle(Action func, int delay = 1000)
        {
            if (timer != null)
            {
                timer.Dispose();
            }
            timer = new Timer(state =>
            {
                func();
                timer.Dispose();
            },null, delay, Timeout.Infinite);
        }

        public static Action Debounce(Action func, int delay)
        {
            Timer _timer = null;
            return () =>
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                }
                _timer = new Timer(state =>
                {
                    func();
                    _timer.Dispose();
                },null, delay, Timeout.Infinite);
            };
        }

        public static Action<Action> DelayExecution(int delay)
        {
            Queue<Action> _List = new Queue<Action>();
            Timer _timer = new Timer(state => { });

            Action execute = null;
            execute = () =>
            {
                if (_List.Count > 0)
                {
                    var func = _List.Dequeue();
                    func();
                    if (_timer != null)
                    {
                        _timer.Dispose();
                        _timer = null;
                    }
                    _timer = new Timer(state => execute(), null, delay, Timeout.Infinite);
                }
            };

            return (Action func) =>
            {
                _List.Enqueue(func);

                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }

                _timer = new Timer(state => execute(), null, delay, Timeout.Infinite);
            };
        }

    }

}
