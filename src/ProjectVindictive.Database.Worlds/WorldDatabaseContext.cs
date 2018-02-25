using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ProjectVindictive
{
	public sealed class WorldDatabaseContext : DbContext
	{
		/// <summary>
		/// Set of characters in the database.
		/// </summary>
		public DbSet<WorldEntryModel> WorldEntries { get; set; }

		public WorldDatabaseContext(DbContextOptions<WorldDatabaseContext> options)
			: base(options)
		{

		}

		public WorldDatabaseContext()
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		//TODO: This builds the Database on local test servers. How should we handle this for prod?
#if DEBUG || DEBUGBUILD
		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql("Server=localhost;Database=projectvindictive;Uid=root;Pwd=test;");
		}
#endif
	}
}
