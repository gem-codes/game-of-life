using System.ComponentModel.DataAnnotations;

namespace GameOfLifeTestProject.Models
{
    public class Board
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Rows { get; set; }

        [Required]
        public int Columns { get; set; }

        public string CurrentState { get; set; }
    }
}
