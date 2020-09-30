using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPSP.Models
{
    public class HistoricoProcesso
    {
        public HistoricoProcesso() { }

        public HistoricoProcesso(Processo processo)
        {
            ProcessoId = processo.Id;
            NumeroPrefixo = processo.NumeroPrefixo;
            Numero = processo.Numero;
            Data = processo.Data.Value;
            TipoProcesso = processo.TipoProcesso;
            Documento = processo.Documento;
            ExtensaoDocumento = processo.ExtensaoDocumento;
            Observacao = processo.Observacao;
        }

        public int Id { get; set; }

        public int ProcessoId { get; set; }
        public Processo Processo { get; set; }

        [Required]
        [StringLength(3)]
        public string NumeroPrefixo { get; set; }

        [Required]
        [Display(Name = "Número")]
        [StringLength(5)]
        [Range(00001, 99999)]
        public string Numero { get; set; }

        [NotMapped]
        public string NumeroProcesso { get => NumeroPrefixo + "-" + Numero; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required]
        [Display(Name = "Tipo do Processo")]
        public TipoProcesso TipoProcesso { get; set; }

        public byte[] Documento { get; set; }
        public string ExtensaoDocumento { get; set; }

        public string Observacao { get; set; }
    }
}
