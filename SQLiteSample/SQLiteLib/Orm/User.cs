using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteLib.Orm
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("data1")]
        public int Data1 { get; set; }

        [Column("data2")]
        public string Data2 { get; set; }
        
        public virtual ICollection<UserParameter> Params { get; set; }
        
    }
}
