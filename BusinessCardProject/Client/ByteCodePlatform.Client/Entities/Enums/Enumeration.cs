using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BusinessCardProject.Client.Entities.Enums
{
    internal abstract class Enumeration
        : IComparable
    {
        [Required] protected string Name { get; private set; }

        protected Guid Id { get; private set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        protected int SequenceNumber { get; private set; }

        protected Enumeration(Guid id, string name, int sequenceNum)
            => (Id, Name, SequenceNumber) = (id, name, sequenceNum);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public static T FromValue<T>(Guid value) where T : Enumeration
        {
            var matchingItem = Parse<T, Guid>(value, "value", item => item.Id == value);
            return matchingItem;
        }

        private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}