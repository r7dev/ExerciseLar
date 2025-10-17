using ExerciseLar.Infrastructure.DataContexts;

namespace ExerciseLar.Infrastructure.DataServices
{
	public abstract partial class DataServiceBase(ILoanDataSource loanDataSource) : IDataService, IDisposable
	{
		private readonly ILoanDataSource _loanDataSource = loanDataSource;

		#region Dispose
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_loanDataSource?.Dispose();
			}
		}
		#endregion
	}
}
