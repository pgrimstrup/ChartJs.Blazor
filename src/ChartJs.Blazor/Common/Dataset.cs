﻿using ChartJs.Blazor.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChartJs.Blazor.Common
{
    /// <summary>
    /// Represents a dataset containing a collection of values.
    /// </summary>
    /// <typeparam name="T">The type of data this <see cref="Dataset{T}"/> contains.</typeparam>
    [JsonObject]
    public abstract class Dataset<T> : Collection<T>, IDataset<T>
    {
        /// <summary>
        /// Gets the ID of this dataset. Used to keep track of the datasets
        /// across the .NET &lt;-&gt; JavaScript boundary.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the data contained in this dataset. This property is read-only.
        /// This is in addition to implementing <see cref="IList{T}"/>.
        /// </summary>
        public IReadOnlyList<T> Data { get; }

        /// <summary>
        /// Gets the <see cref="ChartType"/> this dataset is for.
        /// Important to set in mixed charts.
        /// </summary>
        public ChartType Type { get; }

        /// <summary>
        /// Creates a new <see cref="Dataset{T}"/>.
        /// </summary>
        /// <param name="type">The <see cref="ChartType"/> this dataset is for.</param>
        /// <param name="id">The id for this dataset. If <see langword="null"/>,
        /// a random GUID string will be used.</param>
        protected Dataset(ChartType type, string id = null) : base(new List<T>())
        {
            Data = new ReadOnlyCollection<T>(Items);
            Id = id ?? Guid.NewGuid().ToString();
            Type = type;
        }

        /// <summary>
        /// Sets the dataset to be hidden when rendered. The label will still show in the Legend so
        /// that the user can click on it and show the data.
        /// </summary>
        public bool? Hidden { get; set; }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="Dataset{T}"/>.
        /// </summary>
        /// <param name="items">
        /// The collection whose elements should be added to the end of the <see cref="Dataset{T}"/>.
        /// </param>
        public void AddRange(IEnumerable<T> items) => ((List<T>)Items).AddRange(items ?? throw new ArgumentNullException(nameof(items)));

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="Dataset{T}"/>.
        /// </summary>
        /// <param name="items">
        /// The collection whose elements should be added to the end of the <see cref="Dataset{T}"/>.
        /// </param>
        public void AddRange(params T[] items) => AddRange(items as IEnumerable<T>);

        /// <summary>
        /// Determines whether the specified object is considered equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true"/> if the specified object is considered to be equal
        /// to the current object; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => obj is Dataset<T> set &&
                Id == set.Id &&
                EqualityComparer<IList<T>>.Default.Equals(Items, set.Items);

        /// <summary>
        /// Returns the hash code for this <see cref="Dataset{T}"/>.
        /// </summary>
        /// <returns>The hash code for this <see cref="Dataset{T}"/>.</returns>
        public override int GetHashCode() => HashCode.Combine(Items, Id);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool operator ==(Dataset<T> left, Dataset<T> right) =>
                EqualityComparer<Dataset<T>>.Default.Equals(left, right);

        public static bool operator !=(Dataset<T> left, Dataset<T> right) => !(left == right);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
