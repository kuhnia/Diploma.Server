namespace Diploma.UI.Entities.Models
{
    public class CounterSnapshot
    {
        public Guid Id { get; set; }
        public double CurrentValue { get; set; }
        public double DifferenceWithPreviousValue { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
