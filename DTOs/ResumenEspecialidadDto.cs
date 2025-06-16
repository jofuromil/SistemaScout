public class ResumenEspecialidadDto
{
    public Guid EspecialidadId { get; set; }
    public string Nombre { get; set; }
    public int TotalRequisitos { get; set; }
    public int Seleccionados { get; set; }
    public int Aprobados { get; set; }
    public bool Cumplida { get; set; }
}
