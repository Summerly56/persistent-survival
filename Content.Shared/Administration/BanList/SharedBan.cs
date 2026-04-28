using Content.Shared.Database;
using Robust.Shared.Network;
using Robust.Shared.Serialization;
using System.Collections.Immutable;

namespace Content.Shared.Administration.BanList;

[Serializable, NetSerializable]
public record SharedBan(
    int? Id,
    BanType Type,
    ImmutableArray<NetUserId> UserIds,
    ImmutableArray<(string address, int cidrMask)> Addresses,
    ImmutableArray<string> HWIds,
    DateTime BanTime,
    DateTime? ExpirationTime,
    string Reason,
    string? BanningAdminName,
    SharedUnban? Unban,
    ImmutableArray<BanRoleDef>? Roles);
