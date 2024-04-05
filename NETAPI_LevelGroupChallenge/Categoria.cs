using System.ComponentModel.DataAnnotations;

namespace NETAPI_LevelGroupChallenge
{
    public class Categoria
    {


        [Key]
        public long Id { get; set; }

        public string? Name { get; set; }

    }
}
