using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ru.snowprelicator.populate_database
{
    [Table("horse", Schema = "public")]
    public class Horse
    {
        [Column("horseid", TypeName = "int")]
        public int HorseId { get; set; }

        [Column("name", TypeName = "text")]
        public string Name { get; set; }

        [Column("weight", TypeName = "double precision")]
        public double Weight { get; set; }

        [Column("birthday", TypeName = "timestamp without time zone")]
        public DateTime BirthDay { get; set; }
    }
}
