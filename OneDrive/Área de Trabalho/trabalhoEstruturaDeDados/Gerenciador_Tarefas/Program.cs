using System;
using System.Collections.Generic;

namespace GerenciadorTarefas
{
    // Classe que representa uma tarefa
    class Tarefa
    {
        public int Id;
        public string Descricao;
        public bool Concluida; // false = pendente, true = concluída
    }

    class Program
    {
        // Lista que guarda todas as tarefas
        static List<Tarefa> tarefas = new List<Tarefa>();

        // Pilha que guarda os IDs das tarefas concluídas (o último concluído fica no topo)
        static Stack<int> pilhaConcluidas = new Stack<int>();

        // Contador para gerar IDs únicos automaticamente
        static int proximoId = 1;

        static void Main()
        {
            int opcao = 0;

            while (opcao != 6)
            {
                MostrarMenu();

                string entrada = Console.ReadLine();

                // Tenta transformar o texto em número. Se não der, é opção inválida.
                if (!int.TryParse(entrada, out opcao))
                {
                    Console.WriteLine("Opção inválida! Digite um número de 1 a 6.");
                    opcao = 0;
                    continue;
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarTarefa();
                        break;
                    case 2:
                        ListarTarefas();
                        break;
                    case 3:
                        BuscarTarefa();
                        break;
                    case 4:
                        ConcluirTarefa();
                        break;
                    case 5:
                        DesfazerConclusao();
                        break;
                    case 6:
                        Console.WriteLine("Encerrando o programa...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida! Digite um número de 1 a 6.");
                        break;
                }
            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine();
            Console.WriteLine("===== GERENCIADOR DE TAREFAS =====");
            Console.WriteLine("1 - Cadastrar tarefa");
            Console.WriteLine("2 - Listar tarefas");
            Console.WriteLine("3 - Buscar tarefa");
            Console.WriteLine("4 - Concluir tarefa");
            Console.WriteLine("5 - Desfazer a última conclusão");
            Console.WriteLine("6 - Sair");
            Console.Write("Escolha uma opção: ");
        }

        static void CadastrarTarefa()
        {
            Console.Write("Digite a descrição da tarefa: ");
            string descricao = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(descricao))
            {
                Console.WriteLine("A descrição não pode ficar vazia.");
                return;
            }

            Tarefa nova = new Tarefa();
            nova.Id = proximoId;
            nova.Descricao = descricao;
            nova.Concluida = false; // começa pendente

            tarefas.Add(nova);
            proximoId++; // o próximo ID será diferente

            Console.WriteLine("Tarefa cadastrada com ID " + nova.Id + ".");
        }

        static void ListarTarefas()
        {
            if (tarefas.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa cadastrada.");
                return;
            }

            foreach (Tarefa t in tarefas)
            {
                MostrarTarefa(t);
            }
        }

        static void BuscarTarefa()
        {
            int id;
            if (!LerId(out id))
            {
                return;
            }

            Tarefa tarefa = BuscarPorId(id);

            if (tarefa == null)
            {
                Console.WriteLine("Nenhuma tarefa encontrada com esse ID.");
                return;
            }

            MostrarTarefa(tarefa);
        }

        static void ConcluirTarefa()
        {
            int id;
            if (!LerId(out id))
            {
                return;
            }

            Tarefa tarefa = BuscarPorId(id);

            if (tarefa == null)
            {
                Console.WriteLine("Nenhuma tarefa encontrada com esse ID.");
                return;
            }

            if (tarefa.Concluida)
            {
                Console.WriteLine("Essa tarefa já está concluída.");
                return;
            }

            tarefa.Concluida = true;
            pilhaConcluidas.Push(tarefa.Id); // coloca o ID no topo da pilha

            Console.WriteLine("Tarefa concluída com sucesso.");
        }

        static void DesfazerConclusao()
        {
            if (pilhaConcluidas.Count == 0)
            {
                Console.WriteLine("Não há nenhuma conclusão para desfazer.");
                return;
            }

            int id = pilhaConcluidas.Pop(); // tira o ID do topo da pilha
            Tarefa tarefa = BuscarPorId(id);
            tarefa.Concluida = false; // volta a ser pendente

            Console.WriteLine("Conclusão desfeita. A tarefa " + tarefa.Id + " voltou a ser pendente.");
        }

        // Pede um ID ao usuário. Retorna false se não for um número inteiro.
        static bool LerId(out int id)
        {
            Console.Write("Digite o ID da tarefa: ");
            string entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out id))
            {
                Console.WriteLine("ID inválido! Digite apenas números inteiros.");
                return false;
            }

            return true;
        }

        // Procura na lista uma tarefa com o ID informado. Retorna null se não achar.
        static Tarefa BuscarPorId(int id)
        {
            foreach (Tarefa t in tarefas)
            {
                if (t.Id == id)
                {
                    return t;
                }
            }
            return null;
        }

        static void MostrarTarefa(Tarefa t)
        {
            string situacao;
            if (t.Concluida)
            {
                situacao = "Concluída";
            }
            else
            {
                situacao = "Pendente";
            }

            Console.WriteLine("ID: " + t.Id + " | Descrição: " + t.Descricao + " | Situação: " + situacao);
        }
    }
}