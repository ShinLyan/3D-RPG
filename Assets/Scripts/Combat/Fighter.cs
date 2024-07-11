using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float _weaponRange = 2f;

        private Transform _target;

        private void Update()
        {
            if (!_target) return;

            if (GetIsInRange())
            {
                GetComponent<Mover>().Stop();
            }
            else
            {
                GetComponent<Mover>().MoveTo(_target.position);
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < _weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.transform;
        }

        public void Cancel()
        {
            _target = null;
        }
    }
}
