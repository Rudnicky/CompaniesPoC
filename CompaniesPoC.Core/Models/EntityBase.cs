using System.ComponentModel.DataAnnotations;

namespace CompaniesPoC.Core.Models
{
    public abstract class EntityBase
    {
        [Key]
        public long Id { get; set; }
    }
}
