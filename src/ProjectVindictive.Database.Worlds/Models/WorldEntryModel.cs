﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectVindictive
{
	/// <summary>
	/// Database table model for the world entries.
	/// </summary>
	[Table("world_entry")]
	public class WorldEntryModel
	{
		/// <summary>
		/// The primary unique 64bit integer key used for the
		/// world's unique ID.
		/// </summary>
		[Key]
		[Range(0, long.MaxValue)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long WorldId { get; private set; }

		//TODO: Should we like tables somehow with auth? Probably not?
		/// <summary>
		/// Key for the account associated with the creation/registeration of this world.
		/// </summary>
		[Required]
		[Range(0, int.MaxValue)]
		public int AccountId { get; private set; }

		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreationTime { get; private set; }

		[Required]
		[StringLength(15, MinimumLength = 7)] //IP constraints
		public string CreationIp { get; private set; }

		//TODO: Add public/private/uploading/uploaded state field
		/// <summary>
		/// The GUID name of the stored world.
		/// </summary>
		[Required]
		public Guid StorageGuid { get; private set; }

		public WorldEntryModel(int accountId, string creationIp, Guid storageGuid)
		{
			if(string.IsNullOrWhiteSpace(creationIp)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(creationIp));
			if(accountId <= 0) throw new ArgumentOutOfRangeException(nameof(accountId));
			if(storageGuid == Guid.Empty) throw new ArgumentOutOfRangeException(nameof(storageGuid));

			//TODO: Should we validate it's an IpAddress?
			AccountId = accountId;
			CreationIp = creationIp;
			StorageGuid = storageGuid;
		}

		protected WorldEntryModel()
		{
			
		}
	}
}
