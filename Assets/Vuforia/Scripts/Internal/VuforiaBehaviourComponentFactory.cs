/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Qualcomm Confidential and Proprietary
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// Factory class that adds child class Behaviours
    /// </summary>
    public class VuforiaBehaviourComponentFactory : IBehaviourComponentFactory
    {
        #region PUBLIC_METHODS

        public MaskOutAbstractBehaviour AddMaskOutBehaviour(GameObject gameObject)
        {
            return gameObject.AddComponent<MaskOutBehaviour>();
        }

        public VirtualButtonAbstractBehaviour AddVirtualButtonBehaviour(GameObject gameObject)
        {
            return gameObject.AddComponent<VirtualButtonBehaviour>();
        }

        public TurnOffAbstractBehaviour AddTurnOffBehaviour(GameObject gameObject)
        {
#if !UNITY_EDITOR
            return gameObject.AddComponent<TurnOffBehaviour>();
#else 
            return null;
#endif
        }

        public ImageTargetAbstractBehaviour AddImageTargetBehaviour(GameObject gameObject)
        {
            return gameObject.AddComponent<ImageTargetBehaviour>();
        }

        public MarkerAbstractBehaviour AddMarkerBehaviour(GameObject gameObject)
        {
            return gameObject.AddComponent<MarkerBehaviour>();
        }

        public MultiTargetAbstractBehaviour AddMultiTargetBehaviour(GameObject gameObject)
        {
            return gameObject.AddComponent<MultiTargetBehaviour>();
        }

        public CylinderTargetAbstractBehaviour AddCylinderTargetBehaviour(GameObject gameObject)
        {
            return gameObject.AddComponent<CylinderTargetBehaviour>();
        }

        public WordAbstractBehaviour AddWordBehaviour(GameObject gameObject)
        {
            return gameObject.AddComponent<WordBehaviour>();
        }

        public TextRecoAbstractBehaviour AddTextRecoBehaviour(GameObject gameObject)
        {
            return gameObject.AddComponent<TextRecoBehaviour>();
        }

        public ObjectTargetAbstractBehaviour AddObjectTargetBehaviour(GameObject gameObject)
        {
            return gameObject.AddComponent<ObjectTargetBehaviour>();
        }

        #endregion // PUBLIC_METHODS
    }
}
