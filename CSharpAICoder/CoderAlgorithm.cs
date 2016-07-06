using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAICoder
{
	public class CoderAlgorithm
	{
		public MethodInfo[] PossibleMethods { get; set; }

		Random r = new Random();

		public int IterationCount { get; set; }
		public int PopulationCount { get; set; } = 10;

		public string Goal { get; private set; }

		public CoderAlgorithm(IEnumerable<MethodInfo> possibleValues, string goal, int iterations = 1000)
		{
			PossibleMethods = possibleValues.ToArray();
			IterationCount = iterations;
			Goal = goal;
		}

		public double Fitness(CodeWrap cw)
		{
			try
			{
				var app = new App(Guid.NewGuid().ToString(), cw.Code);
				cw.Output = app.Run();
			}
			catch(Exception ex) { }

			double score = 0;
			for (int i = 0; i < Goal.Length && i < cw.Output?.Length; i++)
				if (cw.Output[i].CompareTo(Goal[i]) == 0) score++;
			return score / Goal.Length;
		}

		public List<MethodInfo> Generate(int length)
		{
			List<MethodInfo> methods = new List<MethodInfo>();
			for (int i = 0; i < length; i++)
			{
				var m = PossibleMethods[r.Next(0, PossibleMethods.Length)];
				foreach (var pi in m.GetParameters())
				{
					
				}
				methods.Add(m);
			}
			return methods;
		}

		public List<MethodInfo> Mutate(List<MethodInfo> chromosome, double probability)
		{
			List<MethodInfo> s = new List<MethodInfo>();
			for (int i = 0; i < chromosome.Count; i++)
			{
				if (r.NextDouble() < probability)
				{
					MethodInfo[] others = PossibleMethods.Where(p => p.Name.CompareTo(chromosome[i].Name) != 0).ToArray();
					s.Add(others[r.Next(0, others.Length)]);
				}
				else
					s.Add(chromosome[i]);
			}

			return s;
		}

		public List<MethodInfo>[] Crossover(List<MethodInfo> chromosome1, List<MethodInfo> chromosome2, double p_c)
		{
			if (r.NextDouble() < p_c)
			{
				int cut = r.Next(1, Math.Min(chromosome1.Count, chromosome2.Count));
				return new List<MethodInfo>[]
				{
					new List<MethodInfo>(chromosome1.Take(cut).Concat(chromosome2.Skip(cut))),
					new List<MethodInfo>(chromosome2.Take(cut).Concat(chromosome1.Skip(cut)))
				};
			}
			else
				return new List<MethodInfo>[] { chromosome1, chromosome2 };
		}

		public List<CodeWrap> MapPopulationFit(IEnumerable<List<MethodInfo>> population, Func<CodeWrap, double> fitness)
		{
			var fitnesses = new List<CodeWrap>();
			foreach (var p in population)
			{
				var cw = new CodeWrap() { Methods = p };
				cw.WriteCode();
				cw.Fitness = fitness.Invoke(cw);
				fitnesses.Add(cw);
			}
			return fitnesses;
		}

		public int Select(IEnumerable<CodeWrap> fitnesses)
		{
			double fitSum = fitnesses.Sum(f => f.Fitness);
			double value = r.NextDouble() * fitSum;
			int i = 0;
			foreach (var f in fitnesses)
			{
				value -= f.Fitness;
				if (value <= 0) return i;
				i++;
			}

			return i - 1;
		}

		public CodeWrap Run(double p_c, double p_m)
		{

			List<MethodInfo>[] population = new List<MethodInfo>[PopulationCount];
			List<MethodInfo>[] newPoppulation = new List<MethodInfo>[population.Length];
			IEnumerable<CodeWrap> fitnesses;
			List<MethodInfo>[] luckyFew = new List<MethodInfo>[2];
			int chromosomeLength = 3;

			for (int i = 0; i < population.Length; i++) population[i] = Generate(chromosomeLength);

			for (int iteration = 0; iteration < IterationCount; iteration++)
			{
				fitnesses = MapPopulationFit(population, Fitness);

				if (fitnesses.Any(f => f.Fitness == 1))
				{
					Trace.WriteLine($"Found perfect match!");
					return fitnesses.Where(f => f.Fitness == 1).Select(f => f).First();
				}
				Trace.WriteLine($"{iteration}\t{fitnesses.Average(f => f.Fitness)}");

				for (int iPop = 0; iPop < population.Length; iPop += 2)
				{
					int selected = Select(fitnesses);
					luckyFew[0] = fitnesses.ElementAt(selected).Methods;
					var minusOne = fitnesses.Where((f, fi) => fi != selected);
					luckyFew[1] = minusOne.ElementAt(Select(minusOne)).Methods;


					luckyFew = Crossover(luckyFew[0], luckyFew[1], p_c).ToArray();
					for (int iLuck = 0; iLuck < luckyFew.Length; iLuck++)
						newPoppulation[iPop + iLuck] = Mutate(luckyFew[iLuck], p_m);
				}
				population = newPoppulation;
			}

			fitnesses = MapPopulationFit(population, Fitness);
			Trace.WriteLine($"I have to guess...");
			return fitnesses.OrderByDescending(f => f.Fitness).First();
		}
	}
}
