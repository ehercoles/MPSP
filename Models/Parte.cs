using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPSP.Models
{
    public enum Sexo { Masculino = 1, Feminino = 2 }

    public class Parte
    {
        private string cpf = "";

        public int Id { get; set; }

        [StringLength(14)]
        public string Cpf { get => cpf; set => cpf = Util.RemoverMascaraCpf(value); }

        [NotMapped]
        public string CpfMascarado { get => Util.AdicionarMascaraCpf(cpf); }

        [StringLength(80)]
        public string Nome { get; set; }

        public Sexo Sexo { get; set; }

        public ICollection<ProcessoParte> ProcessoPartes { get; set; }
    }
}
