using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPSP.Models
{
    public enum TipoProcesso { Judicial = 1, Extrajudicial = 2 }

    public class Processo
    {
        public int Id { get; set; }

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
        public DateTime? Data { get; set; }

        [Required]
        [Display(Name = "Tipo do Processo")]
        public TipoProcesso TipoProcesso { get; set; }

        [NotMapped]
        public IFormFile FormFile { get; set; }

        public byte[] Documento { get; set; }
        public string ExtensaoDocumento { get; set; }

        public string Observacao { get; set; }

        public bool Sigiloso { get; set; }

        public virtual ICollection<HistoricoProcesso> HistoricoProcesso { get; set; }

        public ICollection<ProcessoParte> ProcessoPartes { get; set; }

        [NotMapped]
        public List<Parte> Partes { get; set; }
    }
}
