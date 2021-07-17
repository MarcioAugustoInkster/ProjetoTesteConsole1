using System;
using System.Collections.Generic;
using System.Globalization;
using ProjetoTeste.Classes;

namespace ProjetoTeste
{
    class Program
    {
        public static List<Empresa> listaEmpresa = new List<Empresa>();
        public static List<Fornecedor> listaFornecedor = new List<Fornecedor>();

        static void Main(string[] args)
        {
            string item = "";

            do
            {
                Console.Clear();
                Console.WriteLine("[ MENU - Teste C# ]");
                Console.WriteLine("1 = Cadastra Empresa");
                Console.WriteLine("2 = Cadastra Fornecedor");
                Console.WriteLine("3 = Listagem");
                Console.WriteLine("0 = Encerrar");
                item = Console.ReadLine();

                Menu(item);
            } while(item != "0");

            Console.WriteLine("Pressione qualquer tecla para encerrar...");
            Console.ReadLine();
        }

        private static void Menu(string item)
        {
            switch(item)
            {
                case "1":
                    CadastraEmpresa();
                    break;
                case "2":
                    CadastraFornecedor();
                    break;
                case "3":
                    CadastroListagem();
                    break;
                case "0":
                    break;
                default:
                    Console.WriteLine("Por favor, informe uma das opções abaixo");
                    break;
            }
        }

