using MilitaryGame.Building;
using UnityEngine;

namespace Interfaces.MilitaryGame
{
    public interface IAttackable
    {
        public void Attack(IDamageable damageableObject);
    }
}
