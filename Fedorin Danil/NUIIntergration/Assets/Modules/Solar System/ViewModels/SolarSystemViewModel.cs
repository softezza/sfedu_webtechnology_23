#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using UnityEngine;
#else
using System.Collections.Generic;
using System.Windows.Media.Imaging;
#endif

using System;
using SmartTwin.NoesisGUI.Views;
using System.Collections.ObjectModel;
using System.Resources;

namespace DataBinding.ViewModels
{
	public class SolarSystemViewModel : BaseViewModel
	{
		private readonly ObservableCollection<SpaceBody> _bodies;

		public SolarSystemViewModel()
		{
#if NOESIS

			_bodies = new ObservableCollection<SpaceBody>()
			{
				new SpaceBody()
				{
					Name = "Sun",
					Orbit = 0,
					Diameter = 1380000,
					Details = "The yellow dwarf star in the center of our solar system.",
					Source = new TextureSource((Texture2D)Resources.Load("sun"))
				},
				new SpaceBody
				{
					Name = "Mercury",
					Orbit = 0.38f,
					Diameter = 4880,
					Details = "The small and rocky planet Mercury is the closest planet to the Sun.",
					Source = new TextureSource((Texture2D)Resources.Load("merglobe"))
				},
				new SpaceBody
				{
					Name = "Venus",
					Orbit = 0.72f,
					Diameter = 12103.6f,
					Details = "At first glance, if Earth had a twin, it would be Venus.",
					Source = new TextureSource((Texture2D)Resources.Load("venglobe"))
				},
				new SpaceBody
				{
					Name = "Earth",
					Orbit = 1,
					Diameter = 12756.3f,
					Details = "Earth, our home planet, is the only planet in our solar system known to harbor life.",
					Source = new TextureSource((Texture2D)Resources.Load("earglobe"))
				},
				new SpaceBody
				{
					Name = "Mars",
					Orbit = 1.52f,
					Diameter = 6794,
					Details = "The red planet Mars has inspired wild flights of imagination over the centuries.",
					Source =  new TextureSource((Texture2D)Resources.Load("marglobe"))
				},
				new SpaceBody
				{
					Name = "Jupiter",
					Orbit = 5.20f,
					Diameter = 142984,
					Details = "With its numerous moons and several rings, the Jupiter system is a \"mini-solar system\".",
					Source = new TextureSource((Texture2D)Resources.Load("jupglobe"))
				},
				new SpaceBody
				{
					Name = "Saturn",
					Orbit = 9.54f,
					Diameter = 120536,
					Details = "Saturn is the most distant of the five planets known to ancient stargazers.",
					Source = new TextureSource((Texture2D)Resources.Load("moons_2"))
				},
				new SpaceBody
				{
					Name = "Uranus",
					Orbit = 19.218f,
					Diameter = 51118,
					Details = "Uranus gets its blue-green color from methane gas above the deeper cloud layers.",
					Source =  new TextureSource((Texture2D)Resources.Load("uraglobe"))
				},
				new SpaceBody
				{
					Name = "Neptune",
					Orbit = 30.06f,
					Diameter = 49532,
					Details = "Neptune was the first planet located through mathematical predictions.",
					Source = new TextureSource((Texture2D)Resources.Load("nepglobe"))
				},
				new SpaceBody
				{
					Name = "Pluto",
					Orbit = 39.5f,
					Diameter = 2274,
					Details = "Long considered to be the smallest, coldest, and most distant planet from the Sun.",
					Source = new TextureSource((Texture2D)Resources.Load("plutoch_2"))
				}
			};

#endif

			Bodies = new ReadOnlyObservableCollection<SpaceBody>(_bodies);
		}


		public ReadOnlyObservableCollection<SpaceBody> Bodies { get; }
	}

	public class SpaceBody : BaseViewModel
	{	
		public string Name { get; set; }

		public float Orbit { get; set; }

		public float Diameter { get; set; }

		public string Details { get; set; }

#if NOESIS
		public TextureSource Source { get; set; }
#endif
	}
}
