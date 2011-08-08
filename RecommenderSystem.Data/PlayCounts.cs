using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommenderSystem.Data
{
    public class PlayCounts : List<PlayCount>
    {
        public int TotalPlays { get; set; }

        public new void Add(PlayCount item)
        {
            base.Add(item);

            TotalPlays += item.Plays;
        }

        public new bool Remove(PlayCount item)
        {
            if (base.Remove(item))
            {
                TotalPlays -= item.Plays;
                return true;
            }

            return false;
        }

        public new void RemoveAt(int index)
        {
            TotalPlays -= this[index].Plays;
            base.RemoveAt(index);
        }

        public new int RemoveAll(Predicate<PlayCount> match)
        {
            var items = base.FindAll(match);
            foreach (var item in items)
                TotalPlays -= item.Plays;

            return base.RemoveAll(match);
        }
    }
}
