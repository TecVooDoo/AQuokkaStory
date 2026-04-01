using UnityEngine;
using System.Reflection;

namespace AQS.Test
{
    public class WeaponDebug : MonoBehaviour
    {
        private Component _wm;
        private System.Type _wmType;
        private PropertyInfo _activeProp;
        private PropertyInfo _weaponIsActiveProp;
        private PropertyInfo _hasWeaponProp;
        private PropertyInfo _weaponProp;
        private PropertyInfo _weaponModeProp;
        private PropertyInfo _combatModeProp;
        private MethodInfo _mainAttackMethod;

        private void Start()
        {
            foreach (Assembly asm in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                _wmType = asm.GetType("MalbersAnimations.MWeaponManager");
                if (_wmType != null) break;
            }

            _wm = GetComponent(_wmType);
            if (_wm == null)
            {
                Debug.Log("[WeaponDebug] No MWeaponManager found!");
                return;
            }

            _activeProp = _wmType.GetProperty("Active", BindingFlags.Instance | BindingFlags.Public);
            _weaponIsActiveProp = FindProp(_wmType, "WeaponIsActive");
            _hasWeaponProp = FindProp(_wmType, "HasWeapon");
            _weaponProp = _wmType.GetProperty("Weapon", BindingFlags.Instance | BindingFlags.Public);
            _weaponModeProp = FindProp(_wmType, "WeaponMode");
            _combatModeProp = _wmType.GetProperty("CombatMode", BindingFlags.Instance | BindingFlags.Public);

            _mainAttackMethod = _wmType.GetMethod("MainAttack", BindingFlags.Instance | BindingFlags.Public, null,
                new System.Type[] { typeof(bool) }, null);

            Debug.Log("[WeaponDebug] Ready. Press T to force MainAttack(true). LMB is monitored.");
            Invoke(nameof(LogState), 1f);
        }

        private void LogState()
        {
            if (_wm == null) return;

            bool active = (bool)(_activeProp?.GetValue(_wm) ?? false);
            bool weaponIsActive = (bool)(_weaponIsActiveProp?.GetValue(_wm) ?? false);
            bool hasWeapon = (bool)(_hasWeaponProp?.GetValue(_wm) ?? false);
            object weapon = _weaponProp?.GetValue(_wm);
            object weaponMode = _weaponModeProp?.GetValue(_wm);
            object combatMode = _combatModeProp?.GetValue(_wm);

            PropertyInfo hasInputProp = FindProp(_wmType, "HasInput");
            bool hasInput = hasInputProp != null && (bool)hasInputProp.GetValue(_wm);

            PropertyInfo mInputProp = FindProp(_wmType, "MInput");
            object mInput = mInputProp?.GetValue(_wm);

            Debug.Log("[WeaponDebug] Active=" + active + " WeaponIsActive=" + weaponIsActive
                + " HasWeapon=" + hasWeapon
                + " Weapon=" + (weapon != null ? weapon.ToString() : "null")
                + " WeaponMode=" + (weaponMode != null ? weaponMode.ToString() : "null")
                + " CombatMode=" + combatMode
                + " HasInput=" + hasInput
                + " MInput=" + (mInput != null ? mInput.ToString() : "null"));

            if (weapon != null)
            {
                System.Type wt = weapon.GetType();
                PropertyInfo wActive = wt.GetProperty("Active", BindingFlags.Instance | BindingFlags.Public);
                PropertyInfo canAttack = wt.GetProperty("CanAttack", BindingFlags.Instance | BindingFlags.Public);
                PropertyInfo projProp = FindProp(wt, "Projectile");
                PropertyInfo projParent = FindProp(wt, "ProjectileParent");
                PropertyInfo aimOrigin = FindProp(wt, "AimOrigin");
                PropertyInfo projInstance = FindProp(wt, "ProjectileInstance");
                PropertyInfo hasAmmo = FindProp(wt, "HasAmmo");

                object proj = projProp?.GetValue(weapon);
                object parent = projParent?.GetValue(weapon);
                object aim = aimOrigin?.GetValue(weapon);
                object inst = projInstance?.GetValue(weapon);

                Debug.Log("[WeaponDebug] Weapon.Active=" + wActive?.GetValue(weapon)
                    + " CanAttack=" + canAttack?.GetValue(weapon)
                    + " HasAmmo=" + hasAmmo?.GetValue(weapon)
                    + " Projectile=" + (proj != null ? proj.ToString() : "NULL")
                    + " ProjectileParent=" + (parent != null ? parent.ToString() : "NULL")
                    + " AimOrigin=" + (aim != null ? aim.ToString() : "NULL")
                    + " ProjectileInstance=" + (inst != null ? inst.ToString() : "NULL"));
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("[WeaponDebug] T pressed -- calling MainAttack(true) ONLY");
                LogState();
                _mainAttackMethod?.Invoke(_wm, new object[] { true });
            }

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("[WeaponDebug] LMB detected by Unity Input.GetMouseButtonDown(0)");
            }
        }

        private static PropertyInfo FindProp(System.Type t, string name)
        {
            System.Type cur = t;
            while (cur != null)
            {
                PropertyInfo p = cur.GetProperty(name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (p != null) return p;
                cur = cur.BaseType;
            }
            return null;
        }
    }
}
