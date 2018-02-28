using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVindictive
{
	/// <summary>
	/// 
	/// </summary>
	public interface IWorldEntryRepository : IReadOnlyWorldEntryRepository
	{
		/// <summary>
		/// Adds a new <see cref="WorldEntryModel"/> to the database associated with
		/// the provided <see cref="accountId"/>.
		/// </summary>
		/// <param name="accountId">The ID to associate with the world.</param>
		/// <param name="creationIp">The original world creation IP.</param>
		/// <param name="worldGuid">The guid of the world./was</param>
		/// <returns></returns>
		Task<WorldEntryModel> AddWorldEntry(int accountId, string creationIp, Guid worldGuid);
	}
}
