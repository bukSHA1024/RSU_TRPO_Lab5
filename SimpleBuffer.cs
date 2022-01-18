using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5_mag
{
    class SimpleBuffer
    {
        private bool _finish = false;
        private bool _bEmpty = true;
        private int _buffer;

        private readonly List<Task> _writers = new();
        private readonly List<Task<int>> _readers = new();

        private readonly int _writersCount;
        private readonly int _readersCount;
        private readonly int _messagesCount;

        public SimpleBuffer(int writersCount, int readersCount, int n)
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
                    myMessages.Add(_buffer);
                    _bEmpty = true;
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
                if (_bEmpty)
                {
                    _buffer = myMessages[i++];
                    _bEmpty = false;
                }
            }
        }
    }
}