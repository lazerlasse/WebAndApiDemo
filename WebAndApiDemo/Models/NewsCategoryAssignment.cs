using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAndApiDemo.Models
{
	public class NewsCategoryAssignment
	{
		public int ID { get; set; }

		public int NewsID { get; set; }
		public News News { get; set; }

		public int NewsCategoryID { get; set; }
		public NewsCategory NewsCategory { get; set; }
	}
}
