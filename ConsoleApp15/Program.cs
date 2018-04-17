using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.CodeGeneration;

//что то меняешь 
//саня
//S

namespace ConsoleApp15
{
	class Program
	{
		public const int MIN = 1, MAX = 100, AnimalCount = 5, MainIteration = 10, genCount = 50;
		static StreamWriter W = new StreamWriter("output.txt");
		static StreamReader R = new StreamReader("input.txt");
		static Random random = new Random();

		static Dictionary<int, string> dict = new Dictionary<int, string>();

		static Stopwatch stopWatch = new Stopwatch();

		static public Animal[] animal;

		static class Player
		{
			static int res;
			static public void Play()
			{
				res = random.Next() % 100;
				for (int i = 0; i < AnimalCount; i++)
				{
					try
					{
						var result = CodeGenerator.ExecuteCode<int>(animal[i].BuildProg());
						animal[i].fitness = Math.Abs(res - result);
					}
					catch
					{
						animal[i].fitness = 1000;
					}
				}
			}

			static bool isBigger(int x)
			{
				return x > res;
			}
		}

		public class Animal : IComparable
		{
			public int[] genom = new int[genCount];
			public long fitness;

			public Animal()
			{
				for (int i = 0; i < genCount; i++)
				{
					genom[i] = random.Next() % 19;
				}
			}

			public string BuildProg()
			{
				string p = "";
				for (int i = 0; i < genCount; i++)
				{
					p += dict[genom[i]];
				}
				return p;
			}

			public int CompareTo(object obj)
			{
				return fitness.CompareTo(obj);
			}
		}

		static void Main(string[] args)
		{
			Init();
			animal = new Animal[AnimalCount];
			for (int i = 0; i < AnimalCount; i++)
			{
				animal[i] = new Animal();
			}

			for (int i = 0; i < MainIteration; i++)
			{
				runOneIter();

				Player.Play();
			}
			for (int i = 0; i < genCount; i++)
			{
				Console.Write(animal[0].genom[i]);
			}

			Console.WriteLine("painfull");
			Console.ReadLine();
		}

		public static void runOneIter()
		{
			List<Animal> childs = new List<Animal>();

			int counter = 0;

			for (int i = 0; i < AnimalCount; i++)
			{
				for (int j = 0; j < AnimalCount; j++)
				{
					if (i != j)
					{
						childs.Add(crossing(animal[i], animal[j]));
						counter++;
					}
				}
			}
			childs.Sort();

			for (int i = 0; i < AnimalCount; i++)
			{
				animal[i] = childs[i];
			}

		}

		private static Animal crossing(Animal a1, Animal a2)
		{
			double v1 = a1.fitness;
			double v2 = a2.fitness;
			Animal nAnimal = new Animal();
			double porog = v1 / (v2 + v1);
			for (int i = 0; i < genCount; i++)
			{
				double v = random.NextDouble();

				if (v < porog)
				{
					nAnimal.genom[i] = a1.genom[i];
				}
				else
				{
					nAnimal.genom[i] = a2.genom[i];
				}
			}
			return nAnimal;
		}

		private static void Init()
		{
			dict.Add(0, " ");

			dict.Add(1, "+");
			dict.Add(2, "-");
			dict.Add(3, "*");
			dict.Add(4, "=");

			dict.Add(5, "<");
			dict.Add(6, ">");
			dict.Add(7, "==");

			dict.Add(8, "while(");
			dict.Add(9, "for(");

			dict.Add(10, ")");

			dict.Add(11, "return");

			dict.Add(12, ";");

			dict.Add(13, "Player.isBigger(");

			dict.Add(14, "d0");
			dict.Add(15, "d1");


			dict.Add(16, "c1");
			dict.Add(17, "c2");
			dict.Add(18, "c3");
		}
	}
}
