﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.PixelFilter
{
	/// <summary>
	/// Logic to use when checking.
	/// </summary>
	public enum LogicMode
	{
		/// <summary>
		/// All filter checks need to pass.
		/// </summary>
		And,

		/// <summary>
		/// Only one filter check needs to pass.
		/// </summary>
		Or
	}

	/// <summary>
	/// Filter chain for processors.
	/// </summary>
	[DataContract]
	public sealed class PixelFilterChain
	{
		#region Properties

		/// <summary>
		/// Gets if the filter chain has any active filter.
		/// </summary>
		public bool HasActiveFilter => _activeValueFilter.Count != 0 || _activeIndexFilter.Count != 0 | _activeAutoFilter.Count != 0;

		/// <summary>
		/// Logic used when checking.
		/// </summary>
		[DataMember]
		public LogicMode Mode { get; set; }

		/// <summary>
		/// All configured filters.
		/// </summary>
		public IEnumerable<KeyValuePair<IPixelFilter, bool>> Filters => ValueFilters.Select(kvp => new KeyValuePair<IPixelFilter, bool>(kvp.Key, kvp.Value))
																.Concat(IndexFilters.Select(kvp => new KeyValuePair<IPixelFilter, bool>(kvp.Key, kvp.Value))
																.Concat(AutoFilters.Select(kvp => new KeyValuePair<IPixelFilter, bool>(kvp.Key, kvp.Value))));

		/// <summary>
		/// The configured value filters.
		/// </summary>
		public IReadOnlyList<KeyValuePair<IPixelValueFilter, bool>> ValueFilters => _valueFilters.AsReadOnly();
		[DataMember]
		private readonly List<KeyValuePair<IPixelValueFilter, bool>> _valueFilters = [];

		/// <summary>
		/// The configured index filters.
		/// </summary>
		public IReadOnlyList<KeyValuePair<IPixelIndexFilter, bool>> IndexFilters => _indexFilters.AsReadOnly();
		[DataMember]
		private readonly List<KeyValuePair<IPixelIndexFilter, bool>> _indexFilters = [];

		/// <summary>
		/// The configured auto filters.
		/// </summary>
		public IReadOnlyList<KeyValuePair<IPixelAutoFilter, bool>> AutoFilters => _autoFilters.AsReadOnly();
		[DataMember]
		private readonly List<KeyValuePair<IPixelAutoFilter, bool>> _autoFilters = [];

		#endregion Properties

		#region Member

		private List<IPixelValueFilter> _activeValueFilter = [];
		private List<IPixelIndexFilter> _activeIndexFilter = [];
		private List<IPixelAutoFilter> _activeAutoFilter = [];

		#endregion Member

		/// <summary>
		/// Adds the given <paramref name="filter"/>
		/// to the chain.
		/// </summary>
		/// <param name="filter">Filter to add.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
		/// <exception cref="ArgumentException">When the type of <paramref name="filter"/> is unknown.</exception>
		public void AddPixelFilter(IPixelFilter filter)
		{
			ArgumentNullException.ThrowIfNull(filter);

			if (filter is IPixelValueFilter vf)
				_valueFilters.Add(new KeyValuePair<IPixelValueFilter, bool>(vf, true));
			else if (filter is IPixelIndexFilter inf)
				_indexFilters.Add(new KeyValuePair<IPixelIndexFilter, bool>(inf, true));
			else if (filter is IPixelAutoFilter af)
				_autoFilters.Add(new KeyValuePair<IPixelAutoFilter, bool>(af, true));
			else
				throw new ArgumentException("Unknown pixel filter type", nameof(filter));

			RebuildActiveFilters();
		}

		/// <summary>
		/// Removes the given <paramref name="filter"/>
		/// from the chain.
		/// </summary>
		/// <param name="filter">Filter to add.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
		/// <exception cref="ArgumentException">When the type of <paramref name="filter"/> is unknown.</exception>
		public void RemovePixelFilter(IPixelFilter filter)
		{
			ArgumentNullException.ThrowIfNull(filter);

			if (filter is IPixelValueFilter vf)
				_valueFilters.Remove(_valueFilters.First(kvp => kvp.Key == vf));
			else if (filter is IPixelIndexFilter inf)
				_indexFilters.Remove(_indexFilters.First(kvp => kvp.Key == inf));
			else if (filter is IPixelAutoFilter af)
				_autoFilters.Remove(_autoFilters.First(kvp => kvp.Key == af));
			else
				throw new ArgumentException("Unknown pixel filter type", nameof(filter));

			RebuildActiveFilters();
		}

		/// <summary>
		/// Sets the active state of the given <paramref name="filter"/>.
		/// </summary>
		/// <param name="filter">Filter to add.</param>
		/// <param name="isActive">Active state of the filter in the chain.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
		/// <exception cref="ArgumentException">When the type of <paramref name="filter"/> is unknown.</exception>
		public void SetIsActiveState(IPixelFilter filter, bool isActive)
		{
			ArgumentNullException.ThrowIfNull(filter);

			if (filter is IPixelValueFilter vf)
				_valueFilters[_valueFilters.IndexOf(_valueFilters.First(kvp => kvp.Key == vf))] = new KeyValuePair<IPixelValueFilter, bool>(vf, isActive);
			else if (filter is IPixelIndexFilter inf)
				_indexFilters[_indexFilters.IndexOf(_indexFilters.First(kvp => kvp.Key == inf))] = new KeyValuePair<IPixelIndexFilter, bool>(inf, isActive);
			else if (filter is IPixelAutoFilter af)
				_autoFilters[_autoFilters.IndexOf(_autoFilters.First(kvp => kvp.Key == af))] = new KeyValuePair<IPixelAutoFilter, bool>(af, isActive);
			else
				throw new ArgumentException("Unknown pixel filter type", nameof(filter));

			RebuildActiveFilters();
		}

		/// <summary>
		/// Checks if the given <paramref name="pixel"/>
		/// passes the filter.
		/// </summary>
		/// <param name="pixel">Pixel value to check.</param>
		/// <param name="index">Pixel index to check.</param>
		/// <returns>True if the <paramref name="pixel"/> and <paramref name="index"/> pass
		/// the filter, otherwise false.</returns>
		public bool Check(byte pixel, int index)
		{
			if (Mode == LogicMode.And)
			{
				if (_activeValueFilter.Count > 0)
				{
					foreach (var filter in _activeValueFilter)
					{
						if (!filter.Check(pixel))
							return false;
					}
				}

				if (_activeIndexFilter.Count > 0)
				{
					foreach (var filter in _activeIndexFilter)
					{
						if (!filter.Check(index))
							return false;
					}
				}

				if (_activeAutoFilter.Count > 0)
				{
					foreach (var filter in _activeAutoFilter)
					{
						if (!filter.Check())
							return false;
					}
				}
			}
			else
			{
				if (_activeValueFilter.Count > 0)
				{
					foreach (var filter in _activeValueFilter)
					{
						if (filter.Check(pixel))
							return true;
					}
				}

				if (_activeIndexFilter.Count > 0)
				{
					foreach (var filter in _activeIndexFilter)
					{
						if (filter.Check(index))
							return true;
					}
				}

				if (_activeAutoFilter.Count > 0)
				{
					foreach (var filter in _activeAutoFilter)
					{
						if (filter.Check())
							return true;
					}
				}

				return false;
			}

			return true;
		}

		/// <summary>
		/// Checks if the given rgb pixel pass the filter.
		/// </summary>
		/// <param name="r">R pixel to check.</param>
		/// <param name="g">G pixel to check.</param>
		/// <param name="b">B pixel to check.</param>
		/// <param name="index">Pixel index to check.</param>
		/// <returns>True if the given pixels meet the filter conditions.</returns>
		public bool Check(byte r, byte g, byte b, int index)
		{
			if (Mode == LogicMode.And)
			{
				if (_activeValueFilter.Count > 0)
				{
					foreach (var filter in _activeValueFilter)
					{
						if (!filter.Check(r) || !filter.Check(g) || !filter.Check(b))
							return false;
					}
				}

				if (_activeIndexFilter.Count > 0)
				{
					foreach (var filter in _activeIndexFilter)
					{
						if (!filter.Check(index))
							return false;
					}
				}

				if (_activeAutoFilter.Count > 0)
				{
					foreach (var filter in _activeAutoFilter)
					{
						if (!filter.Check())
							return false;
					}
				}
			}
			else
			{
				if (_activeValueFilter.Count > 0)
				{
					foreach (var filter in _activeValueFilter)
					{
						if (filter.Check(r) && filter.Check(g) && filter.Check(b))
							return true;
					}
				}

				if (_activeIndexFilter.Count > 0)
				{
					foreach (var filter in _activeIndexFilter)
					{
						if (filter.Check(index))
							return true;
					}
				}

				if (_activeAutoFilter.Count > 0)
				{
					foreach (var filter in _activeAutoFilter)
					{
						if (filter.Check())
							return true;
					}
				}

				return false;
			}

			return true;
		}

		/// <summary>
		/// Rebuilds the filter chain with the active filters.
		/// </summary>
		private void RebuildActiveFilters()
		{
			_activeValueFilter = [.. ValueFilters.Where(v => v.Value).Select(v => v.Key)];
			_activeIndexFilter = [.. IndexFilters.Where(v => v.Value).Select(v => v.Key)];
			_activeAutoFilter = [.. AutoFilters.Where(v => v.Value).Select(v => v.Key)];
		}

		/// <summary>
		/// Rebuilds the active filters when deserialized.
		/// </summary>
		/// <param name="context">Ignored.</param>
		[OnDeserialized]
		internal void OnDeserialized(StreamingContext context)
		{
			RebuildActiveFilters();
		}
	}
}