using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAndApiDemo.Models.ViewModels
{
	public class ChosenNewsCategoryData
	{
		public int CategoryID { get; set; }
		public string Titel { get; set; }
		public bool IsChecked { get; set; }
	}
}
