using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Destructy
{
    internal static class Fields
    {
        internal static MethodInfo weapon_controller_projectile_serverRPC = typeof(WeaponController).GetMethod("SpawnProjectile_ServerRpc", BindingFlags.Instance | BindingFlags.NonPublic);
        internal static FieldInfo weapon_controller_elapsed = typeof(WeaponController).GetField("elapsed", BindingFlags.NonPublic|BindingFlags.Instance);
    }
}
