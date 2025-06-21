using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrackWPF.Models
{
    public class NewGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Devices { get; set; }
    }

    public class CreateGrupoResponse
    {
        public string Message { get; set; }
        public NewGroup NewGroup { get; set; }
    }
}