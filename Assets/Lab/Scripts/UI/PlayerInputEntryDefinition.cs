using System;

namespace Multi2D
{
    /// <summary>
    /// Represents a player control definition that maps a device to a player number.
    /// 
    /// Equality comparison is based on PlayerNumber and IsDefined status only.
    /// This means two definitions are considered equal if they represent the same
    /// player slot, regardless of the specific device name. This design allows for
    /// device swapping while maintaining player identity.
    /// 
    /// Sorting is performed by PlayerNumber in ascending order, enabling consistent
    /// player ordering in UI displays and processing.
    /// 
    /// Note: DeviceName is not included in equality comparison to allow the same
    /// player slot to be controlled by different input devices without breaking
    /// equality contracts.
    /// </summary>
    public struct PlayerInputEntryDefinition : IEquatable<PlayerInputEntryDefinition>, IComparable<PlayerInputEntryDefinition>
    {
        /// <summary>
        /// The name of the input device (e.g., "Keyboard", "Controller1", "Gamepad").
        /// This field is informational and does not affect equality comparison.
        /// </summary>
        public string DeviceName;

        /// <summary>
        /// The player number assigned to this control definition.
        /// Used for sorting and equality comparison.
        /// </summary>
        public int PlayerNumber;

        /// <summary>
        /// Indicates whether this player slot is actively defined and in use.
        /// Used for equality comparison to distinguish between defined and undefined slots.
        /// </summary>
        public bool IsDefined;

        /// <summary>
        /// Compares this instance with another PlayerControllDefinition based on PlayerNumber.
        /// Sorting is performed in ascending order by PlayerNumber.
        /// </summary>
        /// <param name="other">The other PlayerControllDefinition to compare with.</param>
        /// <returns>
        /// A value less than zero if this instance precedes other in the sort order;
        /// zero if this instance occurs in the same position in the sort order as other;
        /// a value greater than zero if this instance follows other in the sort order.
        /// </returns>
        public int CompareTo(PlayerInputEntryDefinition other) => PlayerNumber.CompareTo(other.PlayerNumber);

        /// <summary>
        /// Determines whether this instance is equal to another PlayerControllDefinition.
        /// Two definitions are considered equal if they have the same PlayerNumber and IsDefined status.
        /// The DeviceName is intentionally excluded from equality comparison to allow device swapping
        /// while maintaining player identity consistency.
        /// </summary>
        /// <param name="other">The other PlayerControllDefinition to compare with.</param>
        /// <returns>
        /// true if the specified PlayerControllDefinition is equal to this instance; otherwise, false.
        /// </returns>
        public bool Equals(PlayerInputEntryDefinition other) => PlayerNumber == other.PlayerNumber && IsDefined == other.IsDefined;
    }
}
