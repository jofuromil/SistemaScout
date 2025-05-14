namespace BackendScout.DTOs
{
    public class AgregarOrganizadorRequest
    {
        public int EventoId { get; set; }
        public Guid DirigentePrincipalId { get; set; }
        public Guid NuevoOrganizadorId { get; set; }
    }
}
