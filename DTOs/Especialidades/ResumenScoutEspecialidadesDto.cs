namespace BackendScout.Dtos.Especialidades;

public class ResumenScoutEspecialidadesDto
{
    public Guid ScoutId { get; set; }
    public string NombreCompleto { get; set; }
    public List<ResumenEspecialidadDirigenteDto> Especialidades { get; set; }
}

public class ResumenEspecialidadDirigenteDto
{
    public Guid EspecialidadId { get; set; }
    public string Nombre { get; set; }
    public int Seleccionados { get; set; }
    public int Aprobados { get; set; }
    public bool Cumplida { get; set; }
}
