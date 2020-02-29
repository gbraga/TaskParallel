using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Parallel
{
      class Program
      {
            static void Main(string[] args)
            {
                  Console.WriteLine("Teste com Threads...");

                  // ChamadaSincrona();
                  // ChamadaAssincrona();
                  // ChamadaComLacosParallel();
                  // ChamadaComLacosParallelAninhados();
                  ChamadaComLacosParallelAninhadosII();
            }

            // Aqui aninhei paralelo para loops.O corpo do loop interno controla quantas outras
            // iterações desse loop estão em execução no momento e
            // gera a média do loop para o console.
            private static void ChamadaComLacosParallelAninhadosII()
            {
                  const int OUTER_ITERS = 2000;
                  const int INNER_ITERS = 2000;

                  Parallel.For(0, OUTER_ITERS, i =>
                  {
                        int innerConcurrencyLevel = 0;
                        int innerConcurrencyLevelSum = 0;

                        Parallel.For(0, INNER_ITERS, j =>
                        {
                              Interlocked.Increment(ref innerConcurrencyLevel);
                              for (int spin = 0; spin < 50000; spin++) ;
                              Interlocked.Add(ref innerConcurrencyLevelSum, Volatile.Read(ref innerConcurrencyLevel));
                              for (int spin = 0; spin < 50000; spin++) ;
                              Interlocked.Decrement(ref innerConcurrencyLevel);
                        });

                        Console.Write("{0},", Math.Round(innerConcurrencyLevelSum / (double)INNER_ITERS));
                  });
            }

            private static void ChamadaComLacosParallelAninhados()
            {
                  Console.WriteLine();
                  Console.WriteLine("Chamada Com Laços Parallel Aninhados");

                  Stopwatch stopwatch = new Stopwatch();
                  stopwatch.Start();

                  Parallel.For(0,300, (j) =>
                  {
                        ImprimirUm();
                        ImprimirZeroAsync();
                        ImprimirPontoAsync();
                        Console.Write("|");
                  });

                  stopwatch.Stop();

                  Console.WriteLine($"A tarefa foi finalizada {stopwatch.ElapsedMilliseconds / 1000.0}.");
            }

            private static void ChamadaComLacosParallel()
            {
                  Console.WriteLine();
                  Console.WriteLine("Chamada Com Laços Parallel");

                  Stopwatch stopwatch = new Stopwatch();
                  stopwatch.Start();

                  ImprimirZeroAsync();
                  ImprimirUmAsync();
                  ImprimirPontoAsync();

                  stopwatch.Stop();

                  Console.WriteLine($"A tarefa foi finalizada {stopwatch.ElapsedMilliseconds / 1000.0}.");
            }

            private static void ChamadaAssincrona()
            {
                  Console.WriteLine();
                  Console.WriteLine("Chamada Assincrona");

                  Stopwatch stopwatch = new Stopwatch();
                  stopwatch.Start();

                  Parallel.Invoke(
                        () => ImprimirZero(),
                        () => ImprimirUm(),
                        () => ImprimirPonto());

                  stopwatch.Stop();

                  Console.WriteLine($"A tarefa foi finalizada {stopwatch.ElapsedMilliseconds / 1000.0}.");
            }

            private static void ChamadaSincrona()
            {
                  Console.WriteLine();
                  Console.WriteLine("Chamada Sincrona");

                  Stopwatch stopwatch = new Stopwatch();
                  stopwatch.Start();

                  ImprimirZero();
                  ImprimirUm();
                  ImprimirPonto();

                  stopwatch.Stop();

                  Console.WriteLine($"A tarefa foi finalizada {stopwatch.ElapsedMilliseconds / 1000.0}.");
            }

            private static void ImprimirZero()
            {
                  for (int i = 0; i < 300; i++)
                  {
                        ExecutarTarefaDemorada();
                        Console.Write("0");
                  }
            }

            private static void ImprimirZeroAsync()
            {
                  Parallel.For (0,300, (i) =>
                  {
                        ExecutarTarefaDemorada();
                        Console.Write("0");
                  });

                  // Exemplo de ForEach
                  // var lista = Enumerable.Range(0, 300);
                  // Parallel.ForEach(lista, (item) =>
                  // {
                  //      ExecutarTarefaDemorada();
                  //      Console.Write("0");
                  // });
            }

            private static void ImprimirUm()
            {
                  for (int i = 0; i < 300; i++)
                  {
                        ExecutarTarefaDemorada();
                        Console.Write("1");
                  }
            }

            private static void ImprimirUmAsync()
            {
                  Parallel.For(0, 300, (i) =>
                  {
                        ExecutarTarefaDemorada();
                        Console.Write("1");
                  });

                  // Exemplo de Break/Stop
                  // Parallel.For(0, 300, (int i, ParallelLoopState state) =>
                  // {
                  //       if (i > 100)
                  //       {
                  //             Console.WriteLine($"O loop foi parado na posição {i}.");
                  //             state.Break(); // ou state.Stop();
                  //       }
                  //       ExecutarTarefaDemorada();
                  //       Console.Write("1");
                  // });
            }

            private static void ImprimirPonto()
            {
                  for (int i = 0; i < 300; i++)
                  {
                        ExecutarTarefaDemorada();
                        Console.Write(".");
                  }
            }

            private static void ImprimirPontoAsync()
            {
                  Parallel.For(0, 300, (i) =>
                  {
                        ExecutarTarefaDemorada();
                        Console.Write(".");
                  });

                  // Exemplo de ParallelResult
                  // ParallelResult result = Parallel.For(0, 300, (int i, ParallelLoopState state) =>
                  // {
                  //       if (i > 100)
                  //       {
                  //             Console.WriteLine($"O loop foi parado na posição {i}.");
                  //             state.Break(); // ou state.Stop();
                  //       }
                  //       ExecutarTarefaDemorada();
                  //       Console.Write("1");
                  // });
                  //
                  // Console.WriteLine($"Qual foi o maior valor executado (o index)? {result.LowestBreakInteration}"); // Só em casos em que o loop foi interrompido COM BREAK.
                  // Console.WriteLine($"O loop foi completado? {result.IsCompleted}");

            }

            private static void ExecutarTarefaDemorada()
            {
                  Thread.Sleep(10);
            }
      }
}
