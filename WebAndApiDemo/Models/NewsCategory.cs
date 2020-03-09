using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAndApiDemo.Models
{
	public class NewsCategory
	{
		public int ID { get; set; }

		[Required]
		public string CategorName { get; set; }

		// Navigation properties.
		public ICollection<NewsCategoryAssignment> NewsCategoryAssignments { get; set; }
	}
}