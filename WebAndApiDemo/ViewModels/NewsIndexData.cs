using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAndApiDemo.Models;

namespace WebAndApiDemo.ViewModels
{
	public class NewsIndexData
	{
		public IEnumerable<News> News { get; set; }
		public IEnumerable<NewsCategory> NewsCategories { get; set; }
	}
}
