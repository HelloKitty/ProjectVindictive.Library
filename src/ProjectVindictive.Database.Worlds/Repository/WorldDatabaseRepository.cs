using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ProjectVindictive
{
	public sealed class WorldDatabaseRepository : IWorldEntryRepository
	{
		/// <summary>
		/// The database context for the world database.
		/// </summary>
		private WorldDatabaseContext Context { get; }

		public WorldDatabaseRepository(WorldDatabaseContext context)
		{
			if(context == null) throw new ArgumentNullException(nameof(context));

			Context = context;
		}

		/// <inheritdoc />
		public async Task<WorldEntryModel> AddWorldEntry(int accountId, string creationIp, Guid worldGuid)
		{
			if(accountId <= 0) throw new ArgumentOutOfRangeException(nameof(accountId));
			if(string.IsNullOrWhiteSpace(creationIp)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(creationIp));

			//We don't have a storage key at this point but the Database will generate it
			EntityEntry<WorldEntryModel> entry = await Context.WorldEntries.AddAsync(new WorldEntryModel(accountId, creationIp, worldGuid));

			await Context.SaveChangesAsync();

			//Return the model so that it can be accessed by caller. They'll likely need access to the GUID.
			return entry.Entity;
		}

		/// <inheritdoc />
		public async Task<bool> HasEntry(int worldId)
		{
			if(worldId < 0) throw new ArgumentOutOfRangeException(nameof(worldId));

			//TODO: Convert API to use long key. Or abstract key type.
			//Use find so it can be cached because likely to be called GetWorldEntry
			return null != (await Context.WorldEntries.FindAsync((long)worldId));
		}

		/// <inheritdoc />
		public async Task<WorldEntryModel> GetWorldEntry(int worldId)
		{
			if(!await HasEntry(worldId))
				throw new InvalidOperationException($"Entry {worldId} is not in the world entry.");

			//TODO: Convert API to use long key. Or abstract key type.
			//We use find because it's likely cached
			return await Context.WorldEntries.FindAsync((long)worldId);
		}
	}
}
