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
	public interface IReadOnlyWorldEntryRepository
	{
		/// <summary>
		/// Indicates if a world entry with the provided <see cref="worldId"/> exists.
		/// </summary>
		/// <param name="worldId">The id of the world to check for.</param>
		/// <returns>True if there is a world with the provided id.</returns>
		Task<bool> HasEntry(int worldId);

		/// <summary>
		/// Gets the <see cref="WorldEntryModel"/> if it exists with the
		/// provided key <see cref="worldId"/>.
		/// </summary>
		/// <param name="worldId">The entry id for the world.</param>
		/// <returns>The model that details the world.</returns>
		Task<WorldEntryModel> GetWorldEntry(int worldId);
	}
}
