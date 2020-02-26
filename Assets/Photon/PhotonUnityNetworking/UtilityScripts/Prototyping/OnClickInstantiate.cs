// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnClickInstantiate.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities, 
// </copyright>
// <summary>
//  This component will instantiate a network GameObject when in a room and the user click on that component's Gameobject
//  Uses PhysicsRaycaster for positioning
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Photon.Pun;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
    /// <summary>
    /// This component will instantiate a network GameObject when in a room and the user click on that component's Gameobject
    /// Uses PhysicsRaycaster for positioning
    /// </summary>
    public class OnClickInstantiate : MonoBehaviour, IPointerClickHandler
    {
        public GameObject Prefab;
        public int InstantiateType;
        private string[] InstantiateTypeNames = { "Mine", "Scene" };

        public bool showGui;


        void OnGUI()
        {
            if (showGui)
            {
                GUILayout.BeginArea(new Rect(Screen.width - 180, 0, 180, 50));
                InstantiateType = GUILayout.Toolbar(InstantiateType, InstantiateTypeNames);
                GUILayout.EndArea();
            }
        }


        #region IPointerClickHandler implementation
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            //TODO reimplement
            if (!PhotonNetwork.InRoom)
            {
                // only use PhotonNetwork.Instantiate while in a room.
                return;
            }

            switch (InstantiateType)
            {
                case 0:
                    PhotonNetwork.Instantiate(Prefab.name, eventData.pointerCurrentRaycast.worldPosition + new Vector3(0, 5f, 0), Quaternion.identity, 0);
                    break;
                case 1:
                    PhotonNetwork.InstantiateSceneObject(Prefab.name, eventData.pointerCurrentRaycast.worldPosition + new Vector3(0, 5f, 0), Quaternion.identity, 0, null);
                    break;
            }
        }
        #endregion
    }
}