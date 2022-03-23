using System.ComponentModel.DataAnnotations;

namespace CleaningManagement.Api.Boundary.Request
{
    public class CleaningPlanForManipulationDto
    {
        [Required]
        [MaxLength(256)]
        public string Title { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        [MaxLength(512)]
        public string Description { get; set; }
    }
}
