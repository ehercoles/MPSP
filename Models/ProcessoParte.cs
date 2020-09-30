namespace MPSP.Models
{
    public class ProcessoParte
    {
        public int ProcessoId { get; set; }
        public Processo Processo { get; set; }
        public int ParteId { get; set; }
        public Parte Parte { get; set; }
    }
}
