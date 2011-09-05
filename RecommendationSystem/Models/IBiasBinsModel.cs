namespace RecommendationSystem.Models
{
    public interface IBiasBinsModel : IModel
    {
        float[] BiasBins { get; set; }
    }
}