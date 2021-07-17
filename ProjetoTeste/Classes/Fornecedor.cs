using System;

namespace ProjetoTeste.Classes
{
    public class Fornecedor
    {
        public Empresa Empresa { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string RG { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Telefone { get; set; }
    }
}