using System.ComponentModel.DataAnnotations;

namespace NETAPI_LevelGroupChallenge.Models
{
    public class TipoProduto
    {

        [Key]
        public long Id { get; set; }

        public string? Name { get; set; }
    }
}
