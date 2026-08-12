using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BusinessCardProject.Client.Entities.Enums
{
    internal abstract class Enumeration(Guid id, string name, int sequenceNum) : IComparable
    {
        [Required] public string Name { get; } = name;

        public Guid Id { get; } = id;

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int SequenceNumber { get; } = sequenceNum;

        public override string ToString() => Name;

        private static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();

        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public bool Equals(Enumeration other)
        {
            return Name == other.Name && Id.Equals(other.Id) && SequenceNumber == other.SequenceNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Id, SequenceNumber);
        }

        public int CompareTo(object? other) => Id.CompareTo(((Enumeration)other!).Id);
    }
}