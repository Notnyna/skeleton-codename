using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{

    /// <summary>
    /// Is not for generating a 'humus'.
    /// Handles the surface level part and component management to act as a global character interactor.
    /// For a more visual description it is a smaller component, possibly a live being that the ground consists of.
    /// For a more graphical description it is a continuously decomposing pile of mud.
    /// -Used by staticposoverride.
    /// </summary>
    public class Humus : MonoBehaviour
    {
        private List<Transform> bodyparts;
        private List<Transform> inventory;

        private void Awake()
        {
            bodyparts = new List<Transform>();
            inventory = new List<Transform>();
            UpdateParts();
        }

        #region Containers 
        private void CompareAdd(List<Transform> L, Transform parent)
        {
            Transform[] parts = parent.GetComponentsInChildren<Transform>();
            for (int i = 1; i < parts.Length; i++)
            {
                if (!L.Contains(parts[i]))
                {
                    L.Add(parts[i]);
                    //Debug.LogFormat("{0} Added", parts[i].name);
                }
            }

        }

        public void UpdateParts()
        {
            //GetComponentInChildren<General.StaticPos>().UpdatePositions();
            foreach (Transform p in transform)
            {
                if (p.name == "bodyparts") { CompareAdd(bodyparts, p); }
                if (p.name == "inventory") { CompareAdd(inventory, p); }
            }
        }

        public Transform GetByName(string n, int type)
        {
            List<Transform> S = null;
            if (type == 0) { S = bodyparts; }
            if (type == 1) { S = inventory; }

            if (S != null)
            {
                foreach (Transform item in S)
                {
                    //Debug.LogFormat("{0} == {1}", n,item.name);
                    if (string.Equals(n, item.name))
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        public T GetByType<T>()
        {
            return gameObject.GetComponentInChildren<T>();
        }
        #endregion

    }
}
