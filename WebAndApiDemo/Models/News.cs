using Microsoft.AspNetCore.Mvc.ModelBinding;
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
		[Key, Display(Name = "Nyheds ID")]
		public int ID { get; set; }

		[Required, Display(Name = "Overskrift")]
		public string Titel { get; set; }

		[Required, Display(Name = "Indhold")]
		public string Content { get; set; }

		[DataType(DataType.DateTime), Display(Name = "Udgivet")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:MM}", ApplyFormatInEditMode = true)]
		public DateTime Published { get; set; }

		// Navigation properties.
		public ICollection<NewsCategoryAssignment> NewsCategoryAssignments { get; set; }
	}
}
