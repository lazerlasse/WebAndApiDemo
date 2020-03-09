using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAndApiDemo.Models
{
	public class News
	{
		[Key]
		public int ID { get; set; }

		[Required]
		public string Titel { get; set; }
		public string Content { get; set; }

		[DisplayFormat(DataFormatString = "{0:/dd/MM/yyyy}", ApplyFormatInEditMode = true), Required]
		public DateTime Published { get; set; }

		// Navigation properties.
		public ICollection<NewsCategoryAssignment> NewsCategoryAssignments { get; set; }
	}
}
