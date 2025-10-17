using System.Linq.Expressions;

namespace ExerciseLar.Infrastructure.Common
{
	public class DataRequest<T>
	{
		public string Query { get; set; } = string.Empty;

		public Expression<Func<T, bool>>? Where { get; set; }
		public List<(Expression<Func<T, object>> KeySelector, bool Desc)> OrderBys { get; set; } = [];
	}
}
