using System.Collections;
using System.Xml.Linq;

namespace AdventOfCode2022.Dto.Day20
{
    public class CircularNodes : IEnumerable<long>
    {
        public CircularNodes(long value) { Value = value; }

        public CircularNodes Previous;
        public CircularNodes Next;
        public long Value;

        public IEnumerator<long> GetEnumerator()
        {
            var current = this;
            foreach (int i in Enumerable.Range(0, 3001))
            {
                yield return current.Value;
                current = current.Next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void MoveRight()
        {
            var oldPrevious = Previous;
            var oldNext = Next;
            var nextOfNext = Next.Next;

            oldPrevious.Next = oldNext;
            oldNext.Previous = oldPrevious;
            oldNext.Next = this;
            Previous = oldNext;
            Next = nextOfNext;
            nextOfNext.Previous = this;
        }
    }
}
