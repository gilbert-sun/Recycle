using Recycle.Models;
using Recycle.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Recycle.UserControls
{
	public class TaskCard : Control
	{
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
			name: "Header",
			propertyType: typeof(string),
			ownerType: typeof(TaskCard));

		public static readonly DependencyProperty ClassItemsProperty = DependencyProperty.Register(
			name: "ClassItems",
			propertyType: typeof(IEnumerable<ClassData>),
			ownerType: typeof(TaskCard));

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
			name: "SelectedItem",
			propertyType: typeof(ClassData),
			ownerType: typeof(TaskCard),
			typeMetadata: new PropertyMetadata(OnSelectedItemChanged));

		public static readonly DependencyProperty AccumulationProperty = DependencyProperty.Register(
			name: "Accumulation",
			propertyType: typeof(string),
			ownerType: typeof(TaskCard));

		public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
			name: "Icon",
			propertyType: typeof(Geometry),
			ownerType: typeof(TaskCard));

		public static readonly DependencyProperty PercentProperty = DependencyProperty.Register(
			name: "Percent",
			propertyType: typeof(string),
			ownerType: typeof(TaskCard)); 
		
		public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
			 name: "Status",
			 propertyType: typeof(ComponentStatus),
			 ownerType: typeof(TaskCard));

		private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			(sender as TaskCard)?.UpdateSelected();
		}

		public string Header
		{
			get => GetValue(HeaderProperty) as string;
			set => SetValue(HeaderProperty, value);
		}

		public IEnumerable<ClassData> ClassItems
		{
			get => GetValue(ClassItemsProperty) as IEnumerable<ClassData>;
			set => SetValue(ClassItemsProperty, value);
		}

		public ClassData SelectedItem
		{
			get => (ClassData)GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		public string Accumulation
		{
			get => GetValue(AccumulationProperty) as string;
			set => SetValue(AccumulationProperty, value);
		}

		public Geometry Icon
		{
			get => GetValue(IconProperty) as Geometry;
			set => SetValue(IconProperty, value);
		}

		public string Percent
		{
			get => GetValue(PercentProperty) as string;
			set => SetValue(PercentProperty, value);
		}

		public ComponentStatus Status
		{
			get => (ComponentStatus)GetValue(StatusProperty);
			set => SetValue(StatusProperty, value);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			comboBox = GetTemplateChild("PART_ComboBox") as ComboBox;
			if (comboBox != null)
			{
				comboBox.SelectionChanged += ComboBox_SelectionChanged;
			}
			UpdateSelected();
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedItem = (ClassData)comboBox.SelectedItem;
		}

		private ComboBox comboBox;

		private void UpdateSelected()
		{
			if (comboBox == null)
			{
				return;
			}
			comboBox.SelectedItem = SelectedItem;
		}
	}
}
