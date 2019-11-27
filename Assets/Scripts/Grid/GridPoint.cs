using JetBrains.Annotations;

namespace Grid
{
    public class GridPoint
    {
        public readonly int value;
        
        public int depth;
        public readonly GridPoint pred;
        public int visits;

        public GridPoint(int value, int visits = 0, [CanBeNull] GridPoint pred = null)
        {
            this.value = value;
            depth = pred?.depth + 1 ?? 1;
            this.pred = pred;
            this.visits = visits;
        }

        public GridPoint GetPred(int dist = 1)
        {
            return dist <= 1 ? pred : pred.GetPred(dist - 1);
        }
    }
}
