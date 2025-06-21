using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerTrackWPF.Models;

namespace PowerTrackWPF.Models;

public class EditGroupResponseDto
{
    public List<DispositivoDto> InGroup { get; set; }
    public List<DispositivoDto> OutGroup { get; set; }
}