using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Aladdin.Core.Helpers
{
    public class MultiWatch
    {
        private readonly Stopwatch _sw = new Stopwatch();
        private readonly List<Tuple<string, long>> _marksSaved = new List<Tuple<string, long>>();
        private readonly List<Tuple<string, long>> _marksInProgress = new List<Tuple<string,long>>();
        public List<Tuple<string, long>> Marks => _marksSaved;

        public MultiWatch()
        {
            _sw.Start();
        }

        public void Start(string tag)
        {
            _marksInProgress.Add(Tuple.Create(tag, _sw.ElapsedMilliseconds));
        }

        public void Stop(string tag){
            var mark = _marksInProgress.FirstOrDefault(x => x.Item1 == tag);
            if(mark == null)
                throw new ArgumentException("mark not found");
            _marksInProgress.Remove(mark);
            _marksSaved.Add(Tuple.Create(mark.Item1,_sw.ElapsedMilliseconds - mark.Item2));
        }

        public void Measure(string tag, Action func)
        {
            var before = _sw.ElapsedMilliseconds;
            func();
            _marksSaved.Add(Tuple.Create(tag, _sw.ElapsedMilliseconds - before));
        }

        public T Measure<T>(string tag, Func<T> func)
        {
            var before = _sw.ElapsedMilliseconds;
            var res = func();
            _marksSaved.Add(Tuple.Create(tag, _sw.ElapsedMilliseconds - before));
            return res;
        }
    }
}