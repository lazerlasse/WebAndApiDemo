using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAndApiDemo.Models
{
	public class NewsCategory
	{
		[Key]
		public int CategoryID { get; set; }

		[Required, Display(Name = "Navn")]
		public string CategoryName { get; set; }

		// Navigation properties.
		[Display(Name = "Nyheder i denne kategori")]
		public ICollection<NewsCategoryAssignment> NewsCategoryAssignments { get; set; }
	}
}