using RecommenderSystem.Data;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RecommenderSystem.Knn
{
    public class KnnUser : User
    {
        [XmlIgnore]
        public SortedSet<User> Neighbours { get; set; }
    }
}
