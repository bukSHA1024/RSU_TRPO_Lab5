using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab5_mag
{
    class MonitorBuffer
    {
        private bool _finish = false;
        private bool _bEmpty = true;
        private int _buffer;

        private readonly object _locker = new object();

        private readonly List<Task> _writers = new();
        private readonly List<Task<int>> _readers = new();

        private readonly int _writersCount;
        private readonly int _readersCount;
        private readonly int _messagesCount;

        public MonitorBuffer(int writersCount, int readersCount, int n)
        {
            _writersCount = writersCount;
            _readersCount = readersCount;
            _messagesCount = n;
        }

        public async Task<IEnumerable<int>> DoWork()
        {
            for (var i = 0; i < _writersCount; i++)
            {
                _writers.Add(Task.Run(() => WriterJob(i)));
            }
            for (var i = 0; i < _readersCount; i++)
            {
                _readers.Add(Task.Run(() => ReaderJob()));
            }

            await Task.WhenAll(_writers);
            _finish = true;
            await Task.WhenAll(_readers);
            var readedMessages = _readers.Select(task => task.Result);
            return readedMessages;
        }

        private int ReaderJob()
        {
            var myMessages = new List<int>();

            while (!_finish)
            {
                if (!_bEmpty)
                {
                    Monitor.Enter(_locker);
                    try
                    {
                        if (!_bEmpty)
                        {
                            myMessages.Add(_buffer);
                            _bEmpty = true;
                        }
                    }
                    finally
                    {
                        Monitor.Exit(_locker);
                    }
                }
            }

            return myMessages.Count;
        }

        private void WriterJob(int index)
        {
            var myMessages = new List<int>();
            for (var message = _messagesCount * index; message < _messagesCount * (index + 1); message++)
            {
                myMessages.Add(message);
            }

            var i = 0;
            while (i < _messagesCount)
            {
                Monitor.Enter(_locker);
                try
                {
                    if (_bEmpty)
                    {
                        _buffer = myMessages[i++];
                        _bEmpty = false;
                    }
                }
                finally
                {
                    Monitor.Exit(_locker);
                }
            }
        }
    }
}