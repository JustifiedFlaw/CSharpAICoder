using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAICoder
{
	class GeneticAlgorithm<T> where T : IComparable
	{
		public T[] PossibleValues { get; set; }

		Random r = new Random();

		public class ChromosomeWrap
		{
			public List<T> Chromosome { get; set; }
			public double Fitness { get; set; }
		}

		public int ChromosomeLength { get; set; }
		public int IterationCount { get; set; }
		public int PopulationCount { get; set; } = 50;

		public List<T> Goal { get; private set; }

		public GeneticAlgorithm(IEnumerable<T> possibleValues, int chromosoneLength = 20, int iterations = 1000)
		{
			PossibleValues = possibleValues.ToArray();
			ChromosomeLength = chromosoneLength;
			IterationCount = iterations;
			Goal = Generate(ChromosomeLength);
		}

		public GeneticAlgorithm(IEnumerable<T> possibleValues, List<T> goal, int iterations = 1000)
		{
			PossibleValues = possibleValues.ToArray();
			ChromosomeLength = goal.Count;
			IterationCount = iterations;
			Goal = goal;
		}

		public double Fitness(List<T> p)
		{
			double score = 0;
			for (int i = 0; i < Goal.Count; i++)
				if (p[i].CompareTo(Goal[i]) == 0) score++;
			return score / Goal.Count;
		}

		public List<T> Generate(int length)
		{
			List<T> chromosome = new List<T>();
			for (int i = 0; i < length; i++) chromosome.Add(PossibleValues[r.Next(0, PossibleValues.Length)]);
			return chromosome;
		}

		public List<T> Mutate(List<T> chromosome, double probability)
		{
			List<T> s = new List<T>();
			for (int i = 0; i < chromosome.Count; i++)
			{
				if (r.NextDouble() < probability)
				{
					T[] others = PossibleValues.Where(p => p.CompareTo(chromosome[i]) != 0).ToArray();
					s.Add(others[r.Next(0, others.Length)]);
				}
				else
					s.Add(chromosome[i]);
			}

			return s;
		}

		public List<T>[] Crossover(List<T> chromosome1, List<T> chromosome2, double p_c)
		{
			if (r.NextDouble() < p_c)
			{
				int cut = r.Next(1, ChromosomeLength);
				return new List<T>[]
				{
					new List<T>(chromosome1.Take(cut).Concat(chromosome2.Skip(cut))),
					new List<T>(chromosome2.Take(cut).Concat(chromosome1.Skip(cut)))
				};
			}
			else
				return new List<T>[] { chromosome1, chromosome2 };
		}

		public List<ChromosomeWrap> MapPopulationFit(IEnumerable<List<T>> population, Func<List<T>, double> fitness)
		{
			return population.Select(p => new ChromosomeWrap()
			{
				Chromosome = p,
				Fitness = fitness.Invoke(p)
			}).ToList();
		}

		public int Select(IEnumerable<ChromosomeWrap> fitnesses)
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

		public List<T> Run(double p_c, double p_m)
		{

			List<T>[] population = new List<T>[PopulationCount];
			List<T>[] newPoppulation = new List<T>[population.Length];
			IEnumerable<ChromosomeWrap> fitnesses;
			List<T>[] luckyFew = new List<T>[2];

			for (int i = 0; i < population.Length; i++) population[i] = Generate(ChromosomeLength);

			for (int iteration = 0; iteration < IterationCount; iteration++)
			{
				fitnesses = MapPopulationFit(population, Fitness);

				if (fitnesses.Any(f => f.Fitness == 1))
				{
					Trace.WriteLine($"Found perfect match!");
					return fitnesses.Where(f => f.Fitness == 1).Select(f => f.Chromosome).First();
				}
				Trace.WriteLine($"{iteration}\t{fitnesses.Average(f => f.Fitness)}");

				for (int iPop = 0; iPop < population.Length; iPop += 2)
				{
					int selected = Select(fitnesses);
					luckyFew[0] = fitnesses.ElementAt(selected).Chromosome;
					var minusOne = fitnesses.Where((f, fi) => fi != selected);
					luckyFew[1] = minusOne.ElementAt(Select(minusOne)).Chromosome;


					luckyFew = Crossover(luckyFew[0], luckyFew[1], p_c).ToArray();
					for (int iLuck = 0; iLuck < luckyFew.Length; iLuck++)
						newPoppulation[iPop + iLuck] = Mutate(luckyFew[iLuck], p_m);
				}

				//int selected = Select(fitnesses);
				//luckyFew[0] = fitnesses.ElementAt(selected).Chromosome;
				//var minusOne = fitnesses.Where((f, fi) => fi != selected);
				//luckyFew[1] = minusOne.ElementAt(Select(minusOne)).Chromosome;

				//for (int iPop = 0; iPop < population.Length; iPop ++)
				//{
				//	luckyFew = Crossover(luckyFew[0], luckyFew[1], p_c).ToArray();
				//	newPoppulation[iPop] = Mutate(luckyFew[0], p_m);
				//}
				population = newPoppulation;
			}

			fitnesses = MapPopulationFit(population, Fitness);
			Trace.WriteLine($"I have to guess...");
			return fitnesses.OrderByDescending(f => f.Fitness).Select(f => f.Chromosome).First();
		}
	}
}
