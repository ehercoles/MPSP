using System;

namespace MPSP
{
    public static class Util
    {
        public static string RemoverMascaraCpf(string cpf)
        {
            return cpf.Replace(".", "").Replace("-", "");
        }

        public static string AdicionarMascaraCpf(string cpf)
        {
            string result = "";

            if (!string.IsNullOrEmpty(cpf))
            {
                result = UInt64.Parse(cpf).ToString(@"000\.000\.000\-00");
            }

            return result;
        }
    }
}
