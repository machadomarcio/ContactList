using System;

namespace ContactList.Domain.Entities.Base
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public class Entity
    {
        #region Properties

        /// <summary>
        /// Get the persisten object identifier
        /// </summary>
        public virtual Guid Id { get; set; }

        public virtual DateTime InsertDate { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if this entity is transient, ie, without identity at this moment
        /// </summary>
        /// <returns>True if entity is transient, else false</returns>
        public bool IsTransient()
        {
            return Id == new Guid();
        }

        #endregion

        #region Overrides Methods

        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Entity)) return false;

            if (ReferenceEquals(this, obj)) return true;

            var item = (Entity)obj;

            if (item.IsTransient() || IsTransient()) return false;

            return item.Id == Id;
        }

        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            
            if (IsTransient()) return base.GetHashCode(); //NOSONAR

            return Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
        }

        #endregion
    }
}