        private static void CadastraEmpresa()
        {
            Empresa empresa = new Empresa();
            bool continua = false;

            try
            {
                do
                {
                    do {
                        Console.Clear();
                        Console.WriteLine("Nome fantasia: ");
                        string nome = Console.ReadLine();

                        if (!string.IsNullOrEmpty(nome))
                        {
                            empresa.NomeFantasia = nome;
                            continua = true;
                        }
                    } while (!continua);

                    do {
                        continua = false;
                        Console.Clear();
                        Console.WriteLine("CNPJ: ");
                        string cnpj = Console.ReadLine();

                        if (!string.IsNullOrEmpty(cnpj))
                        {
                            empresa.Cnpj = cnpj;
                            continua = true;
                        }
                    } while (!continua);

                    do {
                        continua = false;
                        Console.Clear();
                        Console.WriteLine("UF: ");

                        string uf = Console.ReadLine();

                        if (!string.IsNullOrEmpty(uf))
                        {
                            if (uf.Length < 2 || uf.Length > 2)
                            {
                                Console.Clear();
                                Console.WriteLine("Campo 'UF' deve conter 2 dígitos");
                            }
                            else
                            {
                                empresa.UF = uf.ToUpper();
                                continua = true;
                            }
                        }
                    } while (!continua);

                    listaEmpresa.Add(empresa);
                    
                    Console.Clear();
                    Console.WriteLine("Cadastrar outra empresa?\n1 = Sim\n0 = Não");
                    string menu = Console.ReadLine();

                    if (menu.Equals("1") || menu.Equals(1))
                        continua = true;
                    else
                        continua = false;
                } while (continua);

                Console.WriteLine("[ " + listaEmpresa.Count + " Empresas Cadastradas ]");

                foreach (var emp in listaEmpresa)
                {
                    Console.WriteLine("Nome fantasia: " + emp.NomeFantasia);
                    Console.WriteLine("CNPJ: " + emp.Cnpj);
                    Console.WriteLine("UF: " + emp.UF);
                    Console.WriteLine("==========");
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        private static void CadastraFornecedor()
        {
            if (listaEmpresa.Count > 0)
            {
                Fornecedor fornecedor = new Fornecedor();

                try
                {
                    bool continua = false;

                    do {
                        Console.Clear();
                        continua = true;
                        
                        for (int i = 0; i < listaEmpresa.Count; i++)
                            Console.WriteLine(i + " = " + listaEmpresa[i].NomeFantasia);

                        Console.WriteLine("Escolha a Empresa acima:");
                        string emps = Console.ReadLine();

                        if (Int32.Parse(emps) >= 0 && Int32.Parse(emps) <= listaEmpresa.Count)
                        {
                            for (int i = 0; i < listaEmpresa.Count; i++)
                            {
                                if (Int32.Parse(emps) == i)
                                {
                                    fornecedor.Empresa = listaEmpresa[i];
                                    continua = false;
                                }
                            }
                        }
                    } while (continua);

                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Nome do Fornecedor: ");
                        string nome = Console.ReadLine();

                        if (!string.IsNullOrEmpty(nome))
                        {
                            fornecedor.Nome = nome;
                            continua = false;
                        }
                    } while (continua);

                    do
                    {
                        Console.Clear();
                        Console.WriteLine("CPF ou CNPJ: ");
                        string cpfcnpj = Console.ReadLine();

                        if (!string.IsNullOrEmpty(cpfcnpj))
                        {
                            if ((cpfcnpj.Contains(".") && cpfcnpj.Contains("-") && cpfcnpj.Length == 14) ||
                                (cpfcnpj.Contains(".") && cpfcnpj.Contains("-") && 
                                cpfcnpj.Contains("/") && cpfcnpj.Length == 18))
                            {
                                fornecedor.CpfCnpj = cpfcnpj;
                                continua = false;
                            }
                            else
                            {
                                Console.WriteLine("Formato de CPF ou CNPJ incorreto");
                                Console.ReadKey();
                                continua = true;
                            }
                        }
                    } while (continua);

                    bool menorDeIdade = false;
                    DateTime fdataNascimento = new DateTime();
                    
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Informe seu ano de Nascimento:\nFormato: DD/MM/AAAA");
                        string dn = Console.ReadLine();
                        DateTime dataNascimento = DateTime.ParseExact(dn, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (CalculaIdadePorDataNascimento(dataNascimento) < 18)
                        {
                            if ((fornecedor.Empresa.UF.ToUpper().Equals("PR") || fornecedor.Empresa.UF.ToUpper().Equals("PARANÁ")) &&
                                (fornecedor.CpfCnpj.Contains(".") && fornecedor.CpfCnpj.Contains("-") && fornecedor.CpfCnpj.Length == 14))
                            {
                                Console.Clear();
                                Console.WriteLine("Aviso: Se a empresa é do Paraná, este Fornecedor deve:");
                                Console.WriteLine("- Ser +18 anos\n- Informar o CPF");
                                menorDeIdade = true;
                                fdataNascimento = dataNascimento;
                                continua = false;
                            }
                        }

                        if (!menorDeIdade)
                        {
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("Informe seu R.G.");
                                string frg = Console.ReadLine();

                                if (string.IsNullOrEmpty(frg))
                                    continua = true;
                                else
                                {
                                    fornecedor.RG = frg;
                                    continua = false;
                                }
                            } while (continua);

                            Console.Clear();
                            Console.WriteLine("Informe sua Data de Nascimento.\nFormato: DD/MM/AAAA");
                            fornecedor.DataNascimento = fdataNascimento;
                            
                            Console.Clear();
                            Console.WriteLine("Cadastrar um número de telefone?");
                            Console.WriteLine("(S/N)");
                            string valt = Console.ReadLine();

                            if (valt.Equals("s") || valt.Equals("S"))
                            {
                                string telefone1 = Console.ReadLine();
                                fornecedor.Telefone = telefone1;
                            }

                            fornecedor.DataCadastro = DateTime.Now;
                            listaFornecedor.Add(fornecedor);

                            Console.WriteLine("Cadastro de Fornecedor concluído!");
                            Console.ReadKey();
                        }
                    } while (continua);

                    Console.WriteLine("O Cadastro não pôde ser finalizado");
                    Console.ReadKey();
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException(ex.Message);
                }
            }
        }

        private static void CadastroListagem()
        {
            Console.Clear();

            if (listaEmpresa.Count > 0 || listaFornecedor.Count > 0)
            {
                Console.WriteLine("1 = Lista Empresas");
                Console.WriteLine("2 = Lista Fornecedores");

                int listagem = Convert.ToInt32(Console.ReadLine());

                if (listagem == 1)
                {
                    for (int i = 0; i < listaEmpresa.Count; i++)
                    {
                        Console.WriteLine(i + " = " + listaEmpresa[i].NomeFantasia + " / " + listaEmpresa[i].Cnpj + " / " + listaEmpresa[i].UF);
                    }
                    Console.WriteLine("Qualquer tecla para voltar...");
                    Console.ReadKey();
                }
                else if (listagem == 2)
                {
                    bool continua = false;
                    int contador = 0;

                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Informe abaixo:\nNome, CPF/CNPJ ou Data Cadastro\npara pesquisar Fornecedor:");
                        string pesquisa = Console.ReadLine();
                        Console.Clear();

                        foreach (Fornecedor forn in listaFornecedor)
                        {
                            if (forn.Nome.ToLower().Contains(pesquisa.ToLower()) || 
                                forn.CpfCnpj.Equals(pesquisa) || 
                                forn.DataCadastro.ToString().Contains(pesquisa))
                            {
                                Console.WriteLine("Fornecedor: " + forn.Nome);
                                Console.WriteLine("Empresa: " + forn.Empresa.NomeFantasia);
                                Console.WriteLine("CPF/CNPJ: " + forn.CpfCnpj);
                                Console.WriteLine("RG: " + forn.RG);
                                Console.WriteLine("Telefone: " + forn.Telefone);
                                Console.WriteLine("Data Cadastro: " + forn.DataCadastro);
                                Console.WriteLine("==== // ==== // ====");

                                contador++;
                            }
                        }

                        if (contador > 0)
                        {
                            Console.WriteLine("Foram encontrados {0}", contador + " resultados");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Nenhum resultado encontrado.");
                        }

                        Console.Clear();
                        contador = 0;
                        Console.WriteLine("Pesquisar novamente?\n1 = Sim\n0 = Não");
                        string resp = Console.ReadLine();
                        Console.Clear();

                        if (resp.Equals("1") || resp.Equals(1))
                            continua = true;
                        else
                            continua = false;
                    } while (continua);
                }
            }
            else
            {
                Console.WriteLine("Nenhum cadastro encontrado");
                Console.ReadKey();
            }
        }

        private static int CalculaIdadePorDataNascimento(DateTime dataNascimento)
        {
            int idade = DateTime.Now.Year - dataNascimento.Year;

            if (DateTime.Now.DayOfYear < dataNascimento.DayOfYear)
                idade = idade - 1;
            
            return idade;
        }
    }
}
