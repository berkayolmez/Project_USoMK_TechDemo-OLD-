using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class WeaponPickUp : IPickable
    {
        SphereCollider myCollider;
        public SpellItem spell;

        private void Start()
        {
            myCollider = gameObject.AddComponent<SphereCollider>();
            myCollider.center = Vector3.zero;
            myCollider.isTrigger = true;
           //myPickableIcon = spell.itemIcon;
        }

        public override void NearByObject()
        {
           
        }

        public override void PickInteract(PlayerManager playerManager)//pickup item to inventory
        {            
            PickUpItem(playerManager);
        }

        private void Update()
        {
            myCollider.radius = radius;
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            PlayerAnimatorManager animatorManager;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            //playerLocomotion.cController.Move(Vector3.zero); //stop the player when picking up item //i guess its bad idea. This can slow down the tempo of game. I dont want this
            animatorManager.PlayTargetAnimation("PickUpObj", false);
            playerInventory.spellsInventory.Add(spell);
            Destroy(gameObject);

        }
    }
}