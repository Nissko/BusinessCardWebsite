using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Rate
{
    /// <summary>
    /// Сущность рейтинга
    /// TODO: Продумать, чтобы пользователь не мог оставлять несколько раз одинаковую оценку, имел возможность отредактировать уже написанный отзыв
    /// </summary>
    internal class RateEntity : Entity
    {
        /// <summary>
        /// Оценка курса
        /// </summary>
        private double _estimation;
        
        //TODO: Добавить связь с курсом
        
        //TODO: Добавить связь с оценившим пользователем
    }
}

