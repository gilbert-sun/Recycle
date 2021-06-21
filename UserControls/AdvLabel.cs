using Recycle.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Recycle.UserControls
{
	public class AdvLabel : StatusLabel
	{
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
			name: "Description",
			propertyType: typeof(object),
			ownerType: typeof(AdvLabel));


		public object Description
		{
			get => GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}
	}
}
